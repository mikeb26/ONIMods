// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System;

namespace BotTweaks.Hooks;

internal static class HooksFlydo {
    // Flydo: drop any installed power banks right before deconstruction spawns construction materials,
    // so the power banks end up in the world as well.
    [HarmonyPatch(typeof(Deconstructable), "TriggerDestroy")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(byte), typeof(int), typeof(WorkerBase) })]
    public static class Deconstructable_TriggerDestroy_Patch {
        public static void Prefix(Deconstructable __instance) {
            try {
                if (__instance == null || __instance.gameObject == null) {
                    return;
                }
                if (!Flydo.IsFlydo(__instance.gameObject)) {
                    return;
                }

                Flydo.DropAllPowerBanks(__instance.gameObject);
            } catch (Exception e) {
                Util.Log("Flydo deconstruct drop-banks patch error: {0}", e);
            }
        }
    }
}
