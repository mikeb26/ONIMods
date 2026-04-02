// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System;
using UnityEngine;

namespace BotTweaks.Hooks;

internal static class HooksExpireBehavior {
    // When a Rover or MorbRover (Biobot) expires, optionally mark it for deconstruction.
    [HarmonyPatch(typeof(RobotDeathStates), nameof(RobotDeathStates.InitializeStates))]
    public static class RobotDeathStates_InitializeStates_Patch {
        // Only touch the "pst" state (after death anim completes) so we act exactly when the game
        // applies the Dead tag and other systems might fire.
        public static void Postfix(RobotDeathStates __instance) {
            if (__instance == null) {
                return;
            }

            // private RobotDeathStates.State pst;
            var pstField = AccessTools.Field(typeof(RobotDeathStates), "pst");
            var pst = pstField?.GetValue(__instance) as RobotDeathStates.State;
            if (pst == null) {
                Util.LogDbg("RobotDeathStates.InitializeStates: failed to locate pst state");
                return;
            }

            pst.Enter("BotTweaks_ExpireBehavior", smi => {
                try {
                    TrackedRobot.DispatchDeathAnimComplete(smi);
                } catch (Exception e) {
                    Util.Log("Expire behavior error: {0}", e);
                }
            });
        }
    }

    // Biobot: override base-game behavior which always marks for deconstruct on death.
    [HarmonyPatch(typeof(MorbRoverConfig), nameof(MorbRoverConfig.TriggerDeconstructChoreOnDeath))]
    public static class MorbRoverConfig_TriggerDeconstructChoreOnDeath_Patch {
        public static bool Prefix(object obj) {
            // Base game passes the GameObject as the event payload.
            if (obj is GameObject go) {
                return Biobot.HandleDeconstructOnDeathEvent(go);
            }
            // If we can't interpret the payload, let base game run.
            return true;
        }
    }

    // Existing saves: apply dispositions to already-dead rovers/biobots on load.
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_OnSpawn_Patch {
        public static void Postfix() {
            try {
                // OnSpawnComplete is invoked in Game.LateUpdate and then nulled; always use
                // Delegate.Combine to avoid clobbering other handlers.
                Game.Instance.OnSpawnComplete = (System.Action)System.Delegate.Combine(
                    Game.Instance.OnSpawnComplete,
                    new System.Action(ExpiredRobotBehavior.ApplyToExistingDeadRobots)
                );

                Game.Instance.OnSpawnComplete = (System.Action)System.Delegate.Combine(
                    Game.Instance.OnSpawnComplete,
                    new System.Action(TrackedRobotLoader.ScanAndAttachForExistingSaves)
                );
            } catch (Exception e) {
                Util.Log("Failed to register OnSpawnComplete handler: {0}", e);
            }
        }
    }
}
