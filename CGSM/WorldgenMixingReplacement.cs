// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei;
using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;
using UnityEngine;

namespace CGSM;

/// <summary>
/// Replacement for the stock ProcGenGame.WorldgenMixing.DoWorldMixingInternal.
///
/// This implementation intentionally diverges from stock behavior by treating
/// "already present" mix-in worlds as satisfying the guarantee, rather than
/// assuming they don't exist in the cluster.
/// </summary>
public static class WorldgenMixingReplacement
{
    private static bool EnforceTagCompatibility = true;

    private const string KleiGuaranteeLevelId = "GuranteeMixing"; // yes, Klei spelled it this way
    private const string AlternateGuaranteeLevelId = "GuaranteeMixing"; // just in case

    private sealed class MixingEntry
    {
        public string worldPath;
        public PlanetoidType? planetoidType;
        public string identityKey;
        public string settingId;
        public string settingWorldgenPath;
        public string levelId;
        public WorldMixingSettings mixingSettings;
        public ProcGen.World cachedWorld;

        public override string ToString()
            => planetoidType.HasValue
                ? $"{worldPath} (type={planetoidType.Value}, setting={settingId}, level={levelId})"
                : $"{worldPath} (setting={settingId}, level={levelId})";
    }

    private sealed class MixingContext
    {
        public ClusterLayout clusterLayout;
        public string startWorldPath;
        public KRandom rng;

        // "Already present" tracking. Prefer PlanetoidType identity when known.
        public HashSet<string> presentWorldIdentities;
        public HashSet<WorldPlacement> pinnedPlacements;
        public List<WorldPlacement> preferredCandidates;
        public List<WorldPlacement> nonPreferredCandidates;
    }

    private static bool TryGetPlanetoidType(string worldPath, out PlanetoidType planetoidType)
        => PlanetoidInfos.TryLookupTypeByWorldPath(worldPath, out planetoidType);

    private static string GetWorldIdentityKey(string worldPath)
    {
        if (string.IsNullOrWhiteSpace(worldPath))
            return "path:" + (worldPath ?? string.Empty);

        if (TryGetPlanetoidType(worldPath, out var planetoidType))
        {
            // Treat some planetoid types as equivalent for mixing identity.
            var normalized = Planetoids.NormalizeForMixing(planetoidType);
            return $"type:{(int)normalized}";
        }

        return $"path:{worldPath}";
    }

