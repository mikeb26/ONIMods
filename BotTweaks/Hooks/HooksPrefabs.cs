// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using UnityEngine;

namespace BotTweaks.Hooks;

internal static class HooksPrefabs {
    [HarmonyPatch(typeof(WorldInventory), "OnPrefabInit")]
    public static class WorldInventory_OnPrefabInit_Patch {
        public static void Postfix(WorldInventory __instance) {
            Util.LogDbg("WorldInventory.OnPrefabInit: adding RobotInventory to world '{0}' (id:{1})", __instance.gameObject.name,
                __instance.GetComponent<WorldContainer>() != null ? __instance.GetComponent<WorldContainer>().id : -1);
            __instance.gameObject.AddOrGet<RobotInventory>();
        }
    }

    [HarmonyPatch(typeof(ScoutRoverConfig), nameof(ScoutRoverConfig.CreatePrefab))]
    public static class ScoutRoverConfig_CreatePrefab_Patch {
        public static void Postfix(GameObject __result) {
            if (__result == null) {
                return;
            }

            __result.AddOrGet<Rover>();

            // Stock rover prefab has a Deconstructable but it is disabled. Enable it so the
            // standard "Deconstruct" user menu button appears even while the rover is alive.
            var deconstructable = __result.GetComponent<Deconstructable>();
            if (deconstructable != null) {
                deconstructable.enabled = true;
                deconstructable.allowDeconstruction = true;
            }
        }
    }

    [HarmonyPatch(typeof(MorbRoverConfig), nameof(MorbRoverConfig.CreatePrefab))]
    public static class MorbRoverConfig_CreatePrefab_Patch {
        public static void Postfix(GameObject __result) {
            if (__result == null) {
                return;
            }

            __result.AddOrGet<Biobot>();

            // Stock morb rover prefab has a Deconstructable but it is disabled. Enable it so the
            // standard "Deconstruct" user menu button appears even while the biobot is alive.
            var deconstructable = __result.GetComponent<Deconstructable>();
            if (deconstructable != null) {
                deconstructable.enabled = true;
                deconstructable.allowDeconstruction = true;
            }
        }
    }

    [HarmonyPatch(typeof(SweepBotConfig), nameof(SweepBotConfig.CreatePrefab))]
    public static class SweepBotConfig_CreatePrefab_Patch {
        public static void Postfix(GameObject __result) {
            __result?.AddOrGet<Sweepy>();
        }
    }

    [HarmonyPatch(typeof(RemoteWorkerConfig), nameof(RemoteWorkerConfig.CreatePrefab))]
    public static class RemoteWorkerConfig_CreatePrefab_Patch {
        public static void Postfix(GameObject __result) {
            __result?.AddOrGet<RemoteWorker>();
        }
    }

    [HarmonyPatch(typeof(FetchDroneConfig), nameof(FetchDroneConfig.CreatePrefab))]
    public static class FetchDroneConfig_CreatePrefab_Patch {
        public static void Postfix(GameObject __result) {
            if (__result == null) {
                return;
            }

            __result.AddOrGet<Flydo>();

            // Stock prefab has a Deconstructable but it is disabled. Enable it so the
            // standard "Deconstruct" user menu button appears.
            var deconstructable = __result.GetComponent<Deconstructable>();
            if (deconstructable != null) {
                deconstructable.enabled = true;
                deconstructable.allowDeconstruction = true;
            }
        }
    }
}
