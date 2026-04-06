// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System;
using UnityEngine;

namespace BotTweaks.Hooks;

internal static class HooksAtomicPowerBanks {
    // If a robot already has an atomic power bank installed, prevent new fetch chores from
    // targeting atomic banks for this robot.
    //
    // This avoids an infinite loop where dupes continually deliver extra atomics and we drop them.
    [HarmonyPatch(typeof(RobotElectroBankMonitor.Instance), "OnFilterChanged")]
    public static class RobotElectroBankMonitor_Instance_OnFilterChanged_AtomicLimit_Patch {
        public static void Postfix(RobotElectroBankMonitor.Instance __instance, System.Collections.Generic.HashSet<Tag> allowed_tags) {
            AtomicPowerBanks.OnFilterChanged(__instance, allowed_tags);
        }
    }

    // Enforce atomic bank storage limit + ordering whenever electrobank storage changes.
    [HarmonyPatch(typeof(RobotElectroBankMonitor.Instance), nameof(RobotElectroBankMonitor.Instance.ElectroBankStorageChange))]
    public static class RobotElectroBankMonitor_Instance_ElectroBankStorageChange_Patch {
        public static void Postfix(RobotElectroBankMonitor.Instance __instance) {
            AtomicPowerBanks.OnElectroBankStorageChange(__instance);
        }
    }

    // Ensure power consumption prioritizes atomic banks even if other banks exist.
    // (Base game consumes from 'smi.electrobank', which is set from storage.items[0].)
    //
    // We only intervene if an atomic bank exists; otherwise preserve base behavior.
    [HarmonyPatch(typeof(RobotElectroBankMonitor), nameof(RobotElectroBankMonitor.ConsumePower))]
    public static class RobotElectroBankMonitor_ConsumePower_Patch {
        public static bool Prefix(RobotElectroBankMonitor.Instance smi, float dt) {
            return HooksShared.SafePrefix(() => AtomicPowerBanks.ConsumePowerPrefix(smi, dt), fallback: true);
        }
    }
}
