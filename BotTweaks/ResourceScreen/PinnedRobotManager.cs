// Copyright 2026 Peter Han; see LICENSE in this package directory
// Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Core;

using System.Collections.Generic;
using UnityEngine.UI;

namespace BotTweaks.ResourceScreen;

// derived from https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/NewResourceScreen/PinnedCritterManager.cs

/// <summary>
/// An addon component to PinnedResourcesPanel which manages robot-related entries.
/// </summary>
public sealed class PinnedRobotManager : KMonoBehaviour {
    private static void RefreshLine(HierarchyReferences refs, int available) {
        refs.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedSimple(available));
    }

    public bool IsDirty { get; private set; }

    private readonly SortedList<RobotType, HierarchyReferences> pinnedObjects;

    [MyCmpReq]
    private PinnedResourcesPanel pinnedResources;

    public PinnedRobotManager() {
        this.pinnedObjects = new SortedList<RobotType, HierarchyReferences>(8);
        this.IsDirty = true;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        this.pinnedResources = GetComponent<PinnedResourcesPanel>();
    }

    internal HierarchyReferences Create(RobotType robotType) {
        var newRow = global::Util.KInstantiateUI(this.pinnedResources.linePrefab, this.pinnedResources.rowContainer);
        var refs = newRow.GetComponent<HierarchyReferences>();
        var icon = refs.GetReference<Image>("Icon");

        ResourceScreenUtils.ApplyRobotIcon(newRow, icon, robotType);

        refs.GetReference<LocText>("NameLabel").SetText(RobotTrackingUtils.GetRobotDisplayName(robotType));
        refs.GetReference("NewLabel").gameObject.SetActive(false);

        // Unpin uses our own pinned store.
        var pinRow = newRow.AddComponent<PinnedRobotEntry>();
        pinRow.RobotType = robotType;
        refs.GetReference<MultiToggle>("PinToggle").onClick = pinRow.OnUnpin;

        if (newRow.TryGetComponent(out MultiToggle mt)) {
            mt.onClick += pinRow.OnCycleThrough;
        }

        // No notify toggle for robots yet.
        refs.GetReference<MultiToggle>("NotifyToggle").gameObject.SetActive(false);

        return refs;
    }

    internal void PopulatePinnedRows() {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            Util.LogDbg("PinnedRobotManager.PopulatePinnedRows: no active world RobotInventory");
            return;
        }
        var pinned = ri.GetPinnedTypes();
        if (pinned == null) {
            Util.LogDbg("PinnedRobotManager.PopulatePinnedRows: pinned set was null");
            return;
        }

        Util.LogDbg("PinnedRobotManager.PopulatePinnedRows: pinned count {0}", pinned.Count);

        var seen = new HashSet<RobotType>();
        foreach (var pinnedType in pinned) {
            if (!this.pinnedObjects.TryGetValue(pinnedType, out var entry)) {
                this.pinnedObjects.Add(pinnedType, entry = this.Create(pinnedType));
            }

            var row = entry.gameObject;
            if (!row.activeSelf) {
                row.SetActive(true);
            }
            seen.Add(pinnedType);
        }

        foreach (var pinnedTypePair in this.pinnedObjects) {
            var row = pinnedTypePair.Value.gameObject;
            if (!seen.Contains(pinnedTypePair.Key)) {
                if (row.activeSelf) {
                    row.SetActive(false);
                }
            } else {
                row.transform.SetAsLastSibling();
            }
        }

        this.pinnedResources.clearNewButton.transform.SetAsLastSibling();
        this.pinnedResources.seeAllButton.transform.SetAsLastSibling();
        this.IsDirty = false;
    }

    internal void SetDirty() {
        this.IsDirty = true;
    }

    internal void UpdateContents() {
        if (!ResourceScreenUtils.TryGetActiveInventory(out var ri)) {
            Util.LogDbg("PinnedRobotManager.UpdateContents: no active world RobotInventory");
            return;
        }
        var allCounts = new Dictionary<RobotType, int>();
        ri.PopulateCounts(allCounts);

        Util.LogDbg("PinnedRobotManager.UpdateContents: active pinned rows {0}, known types {1}", this.pinnedObjects.Count, allCounts.Count);

        foreach (var pinnedTypePair in this.pinnedObjects) {
            var entry = pinnedTypePair.Value;
            if (entry.gameObject.activeSelf) {
                int available = 0;
                if (allCounts.TryGetValue(pinnedTypePair.Key, out var count)) {
                    available = count;
                }
                RefreshLine(entry, available);
            }
        }
    }
}
