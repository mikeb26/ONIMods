// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using Klei.CustomSettings;

namespace BeyondExtraHard;

public static class Hooks
{
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_Spawn_Patch {
         public static void Prefix(Game __instance) {
             Util.LogDbg("Game spawn");

             Mod.Instance.gameState.applyGameSettings();
         }
    }

    [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
    public class MinionIdentity_OnSpawn_Patch {
        public static void Postfix(ref MinionIdentity __instance) {
             Util.LogDbg("Dupe spawn");

             Mod.Instance.gameState.applyDupeSettings(ref __instance);
        }
    }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("OnPrefabInit")]
    public static class CGS_Prefab_Patch {
         public static void Postfix(ref CustomGameSettings __instance) {
             Util.LogDbg("CGS Init");

             Mod.Instance.gameState.cgs = __instance;
             Mod.Instance.gameState.addToggleSettings(ref __instance);
         }
    }

    // @todo as this appears to invoked very frequently (O(1000) / second) it would be
    // better if we could find an alternative way to block a building from being selected
    // on the build menu
    [HarmonyPatch(typeof(PlanScreen))]
    [HarmonyPatch("GetBuildableState")]
    public static class PlanScreen_GetBuildableState_Patch {
        public static bool Prefix(BuildingDef def, ref PlanScreen.RequirementsState __result) {
            if (Mod.Instance.gameState.shouldHideBuilding(def)) {
                __result = PlanScreen.RequirementsState.Tech;
                return false;
            }

            return true; // Call original method.
        }
    }

    [HarmonyPatch(typeof(CustomGameSettings))]
    [HarmonyPatch("SetQualitySetting", new System.Type[] { typeof(SettingConfig), typeof(string) } )]
    public static class CGS_SetQualitySetting_Patch {
         public static void Postfix(ref CustomGameSettings __instance, SettingConfig config,
                                    string value) {
             if (!config.id.Contains(Constants.ModPrefix)) {
                 Util.LogDbg("setsetting: ignoring id:{0}, label:{1}, tooltip:{2}, value:{3}",
                             config.id, config.label, config.tooltip, value);
                 return;
             }

             Mod.Instance.gameState.toggleSetting(ref __instance, config, value);
         }
    }

    [HarmonyPatch(typeof(WorldDamage))]
    [HarmonyPatch("OnDigComplete")]
    public static class WorldDamage_OnDigComplete_Patch {
        public static void Prefix(ref float mass) {
            Mod.Instance.gameState.applyMassSetting(ref mass);
        }
    }

    [HarmonyPatch(typeof(Deconstructable))]
    [HarmonyPatch("SpawnItem")]
    public static class Deconstructable_SpawnItem_Patch {
        public static void Prefix(ref float src_mass) {
            Mod.Instance.gameState.applyMassSetting(ref src_mass);
        }
    }
}
