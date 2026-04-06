/*
 * Original Copyright 2026 Peter Han; see LICENSE in this package directory
 * Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this
 *   package
 */

using PeterHan.PLib.Core;
using UnityEngine;

namespace BotTweaks.ResourceScreen;

// derived from https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/NewResourceScreen/PinnedCritterEntry.cs

/// <summary>
/// Handles clicks and cycling through on a pinned robot type in the resource list.
/// </summary>
public sealed class PinnedRobotEntry : MonoBehaviour {
    public RobotType RobotType { get; set; }

    private int selectionIndex;

    public PinnedRobotEntry() {
        this.selectionIndex = 0;
    }

    internal void OnCycleThrough() {
        var matching = ListPool<KPrefabID, PinnedRobotEntry>.Allocate();

        // Use the active world's RobotInventory instance set (event-driven).
        if (ResourceScreenUtils.TryGetActiveInventory(out var inv)) {
            foreach (var tracked in inv.GetInstances(this.RobotType)) {
                var kpid = tracked != null ? tracked.GetKPrefabIDSafe() : null;
                if (kpid != null) {
                    matching.Add(kpid);
                }
            }
        }

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

    internal void OnUnpin() {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            return;
        }
        var ai = AllResourcesScreen.Instance;
        var pi = PinnedResourcesPanel.Instance;

        ri.GetPinnedTypes().Remove(this.RobotType);

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
