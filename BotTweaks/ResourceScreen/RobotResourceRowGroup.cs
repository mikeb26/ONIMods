/*
 * Original Copyright 2026 Peter Han; see LICENSE in this package directory
 * Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this
 *   package
 */

using System.Collections.Generic;
using UnityEngine.UI;

// derived from https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/NewResourceScreen/CritterResourceRowGroup.cs

namespace BotTweaks.ResourceScreen;

/// <summary>
/// Additional information for robot categories in the All Resources screen.
/// </summary>
public sealed class RobotResourceRowGroup : KMonoBehaviour {
    public bool IsVisible { get; set; }

    private readonly IDictionary<RobotType, RobotResourceRow> resources;

    [MyCmpReq]
    private HierarchyReferences refs;

    public RobotResourceRowGroup() {
        this.IsVisible = true;
        this.resources = new SortedList<RobotType, RobotResourceRow>();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        this.refs = GetComponent<HierarchyReferences>();
    }

    private RobotResourceRow Create(AllResourcesScreen allResources, RobotType type) {
        var spawn = global::Util.KInstantiateUI(allResources.resourceLinePrefab, this.refs.GetComponent<FoldOutPanel>().container, true);
        var rr = spawn.AddComponent<RobotResourceRow>();
        rr.RobotType = type;

        if (spawn.TryGetComponent(out MultiToggle toggle)) {
            toggle.onClick += rr.OnPinToggle;
        }

        if (spawn.TryGetComponent(out HierarchyReferences newRefs)) {
            var icon = newRefs.GetReference<Image>("Icon");
            ResourceScreenUtils.ApplyRobotIcon(spawn, icon, type);
            rr.References = newRefs;
            newRefs.GetReference<LocText>("NameLabel").SetText(rr.Title);
            newRefs.GetReference<MultiToggle>("PinToggle").onClick += rr.OnPinToggle;
        }

        // Disable notify toggle: not supported for robots yet.
        if (spawn.TryGetComponent(out HierarchyReferences r2))
            r2.GetReference<MultiToggle>("NotificationToggle").gameObject.SetActive(false);

        return rr;
    }

    internal void SearchFilter(string searchUpper) {
        foreach (var resource in this.resources) {
            var rr = resource.Value;
            rr.IsVisible = RobotTrackingUtils.PassesSearchFilterForRow(rr.Title, searchUpper);
        }
    }

    internal void SetRowsActive() {
        bool visible = this.IsVisible && this.resources.Count > 0;
        foreach (var resource in this.resources) {
            var rr = resource.Value;
            bool showRow = rr.IsVisible;
            var go = rr.gameObject;
            if (go != null && showRow != go.activeSelf) {
                go.SetActive(showRow);
            }
            visible |= showRow;
        }

        if (gameObject.activeSelf != visible) {
            gameObject.SetActive(visible);
        }
    }

    internal void SpawnRows(AllResourcesScreen allResources) {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            Util.LogDbg("RobotResourceRowGroup.SpawnRows: no active world RobotInventory");
            return;
        }
        var allRobots = new Dictionary<RobotType, int>();
        ri.PopulateCounts(allRobots);
        Util.LogDbg("RobotResourceRowGroup.SpawnRows: found robot types: {0}", allRobots.Count);
        bool dirty = false;
        foreach (var pair in allRobots) {
            var type = pair.Key;
            if (!this.resources.ContainsKey(type)) {
                Util.LogDbg("RobotResourceRowGroup.SpawnRows: creating row for {0}", type);
                this.resources.Add(type, this.Create(allResources, type));
                dirty = true;
            }
        }

        if (dirty) {
            foreach (var resource in this.resources) {
                resource.Value.gameObject.transform.SetAsLastSibling();
            }
            UpdateContents();
        }
    }

    internal void UpdateContents() {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            Util.LogDbg("RobotResourceRowGroup.UpdateContents: no active world RobotInventory");
            return;
        }

        var allCounts = new Dictionary<RobotType, int>();
        int total = ri.PopulateCounts(allCounts);
        Util.LogDbg("RobotResourceRowGroup.UpdateContents: total robots {0} across {1} types", total, allCounts.Count);

        // Reuse the existing columns: show the count in Available and Total, and 0 in Reserved.
        this.refs.GetReference<LocText>("AvailableLabel").SetText(GameUtil.GetFormattedSimple(total));
        this.refs.GetReference<LocText>("TotalLabel").SetText(GameUtil.GetFormattedSimple(total));
        this.refs.GetReference<LocText>("ReservedLabel").SetText(GameUtil.GetFormattedSimple(0));

        foreach (var resource in this.resources) {
            resource.Value.UpdateContents(allCounts, ri);
        }
    }
}