    private static void AddToCategory(
        MixingEntry entry,
        List<MixingEntry> disabled,
        List<MixingEntry> guaranteed,
        List<MixingEntry> bestEffort)
    {
        if (entry != null && string.IsNullOrWhiteSpace(entry.identityKey))
            entry.identityKey = GetWorldIdentityKey(entry.worldPath);

        // If multiple settings target the same world, pick a single effective entry.
        // Priority: guaranteed > best-effort > disabled.
        // (This can be revisited if modded clusters ever rely on multiple entries.)

        List<MixingEntry> FindListForLevel(string lvl)
        {
            if (string.Equals(lvl, WorldMixingSettingConfig.DisabledLevelId, StringComparison.OrdinalIgnoreCase)) {
                return disabled;
            }
            if (string.Equals(lvl, KleiGuaranteeLevelId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(lvl, AlternateGuaranteeLevelId, StringComparison.OrdinalIgnoreCase)) {
                return guaranteed;
            }
            return bestEffort;
        }

        int PriorityForList(List<MixingEntry> list)
        {
            if (ReferenceEquals(list, guaranteed)) return 3;
            if (ReferenceEquals(list, bestEffort)) return 2;
            return 1;
        }

        var targetList = FindListForLevel(entry.levelId);
        int targetPrio = PriorityForList(targetList);

        // If it already exists in any category, keep the highest priority.
        MixingEntry existing = disabled.FirstOrDefault(m => string.Equals(m.identityKey, entry.identityKey, StringComparison.OrdinalIgnoreCase))
                             ?? bestEffort.FirstOrDefault(m => string.Equals(m.identityKey, entry.identityKey, StringComparison.OrdinalIgnoreCase))
                             ?? guaranteed.FirstOrDefault(m => string.Equals(m.identityKey, entry.identityKey, StringComparison.OrdinalIgnoreCase));

        if (existing != null) {
            // Find which list currently holds it.
            List<MixingEntry> existingList = disabled.Contains(existing) ? disabled :
                                             bestEffort.Contains(existing) ? bestEffort :
                                             guaranteed;
            int existingPrio = PriorityForList(existingList);
            if (targetPrio <= existingPrio) {
                Util.LogDbg("WorldMixing: ignoring duplicate mixing target {0}; keeping {1} over {2}", entry.worldPath, existing.levelId, entry.levelId);
                return;
            }

            existingList.Remove(existing);
        }

        targetList.Add(entry);
    }

    public static MutatedClusterLayout DoWorldMixingInternal(
        MutatedClusterLayout mutatedClusterLayout,
        int seed,
        bool isRunningWorldgenDebug,
        bool muteErrors) {

        Util.LogDbg("DoWorldMixingInternal start");

        if (mutatedClusterLayout?.layout?.worldPlacements == null) {
            return mutatedClusterLayout;
        }

        var clusterLayout = mutatedClusterLayout.layout;
        var rng = new KRandom(seed);

        // Reset previous mixing state.
        foreach (var wp in clusterLayout.worldPlacements) {
            wp.UndoWorldMixing();
        }

        // 1) Classify all world mixing settings.
        GetMixingEntries(
            clusterLayout,
            out List<MixingEntry> disabledMixings,
            out List<MixingEntry> guaranteedMixings,
            out List<MixingEntry> bestEffortMixings);

        var disabledWorldIdentities = new HashSet<string>(disabledMixings.Select(m => m.identityKey), StringComparer.OrdinalIgnoreCase);
        var guaranteedWorldIdentities = new HashSet<string>(guaranteedMixings.Select(m => m.identityKey), StringComparer.OrdinalIgnoreCase);
        var bestEffortWorldIdentities = new HashSet<string>(bestEffortMixings.Select(m => m.identityKey), StringComparer.OrdinalIgnoreCase);

        var ctx = new MixingContext{
            clusterLayout = clusterLayout,
            startWorldPath = clusterLayout.GetStartWorld(),
            rng = rng,

            // Used for fast "already present" checks.
            presentWorldIdentities = new HashSet<string>(clusterLayout.worldPlacements.Select(wp => GetWorldIdentityKey(wp.world)), StringComparer.OrdinalIgnoreCase),
            pinnedPlacements = new HashSet<WorldPlacement>(),
            preferredCandidates = new List<WorldPlacement>(),
            nonPreferredCandidates = new List<WorldPlacement>(),
        };

        // 3) Pin start and warp worlds.
        foreach (var wp in clusterLayout.worldPlacements) {
            if (Planetoids.IsStartWorld(wp, ctx.startWorldPath) ||
                Planetoids.IsWarpWorld(wp.world)) {
                ctx.pinnedPlacements.Add(wp);
            }
        }

        // Initial pass: preferred candidates in mixing placements
        foreach (var wp in clusterLayout.worldPlacements) {
            if (!EligibleAsCandidate(ctx, wp, true)) {
                continue;
            }

            if (disabledWorldIdentities.Contains(GetWorldIdentityKey(wp.world))) {
                AddUnique(ctx.preferredCandidates, wp);
                continue;
            }
        }

        // Second pass: preferred candidate in non-mixing placements + unmanaged non-preferred candidates.
        foreach (var wp in clusterLayout.worldPlacements) {
            if (!EligibleAsCandidate(ctx, wp, false)) {
                continue;
            }

            if (disabledWorldIdentities.Contains(GetWorldIdentityKey(wp.world))) {
                AddUnique(ctx.preferredCandidates, wp);
                continue;
            }

            var wpIdentity = GetWorldIdentityKey(wp.world);
            if (!guaranteedWorldIdentities.Contains(wpIdentity) && !bestEffortWorldIdentities.Contains(wpIdentity))
            {
                AddUnique(ctx.nonPreferredCandidates, wp);
            }
        }

        // Third pass: append best-effort worlds at the end of the ordered non-preferred set.
        foreach (var wp in clusterLayout.worldPlacements)
        {
            if (!EligibleAsCandidate(ctx, wp, false)) {
                continue;
            }

            if (bestEffortWorldIdentities.Contains(GetWorldIdentityKey(wp.world))) {
                AddUnique(ctx.nonPreferredCandidates, wp);
            }
        }

        // Deterministic randomization: sort then shuffle with the worldgen RNG.
        guaranteedMixings = guaranteedMixings.OrderBy(m => m.worldPath, StringComparer.OrdinalIgnoreCase).ToList();
        bestEffortMixings = bestEffortMixings.OrderBy(m => m.worldPath, StringComparer.OrdinalIgnoreCase).ToList();
        guaranteedMixings.ShuffleSeeded(rng);
        bestEffortMixings.ShuffleSeeded(rng);

        // 4) Insert guaranteed mixings.
        ApplyMixingEntries(
            categoryName: "guaranteed",
            mixings: guaranteedMixings,
            ctx: ctx,
            isRunningWorldgenDebug: isRunningWorldgenDebug,
            muteErrors: muteErrors);

        // 5) Insert best-effort mixings.
        ApplyMixingEntries(
            categoryName: "best-effort",
            mixings: bestEffortMixings,
            ctx: ctx,
            isRunningWorldgenDebug: isRunningWorldgenDebug,
            muteErrors: muteErrors);

        // Late, destructive cluster mutations:
        // - Only apply for the *real* generation (not destination-screen preview / debug)
        // - Only apply if Launch() requested it
        //
        // Important: mutate the *mixed* clusterLayout we were given (mutatedClusterLayout.layout),
        // not CustomGameSettings.GetCurrentClusterLayout().
        try {
            if (!isRunningWorldgenDebug
                && Mod.Instance.gameState.ConsumeLateWorldgenMutationsRequest()) {

                Util.LogDbg("WorldMixing: applying late cluster mutations");

                WorldMixing.deleteDisabledWorlds(Mod.Instance.gameState.cgs, clusterLayout);
                WorldMixing.deleteDuplicateWorlds(clusterLayout);
                WorldMixing.disableInjectedWorldTraits(Mod.Instance.gameState.cgs, clusterLayout);
                Mod.Instance.gameState.worldMixing.disableStoryTraits(clusterLayout);

                Util.Log("WorldMixing: Final clusterLayout:\n{0}",
                    ClusterLayoutToDebugString(clusterLayout));
            }
        } catch {
            // never fail worldgen due to our patch
        }

        return mutatedClusterLayout;
    }

    private static void AddUnique(List<WorldPlacement> list, WorldPlacement wp) {
        if (!list.Contains(wp))
            list.Add(wp);
    }

    private static string ClusterLayoutToDebugString(ClusterLayout clusterLayout)
    {
        if (clusterLayout?.worldPlacements == null)
            return "<null>";

        var startWorldPath = clusterLayout.GetStartWorld();
        var sb = new StringBuilder();
        sb.AppendFormat("  worldPlacements={0}", clusterLayout.worldPlacements.Count);
        sb.AppendLine();

        for (int i = 0; i < clusterLayout.worldPlacements.Count; i++)
        {
            var wp = clusterLayout.worldPlacements[i];
            bool isStart = Planetoids.IsStartWorld(wp, startWorldPath);
            bool isWarp = Planetoids.IsWarpWorld(wp.world);
            bool isMixingPlacement = wp.IsMixingPlacement();

            sb.AppendFormat(
                "  [{0}] world={1} start={2} warp={3} mixingPlacement={4}",
                i,
                wp.world,
                isStart,
                isWarp,
                isMixingPlacement);

            if (wp.worldMixing != null && wp.worldMixing.mixingWasApplied)
                sb.AppendFormat(" previousWorld={0}", wp.worldMixing.previousWorld);

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static bool EligibleAsCandidate(MixingContext ctx, WorldPlacement wp,
        bool restrictToMixing) {

        if (ctx.pinnedPlacements.Contains(wp))
            return false;

        // Extra paranoia (should be redundant with pinnedPlacements).
        if (Planetoids.IsStartWorld(wp, ctx.startWorldPath) || Planetoids.IsWarpWorld(wp.world)) {
            return false;
        }

        // Easy-to-loosen restriction: only replace placements marked as mixing placements.
        if (restrictToMixing && !wp.IsMixingPlacement()) {
            Util.LogDbg("WorldMixing: candidate ineligible (not a mixing placement): {0}", wp.world);
            return false;
        }

        return true;
    }

    private static void PinPlacement(MixingContext ctx, WorldPlacement wp) {
        ctx.pinnedPlacements.Add(wp);
        ctx.preferredCandidates.Remove(wp);
        ctx.nonPreferredCandidates.Remove(wp);
    }

    private static void PinAllPlacementsForWorld(MixingContext ctx, string worldPath) {
        foreach (var wp in ctx.clusterLayout.worldPlacements) {
            if (string.Equals(wp.world, worldPath, StringComparison.OrdinalIgnoreCase))
                PinPlacement(ctx, wp);
        }
    }

    private static void PinAllPlacementsForMixingEntry(MixingContext ctx, MixingEntry mixing)
    {
        if (mixing == null)
            return;

        // If the mixing target corresponds to a known PlanetoidType, pin by type instead of by
        // raw world path. This avoids treating Start vs Other variants as distinct worlds.
        if (mixing.planetoidType.HasValue)
        {
            var targetType = Planetoids.NormalizeForMixing(mixing.planetoidType.Value);
            foreach (var wp in ctx.clusterLayout.worldPlacements)
            {
                if (TryGetPlanetoidType(wp.world, out var wpType) &&
                    Planetoids.NormalizeForMixing(wpType) == targetType)
                    PinPlacement(ctx, wp);
            }
            return;
        }

        // Fallback: pin by exact path.
        PinAllPlacementsForWorld(ctx, mixing.worldPath);
    }

    private static void GetMixingEntries(
        ClusterLayout clusterLayout,
        out List<MixingEntry> disabledMixings,
        out List<MixingEntry> guaranteedMixings,
        out List<MixingEntry> bestEffortMixings)
    {
        var disabled = new List<MixingEntry>();
        var guaranteed = new List<MixingEntry>();
        var bestEffort = new List<MixingEntry>();

        if (Mod.Instance.gameState.cgs != null && !GenericGameSettings.instance.devAutoWorldGen) {
            // "All mixing settings" (not only active) so we can detect disabled targets.
            foreach (var kvp in Mod.Instance.gameState.cgs.MixingSettings) {
                if (kvp.Value is not WorldMixingSettingConfig wm) {
                    continue;
                }

                SettingLevel level = null;
                try { level = Mod.Instance.gameState.cgs.GetCurrentMixingSettingLevel(wm); } catch { /* ignore */ }
                if (level == null || string.IsNullOrWhiteSpace(level.id)) {
                    continue;
                }

                WorldMixingSettings mixingSettings = null;
                try { mixingSettings = SettingsCache.TryGetCachedWorldMixingSetting(wm.worldgenPath); } catch { /* ignore */ }
                if (mixingSettings == null || string.IsNullOrWhiteSpace(mixingSettings.world)) {
                    continue;
                }

                // Respect forbidden cluster tags (stock behavior).
                if (clusterLayout.HasAnyTags(mixingSettings.forbiddenClusterTags)) {
                    continue;
                }

                ProcGen.World worldData = null;
                try { worldData = SettingsCache.worlds.GetWorldData(mixingSettings.world); } catch { /* ignore */ }
                if (worldData == null) {
                    continue;
                }

                AddToCategory(new MixingEntry{
                    worldPath = mixingSettings.world,
                    planetoidType = TryGetPlanetoidType(mixingSettings.world, out var pType) ? pType : null,
                    identityKey = GetWorldIdentityKey(mixingSettings.world),
                    settingId = wm.id,
                    settingWorldgenPath = wm.worldgenPath,
                    levelId = level.id,
                    mixingSettings = mixingSettings,
                    cachedWorld = worldData,
                }, disabled, guaranteed, bestEffort);
            }
        } else {
            // Dev mode: use GenericGameSettings.devWorldMixing as guaranteed entries.
            string[] devWorldMixing = GenericGameSettings.instance.devWorldMixing;
            for (int i = 0; i < devWorldMixing.Length; i++) {
                WorldMixingSettings mixingSettings = null;
                try { mixingSettings = SettingsCache.TryGetCachedWorldMixingSetting(devWorldMixing[i]); } catch { /* ignore */ }
                if (mixingSettings == null || string.IsNullOrWhiteSpace(mixingSettings.world)) {
                    continue;
                }

                if (clusterLayout.HasAnyTags(mixingSettings.forbiddenClusterTags)) {
                    continue;
                }

                ProcGen.World worldData = null;
                try { worldData = SettingsCache.worlds.GetWorldData(mixingSettings.world); } catch { /* ignore */ }
                if (worldData == null) {
                    continue;
                }

                AddToCategory(new MixingEntry{
                    worldPath = mixingSettings.world,
                    planetoidType = TryGetPlanetoidType(mixingSettings.world, out var pType) ? pType : null,
                    identityKey = GetWorldIdentityKey(mixingSettings.world),
                    settingId = $"devWorldMixing[{i}]",
                    settingWorldgenPath = devWorldMixing[i],
                    levelId = KleiGuaranteeLevelId,
                    mixingSettings = mixingSettings,
                    cachedWorld = worldData,
                }, disabled, guaranteed, bestEffort);
            }
        }

        disabledMixings = disabled;
        guaranteedMixings = guaranteed;
        bestEffortMixings = bestEffort;
    }

    private static void ApplyMixingEntries(
        string categoryName,
        List<MixingEntry> mixings,
        MixingContext ctx,
        bool isRunningWorldgenDebug,
        bool muteErrors) {

        foreach (var mixing in mixings) {
            if (string.IsNullOrWhiteSpace(mixing?.worldPath) || mixing.cachedWorld == null) {
                continue;
            }

            if (ctx.presentWorldIdentities.Contains(mixing.identityKey)) {
                // Treat "already present" as satisfying the mixing, and prevent it from being replaced.
                PinAllPlacementsForMixingEntry(ctx, mixing);
                continue;
            }

            // Attempt replacement.
            WorldPlacement replacement = SelectReplacementCandidate(
                rng: ctx.rng,
                mixing: mixing,
                preferredCandidates: ctx.preferredCandidates,
                nonPreferredCandidates: ctx.nonPreferredCandidates);

            if (replacement == null) {
                // Stock would throw for unsatisfied guarantees. For CGSM we want to be resilient.
                string msg = $"WorldMixing: could not apply {categoryName} mixing for {mixing.worldPath}; no compatible replacement candidates remain.";
                if (muteErrors) {
                    Util.LogDbg(msg);
                } else {
                    Util.Log(msg);
                }
                continue;
            }

            string oldWorld = replacement.world;
            replacement.worldMixing.previousWorld = replacement.world;
            replacement.worldMixing.mixingWasApplied = true;
            replacement.world = mixing.worldPath;
            ctx.presentWorldIdentities.Add(mixing.identityKey);

            Util.LogDbg("WorldMixing: substitution ({0}) {1} -> {2} (setting={3}, level={4})",
                categoryName,
                oldWorld,
                mixing.worldPath,
                mixing.settingId,
                mixing.levelId);

            // 4/5) Pin and remove from candidate lists.
            PinPlacement(ctx, replacement);
        }
    }

    private static WorldPlacement SelectReplacementCandidate(
        KRandom rng,
        MixingEntry mixing,
        List<WorldPlacement> preferredCandidates,
        List<WorldPlacement> nonPreferredCandidates)
    {
        static HashSet<string> Exempt(params string[] tags)
            => tags == null || tags.Length == 0
                ? null
                : new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase);

        var exemptionSets = new[] {
            Exempt(),
            Exempt("Mixing"),
            Exempt("Mixing", "SmallWorld"),
            Exempt("Mixing", "SmallWorld", "Challenge"),
        };

        foreach (var exemptTags in exemptionSets) {
            var compatiblePreferred = preferredCandidates
                .Where(wp => IsWorldCompatibleWithPlacement(mixing, wp, exemptTags))
                .ToList();
            if (compatiblePreferred.Count > 0) {
                compatiblePreferred.ShuffleSeeded(rng);
                return compatiblePreferred[0];
            }
        }

        foreach (var exemptTags in exemptionSets) {
            var compatibleNonPreferred = nonPreferredCandidates
                .Where(wp => IsWorldCompatibleWithPlacement(mixing, wp, exemptTags))
                .ToList();
            if (compatibleNonPreferred.Count > 0) {
                compatibleNonPreferred.ShuffleSeeded(rng);
                return compatibleNonPreferred[0];
            }
        }

        return null;
    }

    private static bool IsWorldCompatibleWithPlacement(MixingEntry mixing, WorldPlacement placement)
        => IsWorldCompatibleWithPlacement(mixing, placement, exemptRequiredTags: null);

    private static bool IsWorldCompatibleWithPlacement(
        MixingEntry mixing,
        WorldPlacement placement,
        HashSet<string> exemptRequiredTags)
    {
        if (!EnforceTagCompatibility) {
            return true;
        }
        var world = mixing?.cachedWorld;
        if (world?.worldTags == null || placement?.worldMixing == null) {
            return false;
        }

        foreach (string requiredTag in placement.worldMixing.requiredTags) {
            if (exemptRequiredTags != null && exemptRequiredTags.Contains(requiredTag)) {
                continue;
            }
            if (!world.worldTags.Contains(requiredTag)) {
                Util.LogDbg(
                    "WorldMixing: incompatible tags: target={0} missing requiredTag={1} for placement(current={2})",
                    mixing.worldPath,
                    requiredTag,
                    placement.world);
                return false;
            }
        }

        foreach (string forbiddenTag in placement.worldMixing.forbiddenTags) {
            if (world.worldTags.Contains(forbiddenTag)) {
                Util.LogDbg(
                    "WorldMixing: incompatible tags: target={0} has forbiddenTag={1} for placement(current={2})",
                    mixing.worldPath,
                    forbiddenTag,
                    placement.world);
                return false;
            }
        }

        return true;
    }
}
