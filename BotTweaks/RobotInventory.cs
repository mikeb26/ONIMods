/*
 * Original Copyright 2026 Peter Han; see ./ResourceScreen/LICENSE from this directory
 * Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this
 *   package
 */

using KSerialization;
using System.Collections.Generic;

namespace BotTweaks;

// loosely based on https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/CritterInventory.cs

// Stores the inventory all tracked robots per world.
[SerializationConfig(MemberSerialization.OptIn)]
public sealed class RobotInventory : KMonoBehaviour {

    private const int INITIAL_RTYPE_CAPACITY = 8;

    // Track actual instances for cycling-through in the pinned panel.
    // Not serialized (save files can be loaded without this mod).
    private readonly Dictionary<RobotType, HashSet<TrackedRobot>> instances;

    [Serialize]
    private HashSet<RobotType> pinned;

    [MyCmpReq]
    private WorldContainer worldContainer;

    private bool discovered;

    public RobotInventory() {
        this.instances = new Dictionary<RobotType, HashSet<TrackedRobot>>(INITIAL_RTYPE_CAPACITY);
    }

    internal void RegisterInstance(RobotType type, TrackedRobot robot) {
        if (robot == null) {
            return;
        }

        bool hadSet = instances.TryGetValue(type, out var set) && set != null;
        bool newType = !hadSet;
        if (!hadSet) {
            set = new HashSet<TrackedRobot>();
            instances[type] = set;
            this.discovered = true;
        }
        if (!set.Add(robot)) {
            // Already tracked.
            return;
        }

        RefreshUIIfActiveWorld(forcePopulate: newType);
    }

    public ISet<RobotType> GetPinnedTypes() {
        return this.pinned;
    }

    private int GetWorldID() {
        return (this.worldContainer != null) ? this.worldContainer.id : -1;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        this.worldContainer = GetComponent<WorldContainer>();
        if (this.pinned == null) {
            this.pinned = new HashSet<RobotType>();
        }
    }

    internal int PopulateCounts(IDictionary<RobotType, int> results) {
        int all = 0;
        foreach (var pair in instances) {
            var robotType = pair.Key;
            var set = pair.Value;
            int count = set != null ? set.Count : 0;
            if (results != null && !results.ContainsKey(robotType)) {
                results.Add(robotType, count);
            }
            all += count;
        }
        return all;
    }

    internal bool ConsumeDiscoveredFlag() {
        bool d = this.discovered;
        this.discovered = false;
        return d;
    }

    internal IReadOnlyCollection<TrackedRobot> GetInstances(RobotType type) {
        if (instances.TryGetValue(type, out var set) && set != null) {
            return set;
        }
        return System.Array.Empty<TrackedRobot>();
    }

    internal void UnregisterInstance(RobotType type, TrackedRobot robot) {
        if (robot == null) {
            return;
        }

        bool removed = false;
        if (instances.TryGetValue(type, out var set) && set != null) {
            removed = set.Remove(robot);
        }

        // Only refresh if we actually tracked this instance.
        if (removed) {
            RefreshUIIfActiveWorld(forcePopulate: false);
        }
    }

    private void RefreshUIIfActiveWorld(bool forcePopulate) {
        int worldId = GetWorldID();
        var cm = ClusterManager.Instance;
        if (cm == null || cm.activeWorldId != worldId) {
            return;
        }

        var resourcesScreen = AllResourcesScreen.Instance;
        if (resourcesScreen != null) {
            if (forcePopulate || ConsumeDiscoveredFlag()) {
                resourcesScreen.Populate();
            } else {
                resourcesScreen.RefreshRows();
            }
        }

        var pinnedPanel = PinnedResourcesPanel.Instance;
        if (pinnedPanel != null) {
            pinnedPanel.Refresh();
        }
    }
}
