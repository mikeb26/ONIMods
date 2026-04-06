// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;

namespace BotTweaks;

internal abstract class TrackedRobot : KMonoBehaviour, ISim1000ms {
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

    private int currentWorldId = -1;
    private RobotInventory currentInventory;
    // IMPORTANT: Do not cache KPrefabID in OnPrefabInit.
    // This component is attached to prefabs (CreatePrefab patches). In ONI/Unity,
    // managed field values from the prefab component can be copied to instances,
    // which can cause all instances to reference the prefab's KPrefabID.
    // That would collapse our HashSet and counts to 0/1.
    // Always resolve the instance KPrefabID at spawn-time.
    private KPrefabID kpid;

    internal KPrefabID GetKPrefabIDSafe() {
        if (kpid == null) {
            kpid = GetComponent<KPrefabID>();
        }
        return kpid;
    }

    private static readonly EventSystem.IntraObjectHandler<TrackedRobot> OnTagsChangedHandler =
        new EventSystem.IntraObjectHandler<TrackedRobot>((cmp, data) => cmp.OnTagsChanged());

    private bool subscribedToTagsChanged;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
    }

    protected override void OnSpawn() {
        base.OnSpawn();

        // Resolve instance-local components at spawn.
        kpid = GetComponent<KPrefabID>();

        // Robots can be carried between worlds as stored items. When that happens they frequently stop
        // having a meaningful grid cell, so CellChanged/migration events may not fire.
        Subscribe((int)GameHashes.TagsChanged, OnTagsChangedHandler);
        subscribedToTagsChanged = true;

        TryRegisterToCurrentWorld();
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();

        if (subscribedToTagsChanged) {
            Unsubscribe((int)GameHashes.TagsChanged, OnTagsChangedHandler);
            subscribedToTagsChanged = false;
        }

        TryUnregister();
    }

    private void OnTagsChanged() {
        // If the robot is stored (e.g. carried through a rocket door), it should not count towards
        // any world's in-world robot totals.
        if (IsStored()) {
            TryUnregister();
            return;
        }
        // Otherwise, let the 1s reconciliation ensure we end up in the correct world.
    }

    private void TryRegisterToCurrentWorld() {
        // Do not count stored robots toward any world.
        if (IsStored()) {
            return;
        }

        if (kpid == null) {
            kpid = GetComponent<KPrefabID>();
            if (kpid == null) {
                return;
            }
        }

        // ClusterManager may not be ready early in load.
        var cm = ClusterManager.Instance;
        if (cm == null) {
            return;
        }

        int newWorldId = gameObject.GetMyWorldId();
        if (newWorldId < 0) {
            return;
        }

        if (newWorldId == currentWorldId && currentInventory != null) {
            // Still registered.
            return;
        }

        // Move between worlds: unregister from old world inventory.
        TryUnregister();

        var world = cm.GetWorld(newWorldId);
        if (world == null) {
            // Not a known world.
            currentWorldId = -1;
            return;
        }

        var inv = world.GetComponent<RobotInventory>();
        if (inv == null) {
            // Our WorldInventory patch should ensure this exists; but be defensive.
            currentWorldId = -1;
            return;
        }

        currentWorldId = newWorldId;
        currentInventory = inv;

        // Track this instance.
        inv.RegisterInstance(RobotType, this);
    }

    private void TryUnregister() {
        if (currentInventory != null) {
            currentInventory.UnregisterInstance(RobotType, this);
        }
        currentInventory = null;
        currentWorldId = -1;
    }

    // NOTE: some world transfers (notably rocket interior/exterior) do not reliably emit a
    // CellChanged event on the robot, and depending on timing, migration events can fire before
    // Grid.WorldIdx is updated. To keep counts correct, re-check world membership at a low rate.
    //
    // This is intentionally cheap (O(#robots) per second vs. O(#pickupables) per tick).
    public void Sim1000ms(float dt) {
        // Stored robots should not be counted.
        if (IsStored()) {
            if (currentInventory != null) {
                TryUnregister();
            }
            return;
        }

        // If our cached world id no longer matches the grid-derived world id, re-register.
        int myWorldId = gameObject.GetMyWorldId();
        if (myWorldId != currentWorldId && myWorldId >= 0) {
            TryRegisterToCurrentWorld();
        }
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

    private bool IsStored() {
        return gameObject != null && (gameObject.HasTag(GameTags.Stored) || gameObject.HasTag(GameTags.StoredPrivate));
    }
}
