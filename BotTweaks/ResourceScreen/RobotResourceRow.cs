/*
 * Original Copyright 2026 Peter Han; see LICENSE in this directory
 */

using System.Collections.Generic;
using UnityEngine;

namespace BotTweaks.ResourceScreen;

/// <summary>
/// A marker class used to annotate additional information regarding the critter
/// information to be displayed by a row in the new resources screen.
/// </summary>
public sealed class RobotResourceRow : MonoBehaviour {
    /// <summary>
    /// The critter type for this resource row.
    /// </summary>
    public RobotType RobotType { get; set; }

    /// <summary>
    /// Whether this row group should be visible as a whole.
    /// </summary>
    public bool IsVisible { get; set; }

    // [MyCmpReq] populates it too late, since this component can start inactive
    internal HierarchyReferences References { get; set; }

    /// <summary>
    /// The critter species for this resource row.
    /// </summary>
    public Tag Species { get; set; }

    /// <summary>
    /// The title displayed on screen.
    /// </summary>
    public string Title => RobotInventoryUtils.GetTitle(this.Species, this.RobotType);

    public RobotResourceRow() {
        this.IsVisible = true;
    }

    /// <summary>
    /// Called when the resource is pinned/unpinned.
    /// </summary>
    internal void OnPinToggle() {
        var ci = ClusterManager.Instance.activeWorld.GetComponent<RobotInventory>();
        ISet<Tag> pinned;
        if (ci != null && (pinned = ci.GetPinnedSpecies(this.RobotType)) != null) {
            var inst = PinnedResourcesPanel.Instance;
            // Toggle membership in pinned set
            if (!pinned.Remove(this.Species)) {
                pinned.Add(this.Species);
            }
            // Toggle visual checkbox
            this.UpdatePinnedState(ci);
            if (inst != null) {
                if (inst.TryGetComponent(out PinnedRobotManager pm)) {
                    pm.SetDirty();
                }
                inst.Refresh();
            }
        }
        // TODO Notify checkbox is not implemented yet in stock game?
    }

    /// <summary>
    /// Updates the graph for this critter species.
    /// </summary>
    /// <param name="currentTime">The current time from GameClock.</param>
    internal void UpdateChart(float currentTime) {
        const float HISTORY = RobotInventoryUtils.CYCLES_TO_CHART *
            Constants.SECONDS_PER_CYCLE;
        var tracker = RobotInventoryUtils.GetTracker<RobotTracker>(ClusterManager.
            Instance.activeWorldId, this.RobotType, (t) => t.Tag == this.Species);
        if (tracker != null) {
            var chart = this.References.GetReference<SparkLayer>("Chart");
            var chartableData = tracker.ChartableData(HISTORY);
            ref var xAxis = ref chart.graph.axis_x;
            xAxis.max_value = chartableData.Length > 0 ? chartableData[chartableData.
                Length - 1].first : 0f;
            xAxis.min_value = currentTime - HISTORY;
            chart.RefreshLine(chartableData, "resourceAmount");
        }
    }

    /// <summary>
    /// Updates the headings for this critter species.
    /// </summary>
    /// <param name="allTotals">The total critter counts for all species.</param>
    /// <param name="ci">The currently active critter inventory.</param>
    internal void UpdateContents(IDictionary<Tag, RobotTotals> allTotals,
            RobotInventory ci) {
        if (!allTotals.TryGetValue(this.Species, out var totals)) {
            totals = new RobotTotals();
        }
        this.References.GetReference<LocText>("AvailableLabel").SetText(GameUtil.
            GetFormattedSimple(totals.Available));
        this.References.GetReference<LocText>("TotalLabel").SetText(GameUtil.
            GetFormattedSimple(totals.Total));
        this.References.GetReference<LocText>("ReservedLabel").SetText(GameUtil.
            GetFormattedSimple(totals.Reserved));
        this.UpdatePinnedState(ci);
    }

    /// <summary>
    /// Updates the pin checkbox to match the actual pinned state.
    /// </summary>
    /// <param name="ci">The currently active critter inventory.</param>
    private void UpdatePinnedState(RobotInventory ci) {
        this.References.GetReference<MultiToggle>("PinToggle").ChangeState(ci.
            GetPinnedSpecies(this.RobotType).Contains(this.Species) ? 1 : 0);
    }
}
