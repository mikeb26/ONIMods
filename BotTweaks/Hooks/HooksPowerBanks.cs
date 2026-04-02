// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System.Collections.Generic;

namespace BotTweaks.Hooks;

internal static class HooksPowerBanks {
    // Flydo: if the player disables a Power Bank in the Flydo's element filter,
    // the currently-installed bank should be dropped immediately.
    [HarmonyPatch(typeof(RobotElectroBankMonitor.Instance), "OnFilterChanged")]
    public static class RobotElectroBankMonitor_Instance_OnFilterChanged_Patch {
        public static void Postfix(RobotElectroBankMonitor.Instance __instance, HashSet<Tag> allowed_tags) {
            Flydo.MaybeRemoveBattery(__instance, allowed_tags);
        }
    }

    // When a Rover/Biobot has been converted to use power banks, prevent the base game's
    // RobotBatteryMonitor from killing it (and kicking it into the terminal deadBattery state)
    // due to its internal chemical/bio battery being 0.
    [HarmonyPatch(typeof(RobotBatteryMonitor), nameof(RobotBatteryMonitor.BatteryDead))]
    public static class RobotBatteryMonitor_BatteryDead_Patch {
        public static bool Prefix(RobotBatteryMonitor.Instance smi, ref bool __result) {
            return HooksShared.SafePrefixOverrideBool(() => {
                if (smi != null && smi.gameObject != null && TrackedRobot.HasEnabledPowerbank(smi.gameObject)) {
                    return false;
                }
                return null;
            }, ref __result);
        }
    }

    // For converted rovers/biobots, the internal battery is no longer relevant.
    [HarmonyPatch(typeof(RobotBatteryMonitor), nameof(RobotBatteryMonitor.NeedsRecharge))]
    public static class RobotBatteryMonitor_NeedsRecharge_Patch {
        public static bool Prefix(RobotBatteryMonitor.Instance smi, ref bool __result) {
            return HooksShared.SafePrefixOverrideBool(() => {
                if (smi != null && smi.gameObject != null && TrackedRobot.HasEnabledPowerbank(smi.gameObject)) {
                    return false;
                }
                return null;
            }, ref __result);
        }
    }

    [HarmonyPatch(typeof(RobotBatteryMonitor), nameof(RobotBatteryMonitor.ChargeDecent))]
    public static class RobotBatteryMonitor_ChargeDecent_Patch {
        public static bool Prefix(RobotBatteryMonitor.Instance smi, ref bool __result) {
            return HooksShared.SafePrefixOverrideBool(() => {
                if (smi != null && smi.gameObject != null && TrackedRobot.HasEnabledPowerbank(smi.gameObject)) {
                    return true;
                }
                return null;
            }, ref __result);
        }
    }

    // Rovers/biobots do not have the Flydo animation symbols used by RobotElectroBankMonitor.
    // If the requested symbol doesn't exist in the robot's anim, simply skip the override.
    [HarmonyPatch(typeof(RobotElectroBankMonitor.Instance), nameof(RobotElectroBankMonitor.Instance.UpdateBatteryState))]
    public static class RobotElectroBankMonitor_Instance_UpdateBatteryState_Patch {
        private static readonly AccessTools.FieldRef<RobotElectroBankMonitor.Instance, SymbolOverrideController> f_symbolOverrideController =
            AccessTools.FieldRefAccess<RobotElectroBankMonitor.Instance, SymbolOverrideController>("symbolOverrideController");

        private static readonly AccessTools.FieldRef<RobotElectroBankMonitor.Instance, KBatchedAnimController> f_animController =
            AccessTools.FieldRefAccess<RobotElectroBankMonitor.Instance, KBatchedAnimController>("animController");

        private static readonly AccessTools.FieldRef<RobotElectroBankMonitor.Instance, HashedString> f_currentSymbolSwap =
            AccessTools.FieldRefAccess<RobotElectroBankMonitor.Instance, HashedString>("currentSymbolSwap");

        public static bool Prefix(RobotElectroBankMonitor.Instance __instance, HashedString newState) {
            return HooksShared.SafePrefix(() => {
                // Only intercept for our converted rovers/biobots. Let the base game handle Flydos.
                if (__instance == null || __instance.gameObject == null || !TrackedRobot.HasEnabledPowerbank(__instance.gameObject)) {
                    return true;
                }

                var symbolOverrideController = f_symbolOverrideController(__instance);
                var animController = f_animController(__instance);

                if (symbolOverrideController == null || animController == null || animController.AnimFiles == null || animController.AnimFiles.Length == 0) {
                    return false;
                }

                var current = f_currentSymbolSwap(__instance);
                if (current.IsValid) {
                    symbolOverrideController.RemoveSymbolOverride(current);
                }

                // If this anim doesn't contain the battery symbol, do nothing.
                var build = animController.AnimFiles[0]?.GetData()?.build;
                var symbol = build?.GetSymbol(newState);
                if (symbol == null) {
                    f_currentSymbolSwap(__instance) = default;
                    return false;
                }

                symbolOverrideController.AddSymbolOverride(RobotElectroBankMonitor.BATTER_SYMBOL, symbol);
                f_currentSymbolSwap(__instance) = newState;
                return false;
            }, fallback: true);
        }
    }
}
