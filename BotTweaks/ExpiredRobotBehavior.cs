// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;
using UnityEngine;

namespace BotTweaks;

internal static class ExpiredRobotBehavior {
    internal static void ApplyToExistingDeadRobots() {
        // We need to find dead robots (which are NOT present in Components.LiveRobotsIdentities).
        // Pickupables is not reliable here (some robots are not pickupable in all contexts), so
        // scan all world objects and filter by prefab ID.
        foreach (var kpid in UnityEngine.Object.FindObjectsByType<KPrefabID>(FindObjectsSortMode.None)) {
            if (kpid == null) {
                continue;
            }
            var go = kpid.gameObject;
            if (go == null) {
                continue;
            }

            // Only handle in-world
            // NOTE: do NOT use transform.parent here: in-world objects are also parented under
            // scene roots. The Stored tags are the correct indicator.
            if (go.HasTag(GameTags.Stored) || go.HasTag(GameTags.StoredPrivate)) {
                continue;
            }

            if (!go.HasTag(GameTags.Dead)) {
                continue;
            }

            // Only act if the tracked-robot behavior is available.
            var tracked = go.GetComponent<TrackedRobot>();
            if (tracked == null) {
                continue;
            }

            // If the player has opted into power banks, prefer converting the robot rather than
            // applying dead-robot dispositions.
            if (tracked.CanEnablePowerbank()) {
                tracked.EnablePowerbank();
                // If conversion happened, the robot will no longer be dead.
                if (!go.HasTag(GameTags.Dead)) {
                    continue;
                }
            }

            tracked.ApplyExistingDeadRobotDisposition();
        }
    }

    internal static bool IsDeadBatteryDeath(GameObject go) {
        var deathMonitor = go.GetSMI<DeathMonitor.Instance>();
        if (deathMonitor == null) {
            return false;
        }

        var cause = deathMonitor.sm.death.Get(deathMonitor);
        return cause == Db.Get().Deaths.DeadBattery;
    }

    internal static void MarkForDeconstruct(GameObject go) {
        var deconstructable = go.GetComponent<Deconstructable>();
        if (deconstructable == null) {
            return;
        }

        if (!deconstructable.IsMarkedForDeconstruction()) {
            Util.LogDbg("Robot expired: marking '{0}' for deconstruct", go.name);
            deconstructable.QueueDeconstruction(userTriggered: false);
            // Many loose-entity deconstructables (including dead rovers) have the component
            // disabled until death, so explicitly enable it to ensure the chore is generated.
            deconstructable.enabled = true;
        }
    }

    internal static void CancelDeconstruction(GameObject go) {
        var deconstructable = go.GetComponent<Deconstructable>();
        if (deconstructable == null) {
            return;
        }
        if (deconstructable.IsMarkedForDeconstruction()) {
            Util.LogDbg("Robot expired: cancelling deconstruct on '{0}'", go.name);
            deconstructable.CancelDeconstruction();
        }
    }
}
