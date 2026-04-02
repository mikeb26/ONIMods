// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;
using UnityEngine;

namespace BotTweaks;

internal static class ExpirationDispositionApplier {
    /// <summary>
    /// Applies expiration disposition logic consistently for Rover/Biobot.
    /// </summary>
    /// <param name="go">Robot game object.</param>
    /// <param name="disp">Disposition to apply.</param>
    /// <param name="isExpired">Predicate which returns true iff this is an "expiration" (dead battery) case.</param>
    /// <param name="cancelDeconstructWhenDoNothing">If true, cancels deconstruction markings when disposition is DoNothing.
    /// Used for Biobot because the base game auto-marks it for deconstruct on dead-battery expiration.</param>
    internal static void Apply(GameObject go, ExpiredDisposition disp, Func<GameObject, bool> isExpired, bool cancelDeconstructWhenDoNothing) {
        if (go == null || isExpired == null) {
            return;
        }

        // Only apply expire behavior when their internal battery runs out.
        if (!isExpired(go)) {
            return;
        }

        if (!go.TryGetComponent(out TrackedRobot tracked) || tracked == null) {
            return;
        }

        if (disp == ExpiredDisposition.MarkForDeconstruct) {
            ExpiredRobotBehavior.MarkForDeconstruct(go);
        } else if (disp == ExpiredDisposition.EnablePowerBanks) {
            tracked.EnablePowerbank();
        } else if (cancelDeconstructWhenDoNothing) {
            ExpiredRobotBehavior.CancelDeconstruction(go);
        }
    }
}
