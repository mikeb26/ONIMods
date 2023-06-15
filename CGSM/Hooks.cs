// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using Klei.CustomSettings;

namespace CGSM;

public static class Hooks
{
    [HarmonyPatch(typeof(ClusterCategorySelectionScreen))]
    [HarmonyPatch("OnClickOption")]
    public class ClusterCat_OnClickOption_Patch {
        public static void Postfix(ProcGen.ClusterLayout.ClusterCategory clusterCategory) {
            if (clusterCategory == ProcGen.ClusterLayout.ClusterCategory.spacedOutVanillaStyle) {
                Mod.Instance.gameState.unsupportedClusterCat = false;
                Mod.Instance.gameState.cgsmCluster = ClusterUtils.loadClusterFromOptionsAndEmit(false);
                Mod.Instance.gameState.maskedCluster = "clusters/CGSM";
            } else if (clusterCategory == ProcGen.ClusterLayout.ClusterCategory.spacedOutStyle) {
                Mod.Instance.gameState.unsupportedClusterCat = false;
                Mod.Instance.gameState.cgsmCluster = ClusterUtils.loadClusterFromOptionsAndEmit(false);
                Mod.Instance.gameState.maskedCluster = "clusters/CGSMVanilla";
            } else {
                Mod.Instance.gameState.unsupportedClusterCat = true;
            }
       }
    }

    // @todo tweaking geysers is a future feature
    // [HarmonyPatch(typeof(ProcGenGame.Cluster), "BeginGeneration")]
    // public static class Game_OnPrefabInit_Patch {
    //      public static void Postfix(ref ProcGenGame.Cluster __instance) {
    //         WorldGen.ApplyGeyserPreferences(ref __instance);
    //     }
    // }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("OnPrefabInit")]
    public static class CGS_Prefab_Patch {
         public static void Postfix(ref CustomGameSettings __instance) {
             Util.LogDbg("CGS Init");
             Mod.Instance.gameState.addToggleSettings(ref __instance);
         }
    }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("SetQualitySetting", new System.Type[] { typeof(SettingConfig), typeof(string) } )]
    public static class CGS_SetQualitySetting_Patch {
         public static void Postfix(ref CustomGameSettings __instance, SettingConfig config,
                                    string value) {
             // not a fan of these string compares to find when settings are changed but i dont
             // see a cleaner way; and there's no STRINGS constant for "ClusterLayout" in
             // CustomGameSettingConfigs()
             if (config.id == "ClusterLayout") {
                 Mod.Instance.gameState.selectNewCluster(value);
             } else if (config.id.Contains("CGSM.")) {
                 Mod.Instance.gameState.toggleSetting(ref __instance, config, value);
             } else {
                 Util.LogDbg("setsetting: ignoring id:{0}, label:{1}, tooltip:{2}, value:{3}",
                             config.id, config.label, config.tooltip, value);
             }
         }
    }

    [HarmonyPatch(typeof(NewGameSettingsPanel))]
    [HarmonyPatch("Init")]
    public static class NewGameSettings_Init_Patch {
        public static void Postfix(ref NewGameSettingsPanel __instance) {
            Util.LogDbg("NGSP Init");
            Mod.Instance.gameState.ngsp = __instance;
            // appears to be the last one that instantiates
            Mod.Instance.gameState.resetTogglesAndSelectedCluster();
        }
    }

    [HarmonyPatch(typeof(DestinationSelectPanel))]
    [HarmonyPatch("OnPrefabInit")]
    public static class DestinationSelect_Prefab_Patch {
        public static void Postfix(ref DestinationSelectPanel __instance) {
            Util.LogDbg("DSP Init");
            Mod.Instance.gameState.dsp = __instance;
        }
    }

    [HarmonyPatch(typeof(ColonyDestinationSelectScreen))]
    [HarmonyPatch("OnPrefabInit")]
    public static class ColonySelect_Prefab_Patch {
        public static void Postfix(ref ColonyDestinationSelectScreen __instance) {
            Util.LogDbg("CDSS Init");
            Mod.Instance.gameState.cdss = __instance;
        }
    }

    [HarmonyPatch(typeof(ColonyDestinationSelectScreen))]
    [HarmonyPatch("LaunchClicked")]
    public static class CDS_Launch_Patch {
        public static void Prefix(ref ColonyDestinationSelectScreen __instance) {
            Util.LogDbg("CDSS launch");
            Mod.Instance.gameState.Launch();
        }
    }

}
