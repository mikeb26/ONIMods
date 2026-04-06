/*
 * Original Copyright 2026 Peter Han; see LICENSE in this package directory
 * Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this
 *   package
 */

using System.Collections.Generic;
using UnityEngine;

namespace BotTweaks.ResourceScreen;

// derived from https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/NewResourceScreen/CritterResourceRow.cs

/// <summary>
/// Row in the All Resources screen for a specific robot type.
/// </summary>
public sealed class RobotResourceRow : MonoBehaviour {
    public RobotType RobotType { get; set; }

    public bool IsVisible { get; set; }

    internal HierarchyReferences References { get; set; }

    public string Title => RobotTrackingUtils.GetRobotDisplayName(this.RobotType);

    public RobotResourceRow() {
        this.IsVisible = true;
    }

    internal void OnPinToggle() {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            return;
        }
        ISet<RobotType> pinned;
        if (ri == null || (pinned = ri.GetPinnedTypes()) == null) {
            return;
        }
        var inst = PinnedResourcesPanel.Instance;

        if (!pinned.Remove(this.RobotType)) {
            pinned.Add(this.RobotType);
        }

        UpdatePinnedState(ri);

        if (inst != null) {
            if (inst.TryGetComponent(out PinnedRobotManager pm)) {
                pm.SetDirty();
            }
            inst.Refresh();
        }
    }

    internal void UpdateContents(IDictionary<RobotType, int> allCounts, RobotInventory ri) {
        if (!allCounts.TryGetValue(RobotType, out var count))
            count = 0;

        // Reuse the existing columns: show the count in Available and Total, and 0 in Reserved.
        this.References.GetReference<LocText>("AvailableLabel").SetText(GameUtil.GetFormattedSimple(count));
        this.References.GetReference<LocText>("TotalLabel").SetText(GameUtil.GetFormattedSimple(count));
        this.References.GetReference<LocText>("ReservedLabel").SetText(GameUtil.GetFormattedSimple(0));
        UpdatePinnedState(ri);
    }

    private void UpdatePinnedState(RobotInventory ri) {
        this.References.GetReference<MultiToggle>("PinToggle").ChangeState(ri.GetPinnedTypes().Contains(this.RobotType) ? 1 : 0);
    }
}
