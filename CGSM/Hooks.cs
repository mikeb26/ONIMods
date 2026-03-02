// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;
using Klei;
using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;

namespace CGSM;

public static class Hooks
{
    [HarmonyPatch(typeof(ClusterCategorySelectionScreen))]
    [HarmonyPatch("OnClickOption")]
    public class ClusterCat_OnClickOption_Patch {
        public static void Postfix(ProcGen.ClusterLayout.ClusterCategory clusterCategory) {
            Mod.Instance.gameState.cgsmCluster = ClusterUtils.loadClusterFromOptionsAndEmit(false);
            Util.LogDbg("Reset settings cache");
            ProcGen.SettingsCache.Clear();
            List<YamlIO.Error> errors = new List<YamlIO.Error>();
            ProcGen.SettingsCache.LoadFiles(errors);
       }
    }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("OnPrefabInit")]
    public static class CGS_Prefab_Patch {
         public static void Postfix(ref CustomGameSettings __instance) {
             Util.LogDbg("CGS Init");
             Mod.Instance.gameState.cgs = __instance;
         }
    }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("SetQualitySetting", new System.Type[] { typeof(SettingConfig), typeof(string) } )]
    public static class CGS_SetQualitySetting_Patch {
         public static void Postfix(ref CustomGameSettings __instance, SettingConfig config,
                                    string value) {
	     Util.LogDbg("CGS.SetQuality.Patch");
             if (!string.Equals(config.id, CustomGameSettingConfigs.ClusterLayout.id)) {
	         return;
             }
             Mod.Instance.gameState.selectNewCluster(value);
         }
    }

    [HarmonyPatch(typeof(NewGameSettingsPanel))]
    [HarmonyPatch("Init")]
    public static class NewGameSettings_Init_Patch {
        public static void Postfix(ref NewGameSettingsPanel __instance) {
            Util.LogDbg("NGSP Init");
	    Mod.Instance.gameState.maybeSelectNewCluster();
        }
    }

    [HarmonyPatch(typeof(ColonyDestinationSelectScreen))]
    [HarmonyPatch("LaunchClicked")]
    public static class CDS_Launch_Patch {
        public static void Prefix(ref ColonyDestinationSelectScreen __instance) {
	
	    Util.LogDbg("CDS.Launch.Patch");
            Mod.Instance.gameState.Launch();
        }
    }

    /* in testing it looks like our CustomGameSettings.SetQualitySetting hook is no longer being
     * invoked when the player selects a new cluster from the colony destination select screen.
     * so to mitigate we hook here as well
     */
    [HarmonyPatch(typeof(ColonyDestinationSelectScreen))]
    [HarmonyPatch("OnAsteroidClicked")]
    public static class CDS_OnAsteroidClicked_Patch {
        public static void Prefix(ref ColonyDestinationSelectScreen __instance, ColonyDestinationAsteroidBeltData cluster) {
	    Util.LogDbg("CDS.OnAsteroidClicked.Patch");
            Mod.Instance.gameState.selectNewCluster(cluster.beltPath);
        }
    }

    [HarmonyPatch(typeof(CustomGameSettings), nameof(CustomGameSettings.RemoveInvalidMixingSettings))]
    public static class CustomGameSettings_RemoveInvalidMixingSettings_Patch
    {
        public static void Prefix(CustomGameSettings __instance) => WorldMixing.PatchSettingConfigContent(__instance);
    }

    // Disable story trait injection on a per-world basis for any world controlled by a CGSM
    // WorldMixingSetting. The base game assigns story trait candidates per world using this
    // method, and TemplateSpawning then consumes those candidates.
    [HarmonyPatch(typeof(WorldGenSettings))]
    [HarmonyPatch("SetStoryTraitCandidates")]
    public static class WorldGenSettings_SetStoryTraitCandidates_Patch {
        public static void Prefix(WorldGenSettings __instance, ref List<WorldTrait> storyTraits) {
            try {
                if (!Mod.Instance.gameState.worldMixing.shouldDisableStoryTraitsForWorld(__instance.world.filePath))
                    return;

                Util.Log("WorldMixing: disabling story traits for world {0}", __instance.world.filePath);
                storyTraits = new List<WorldTrait>();
            } catch {
                // never fail worldgen due to our patch
            }
        }
    }

    // Replace the stock world mixing implementation so CGSM can customize cluster world mixing.
    // This initially mirrors the game's implementation of ProcGenGame.WorldgenMixing.DoWorldMixingInternal.
    [HarmonyPatch(typeof(ProcGenGame.WorldgenMixing))]
    [HarmonyPatch("DoWorldMixingInternal", new System.Type[] { typeof(MutatedClusterLayout), typeof(int), typeof(bool), typeof(bool) })]
    public static class WorldgenMixing_DoWorldMixingInternal_Patch {
        public static bool Prefix(MutatedClusterLayout mutatedClusterLayout, int seed, bool isRunningWorldgenDebug, bool muteErrors,
                                  ref MutatedClusterLayout __result) {
            __result = WorldgenMixingReplacement.DoWorldMixingInternal(mutatedClusterLayout, seed, isRunningWorldgenDebug, muteErrors);
            return false; // skip original
        }
    }

    // Destination UI cluster preview: keep a "base" layout for mixing and swap in a
    // pruned copy for display (implemented in WorldMixing).
    [HarmonyPatch(typeof(ColonyDestinationAsteroidBeltData))]
    [HarmonyPatch(nameof(ColonyDestinationAsteroidBeltData.RemixClusterLayout))]
    public static class ColonyDestinationAsteroidBeltData_RemixClusterLayout_Patch
    {
        public static void Prefix(ColonyDestinationAsteroidBeltData __instance)
        {
            WorldMixing.DestinationUiRemixRemoveClonedClusterLayout(__instance);
        }

        public static void Postfix(ColonyDestinationAsteroidBeltData __instance)
        {
            WorldMixing.DestinationUiRemixAddClonedClusterLayout(__instance);
        }
    }
}
