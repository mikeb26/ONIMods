// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using Klei.CustomSettings;
using UnityEngine;              
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using PeterHan.PLib.Options;
using ProcGen;
using ProcGenGame;
using Klei;

namespace CGSM;

public class WorldMixing
{
    private HashSet<string> storyTraitDisabledWorlds = null;

    // Prefer PlanetoidType identity when we can resolve it from the world path.
    // This avoids treating Start vs Other variants of the same planetoid as distinct.
    private static string GetWorldIdentityKey(string worldPath)
    {
        if (string.IsNullOrWhiteSpace(worldPath))
            return "path:" + (worldPath ?? string.Empty);

        if (PlanetoidInfos.TryLookupTypeByWorldPath(worldPath, out var planetoidType))
        {
            var normalized = Planetoids.NormalizeForMixing(planetoidType);
            return $"type:{(int)normalized}";
        }

        return $"path:{worldPath}";
    }

    public WorldMixing() {
        this.reset();
    }

    public void reset() {
        this.storyTraitDisabledWorlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public static bool isCGSMWorldMixingSetting(WorldMixingSettingConfig wm) {
        return (wm.worldgenPath.IndexOf("CGSM.", StringComparison.OrdinalIgnoreCase) >= 0) ||
               (wm.id.IndexOf("CGSM.", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    public bool shouldDisableStoryTraitsForWorld(string worldPath) {
        return this.storyTraitDisabledWorlds.Contains(GetWorldIdentityKey(worldPath));
    }

    public static void deleteDisabledWorlds(CustomGameSettings cgs, ClusterLayout clusterLayout) {
        var disabledWorldPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in cgs.MixingSettings) {
            if (kvp.Value is not WorldMixingSettingConfig wm) {
                continue;
            }

            if (!isCGSMWorldMixingSetting(wm)) {
                continue;
            }

            var level = cgs.GetCurrentMixingSettingLevel(wm);
            if (level == null) {
                continue;
            }

            if (!string.Equals(level.id, WorldMixingSettingConfig.DisabledLevelId, StringComparison.OrdinalIgnoreCase)) {
                continue;
            }

            if (!tryGetWorldPath(wm, out var worldPath)) {
                continue;
            }

            disabledWorldPaths.Add(worldPath);
        }

	string startWorldPath = clusterLayout.GetStartWorld();
	if (disabledWorldPaths.Count > 0) {
            // Normalize disabled targets to mixing identity so MiniRegolith disables Regolith (and vice versa).
            var disabledWorldIdentities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var wp in disabledWorldPaths)
                disabledWorldIdentities.Add(GetWorldIdentityKey(wp));

            // Remove from the end to avoid index shifts.
            for (int i = clusterLayout.worldPlacements.Count - 1; i >= 0; --i) {
                var wp = clusterLayout.worldPlacements[i];
		bool isStartWorld = Planetoids.IsStartWorld(wp, startWorldPath);
                bool isWarpWorld = Planetoids.IsWarpWorld(wp.world);
                if (!isStartWorld && !isWarpWorld && disabledWorldIdentities.Contains(GetWorldIdentityKey(wp.world))) {
                    Util.LogDbg("WorldMixing: removing disabled world from cluster: {0}", wp.world);
                    clusterLayout.worldPlacements.RemoveAt(i);
                }
            }
        }
    }

    // Some cluster configurations can result in the same world appearing more than once
    // in clusterLayout.worldPlacements (e.g. if a world is present as both a fixed placement
    // and also inserted via a mixing slot). This can crash or produce invalid clusters.
    //
    // Remove duplicates by world path, keeping the *first* instance and removing later ones.
    public static void deleteDuplicateWorlds(ClusterLayout clusterLayout)
    {
        // Two-pass: seed duplicates map with start/warp worlds so those are always preferred
        // as the placement to keep.
        var keepIndexByWorldIdentity = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        string startWorldPath = clusterLayout.GetStartWorld();

        for (int i = 0; i < clusterLayout.worldPlacements.Count; ++i) {
            var wp = clusterLayout.worldPlacements[i];
            if (Planetoids.IsStartWorld(wp, startWorldPath) || Planetoids.IsWarpWorld(wp.world)) {
                var identity = GetWorldIdentityKey(wp.world);
                if (!keepIndexByWorldIdentity.ContainsKey(identity))
                    keepIndexByWorldIdentity[identity] = i;
            }
        }

        // Fill in remaining worlds with their first seen index.
        for (int i = 0; i < clusterLayout.worldPlacements.Count; ++i) {
            var wp = clusterLayout.worldPlacements[i];
            var identity = GetWorldIdentityKey(wp.world);
            if (!keepIndexByWorldIdentity.ContainsKey(identity))
                keepIndexByWorldIdentity[identity] = i;
        }

        // Remove from the end to avoid index shifts.
        for (int i = clusterLayout.worldPlacements.Count - 1; i >= 0; --i)
        {
            var wp = clusterLayout.worldPlacements[i];
            var identity = GetWorldIdentityKey(wp.world);
            if (keepIndexByWorldIdentity.TryGetValue(identity, out int keepIndex) && keepIndex != i) {
	        // paranoia in case someone created a custom cluster with duplicate start/warp
                if (Planetoids.IsStartWorld(wp, startWorldPath) || Planetoids.IsWarpWorld(wp.world)) {
		    continue;
		}

                Util.Log("WorldMixing dedupe: removing duplicate idx={0} world={1} identity={2} (keepIndex={3})", i, wp.world, identity, keepIndex);
                clusterLayout.worldPlacements.RemoveAt(i);
            }
        }
    }

    // The base game injects random world traits (e.g. Metal Rich, Slime Molds, Geoactive)
    // into worlds unless a world explicitly disables them.
    //
    // Only disable injection for worlds we actually substituted into the cluster during
    // world mixing (see WorldgenMixingReplacement logging "WorldMixing: substitution").
    // If a world was already present in the cluster (even if it had a mixing setting),
    // leave it alone.
    public static void disableInjectedWorldTraits(CustomGameSettings cgs, ClusterLayout clusterLayout) {
        if (clusterLayout?.worldPlacements == null)
            return;

        // Track substitutions by identity (PlanetoidType-normalized where possible).
        var substitutedWorldIdentities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var wp in clusterLayout.worldPlacements)
        {
            if (wp?.worldMixing != null && wp.worldMixing.mixingWasApplied)
                substitutedWorldIdentities.Add(GetWorldIdentityKey(wp.world));
        }

        foreach (var wp in clusterLayout.worldPlacements) {
            if (wp?.worldMixing == null || !wp.worldMixing.mixingWasApplied)
                continue;

            if (!substitutedWorldIdentities.Contains(GetWorldIdentityKey(wp.world)))
                continue;

            ProcGen.World worldData = null;
            try {
                worldData = SettingsCache.worlds.GetWorldData(wp, 0);
            } catch {
                // ignore
            }
            if (worldData == null) {
                continue;
  	    }

            // The trait injection comes from SettingsCache.GetRandomTraits(seed, world), which
            // iterates world.worldTraitRules. We can't set World.disableWorldTraits (private
            // setter), so instead clear the rules list to prevent random trait injection
            if (worldData.worldTraitRules != null && worldData.worldTraitRules.Count > 0) {
                worldData.worldTraitRules.Clear();
		Util.Log("WorldMixing: disabled world traits for world {0}", wp.world);
            }
        }
    }

    public static bool tryGetWorldPath(WorldMixingSettingConfig wm, out string worldPath) {
        worldPath = null;

        WorldMixingSettings settings = null;
        try {
            settings = SettingsCache.TryGetCachedWorldMixingSetting(wm.worldgenPath);
        } catch {
            // ignore
        }

        if (string.IsNullOrWhiteSpace(settings?.world)) {
            return false;
        }

        worldPath = settings.world;
        return true;
    }

    // The base game injects "story traits" per-world. For CGSM, only disable story traits on
    // worlds we actually substituted into the cluster during world mixing (see
    // WorldgenMixingReplacement logging "WorldMixing: substitution").
    //
    // Additionally, only disable for substitutions whose *target world* is one of:
    // {moo, marshy, niobium, regolith, tundra}.
    public void disableStoryTraits(ClusterLayout clusterLayout) {
        this.storyTraitDisabledWorlds.Clear();

        if (clusterLayout?.worldPlacements == null)
            return;

        bool ShouldDisableForWorldPath(string worldPath)
        {
            if (!PlanetoidInfos.TryLookupTypeByWorldPath(worldPath, out var planetoidType))
                return false;

            // Normalize (MiniRegolith => Regolith) so the allowlist works across variants.
            var normalized = Planetoids.NormalizeForMixing(planetoidType);
            return normalized == PlanetoidType.Moo
                || normalized == PlanetoidType.Marshy
                || normalized == PlanetoidType.Superconductive // "Niobium"
                || normalized == PlanetoidType.Regolith
                || normalized == PlanetoidType.Tundra;
        }

        foreach (var wp in clusterLayout.worldPlacements)
        {
            if (wp?.worldMixing == null || !wp.worldMixing.mixingWasApplied)
                continue;

            if (!ShouldDisableForWorldPath(wp.world))
                continue;

            Util.LogDbg("WorldMixing: marking story traits disabled for substituted world {0}", wp.world);
            this.storyTraitDisabledWorlds.Add(GetWorldIdentityKey(wp.world));
        }
    }

    // When ONI loads *modded* worldMixingSettings/subworldMixingSettings, it creates
    // WorldMixingSettingConfig/SubworldMixingSettingConfig with required_content = null
    // (see ProcGenGame.WorldGen.LoadSettings_Internal).
    //
    // Later, CustomGameSettings.RemoveInvalidMixingSettings() iterates required_content
    // without a null-check, causing a NullReferenceException when launching a new game.
    //
    private static readonly FieldInfo RequiredContentBackingField =
        AccessTools.Field(typeof(SettingConfig), "<required_content>k__BackingField");
    public static void PatchSettingConfigContent(CustomGameSettings cgs)
    {
        if (cgs?.MixingSettings == null)
            return;

        foreach (var kvp in cgs.MixingSettings)
            EnsureRequiredContentNotNull(kvp.Value);
    }
    private static void EnsureRequiredContentNotNull(SettingConfig config)
    {
        if (config != null && config.required_content == null)
            RequiredContentBackingField?.SetValue(config, Array.Empty<string>());
    }

    // --- Destination UI cluster preview handling ---
    //
    // ColonyDestinationAsteroidBeltData.RemixClusterLayout() calls
    // WorldgenMixing.RefreshWorldMixing(mutatedClusterLayout, ...) which ultimately calls
    // DoWorldMixingInternal() on the *existing* mutatedClusterLayout instance.
    //
    // If we destructively delete worldPlacements on that layout, future refreshes will see
    // fewer mixing slots. To avoid losing mixing slots while still allowing the destination
    // UI to display a pruned cluster, we keep a "base" mutated layout (unpruned) to feed
    // back into mixing, and swap in a pruned "display" copy after each remix.
    private sealed class DestinationUiRemixState
    {
        public int seed;
        public MutatedClusterLayout baseLayout;
    }

    private static readonly ConditionalWeakTable<ColonyDestinationAsteroidBeltData, DestinationUiRemixState> DestinationUiRemixStates = new();

    private static readonly FieldInfo DestinationUiMutatedClusterLayoutField =
        AccessTools.Field(typeof(ColonyDestinationAsteroidBeltData), "mutatedClusterLayout");

    private static void RebuildDestinationUiWorlds(ColonyDestinationAsteroidBeltData instance, ClusterLayout layout)
    {
        if (instance?.worlds == null || layout?.worldPlacements == null)
            return;

        instance.worlds.Clear();
        for (int i = 0; i < layout.worldPlacements.Count; i++)
        {
            if (i == layout.startWorldIndex)
                continue;
            instance.worlds.Add(SettingsCache.worlds.GetWorldData(layout.worldPlacements[i].world));
        }
    }

    public static void DestinationUiRemixRemoveClonedClusterLayout(ColonyDestinationAsteroidBeltData instance)
    {
        try
        {
            // Restore the unpruned layout before the game re-runs world mixing.
            if (instance == null || DestinationUiMutatedClusterLayoutField == null)
                return;

            if (!DestinationUiRemixStates.TryGetValue(instance, out var state) || state == null)
                return;

            // If the seed changed, the instance was ReInitialize()'d and we must not
            // restore a layout from a previous seed.
            if (state.baseLayout == null || state.seed != instance.seed)
                return;

	    Util.LogDbg("WorldMixing: removing swapped display cluster layout");
            DestinationUiMutatedClusterLayoutField.SetValue(instance, state.baseLayout);

            // Keep instance.worlds coherent with the restored base layout for any other
            // Harmony prefixes which may run before the original method body.
            RebuildDestinationUiWorlds(instance, state.baseLayout.layout);
        }
        catch
        {
            // never fail UI due to our patch
        }
    }

    public static void DestinationUiRemixAddClonedClusterLayout(ColonyDestinationAsteroidBeltData instance)
    {
        try
        {
            if (instance == null || DestinationUiMutatedClusterLayoutField == null)
                return;

            var currentMutated = DestinationUiMutatedClusterLayoutField.GetValue(instance) as MutatedClusterLayout;
            if (currentMutated?.layout?.worldPlacements == null)
                return;

            // Save the freshly-mixed layout as the base for the next remix.
            var state = DestinationUiRemixStates.GetOrCreateValue(instance);
            state.seed = instance.seed;
            state.baseLayout = currentMutated;

            // Create a UI-only copy and apply destructive deletions to the copy.
            var displayMutated = new MutatedClusterLayout(currentMutated.layout);

            // Preserve start world so we can recompute startWorldIndex after deletions.
            string startWorldPath = null;
            try { startWorldPath = currentMutated.layout.GetStartWorld(); } catch { /* ignore */ }

            // These deletions are what are driving this hook existence
            deleteDisabledWorlds(Mod.Instance.gameState.cgs, displayMutated.layout);
            deleteDuplicateWorlds(displayMutated.layout);

            // Recompute startWorldIndex (deletions can shift indices).
            if (!string.IsNullOrWhiteSpace(startWorldPath))
            {
                int newStartIndex = 0;
                for (int i = 0; i < displayMutated.layout.worldPlacements.Count; i++)
                {
                    if (Planetoids.IsStartWorld(displayMutated.layout.worldPlacements[i], startWorldPath))
                    {
                        newStartIndex = i;
                        break;
                    }
                }
                displayMutated.layout.startWorldIndex = newStartIndex;
            }

            // Swap the UI instance to the display copy.
	    Util.LogDbg("WorldMixing: swapping display cluster layout");
            DestinationUiMutatedClusterLayoutField.SetValue(instance, displayMutated);

            // Rebuild the cached worlds list to match the display layout.
            RebuildDestinationUiWorlds(instance, displayMutated.layout);
        }
        catch
        {
            // never fail UI due to our patch
        }
    }
}