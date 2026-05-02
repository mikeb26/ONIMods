// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;

namespace BotTweaks.Hooks;

internal static class HooksSaveLoad {
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Load))]
    public static class SaveManager_Load_Patch {
        public static void Prefix(SaveManager __instance) {
            try {
                PowerBankEnabler.ResetTrackedRobotPrefabStateBeforeLoad(__instance);
            } catch (System.Exception e) {
                Util.Log("Failed to reset tracked robot prefab state before SaveManager.Load: {0}", e);
            }
        }
    }
}
