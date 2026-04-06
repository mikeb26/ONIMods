/*
 * Original Copyright 2026 Peter Han; see LICENSE in this directory
 */

using PeterHan.PLib.Core;
using UnityEngine;

namespace BotTweaks.ResourceScreen;

/// <summary>
/// Handles clicks and cycling through on a pinned critter type in the resource list.
/// </summary>
public sealed class PinnedRobotEntry : MonoBehaviour {
    /// <summary>
    /// The critter type for this resource row.
    /// </summary>
    public RobotType RobotType { get; set; }

    /// <summary>
    /// The critter species for this resource row.
    /// </summary>
    public Tag Species { get; set; }

    /// <summary>
    /// The current index when cycling through critters.
    /// </summary>
    private int selectionIndex;

    public PinnedRobotEntry() {
        this.selectionIndex = 0;
    }

    /// <summary>
    /// Cycles through critters of this type.
    /// </summary>
    internal void OnCycleThrough() {
        int id = ClusterManager.Instance.activeWorldId;
        var matching = ListPool<KPrefabID, PinnedRobotEntry>.Allocate();
        var type = this.RobotType;
        // Compile a list of critters matching this species
        RobotInventoryUtils.GetRobots(id, (kpid) => {
            if (kpid.GetRobotType() == type) {
                matching.Add(kpid);
            }
        }, this.Species);
        int n = matching.Count;
        if (this.selectionIndex >= n) {
            this.selectionIndex = 0;
        } else {
            this.selectionIndex = (this.selectionIndex + 1) % n;
        }
        if (n > 0) {
            PGameUtils.CenterAndSelect(matching[this.selectionIndex]);
        }
        matching.Recycle();
    }

    /// <summary>
    /// Unpins this critter type from the list.
    /// </summary>
    internal void OnUnpin() {
        var cm = ClusterManager.Instance;
        if (cm != null && cm.activeWorld.TryGetComponent(out RobotInventory ci)) {
            var ai = AllResourcesScreen.Instance;
            var pi = PinnedResourcesPanel.Instance;
            ci.GetPinnedSpecies(this.RobotType).Remove(this.Species);
            if (ai != null) {
                ai.RefreshRows();
            }
            if (pi != null) {
                if (pi.TryGetComponent(out PinnedRobotManager pm)) {
                    pm.SetDirty();
                }
                pi.Refresh();
            }
        }
    }
}
