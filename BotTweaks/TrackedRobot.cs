// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;

namespace BotTweaks;

internal abstract class TrackedRobot : KMonoBehaviour {
    internal abstract RobotType RobotType { get; }

    /// <summary>
    /// Whether this robot can (and should) be converted to use Power Banks right now.
    ///
    /// Default: false.
    /// </summary>
    internal virtual bool CanEnablePowerbank() => false;

    /// <summary>
    /// Whether this specific robot instance has been converted to use Power Banks.
    ///
    /// Default: false (most robots either don't support power banks, or already have their own
    /// implementation like Flydo).
    /// </summary>
    internal virtual bool IsPowerbankEnabled() => false;

    /// <summary>
    /// Enable Power Bank behavior on this robot instance.
    ///
    /// Default: logs an error and does nothing.
    /// </summary>
    internal virtual void EnablePowerbank() {
        Util.Log("EnablePowerbank called for unsupported robot type '{0}' on '{1}'", RobotType,
            gameObject != null ? gameObject.name : "(null)");
    }

    internal static bool HasEnabledPowerbank(GameObject go) {
        var tracked = go != null ? go.GetComponent<TrackedRobot>() : null;
        if (tracked != null) {
            return tracked.IsPowerbankEnabled();
        }

        // If we don't have our tracker, do nothing.
        return false;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
    }

    /// <summary>
    /// Called when the robot's death animation completes.
    /// Override for robot-specific "expired" behaviors.
    /// </summary>
    internal virtual void OnDeathAnimComplete(StateMachine.Instance smi) {
        // no-op by default
    }

    /// <summary>
    /// For existing saves (and options changes), apply any type-specific behavior to already-dead
    /// robots.
    /// </summary>
    internal virtual void ApplyExistingDeadRobotDisposition() {
        // no-op by default
    }

    internal static void DispatchDeathAnimComplete(StateMachine.Instance smi) {
        var go = smi != null ? smi.gameObject : null;
        if (go == null) {
            return;
        }

        var tracked = go.GetComponent<TrackedRobot>();
        tracked?.OnDeathAnimComplete(smi);
    }
}
