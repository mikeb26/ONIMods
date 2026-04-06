/*
 * Original Copyright 2026 Peter Han; see LICENSE in this directory
 */

using System;
using System.Collections.Generic;
using UnityEngine.UI;

using PinnedRobotsPerType = System.Collections.Generic.SortedList<Tag, HierarchyReferences>;

namespace BotTweaks.ResourceScreen;

/// <summary>
/// An addon component to PinnedResourcesPanel which manages the critter-related entries.
/// </summary>
public sealed class PinnedRobotManager : KMonoBehaviour {
    /// <summary>
    /// Refreshes one pinned critter entry.
    /// </summary>
    /// <param name="refs">The row to refresh.</param>
    /// <param name="available">The quantity of the critter available.</param>
    private static void RefreshLine(HierarchyReferences refs, int available) {
        refs.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedSimple(
            available));
    }

    /// <summary>
    /// True if critter rows were added or removed since the last sync.
    /// </summary>
    public bool IsDirty { get; private set; }

    /// <summary>
    /// The realized game objects of pinned critter entries.
    /// </summary>
    private readonly IDictionary<RobotType, PinnedRobotsPerType> pinnedObjects;

#pragma warning disable CS0649
#pragma warning disable IDE0044
    // This field is automatically populated by KMonoBehaviour
    [MyCmpReq]
    private PinnedResourcesPanel pinnedResources;
#pragma warning restore IDE0044
#pragma warning restore CS0649

    public PinnedRobotManager() {
        this.pinnedObjects = new SortedList<RobotType, PinnedRobotsPerType>(4);
        foreach (var type in Enum.GetValues(typeof(RobotType))) {
            if (type is RobotType ct) {
                this.pinnedObjects.Add(ct, new PinnedRobotsPerType(8, TagComparer.INSTANCE));
            }
        }
        this.IsDirty = true;
    }

    /// <summary>
    /// Creates a new pinned critter row.
    /// </summary>
    /// <param name="species">The species to pin.</param>
    /// <param name="type">The critter type to pin.</param>
    /// <returns>A pinned row with that critter type and species displayed.</returns>
    internal HierarchyReferences Create(Tag species, RobotType type) {
        var newRow = Util.KInstantiateUI(this.pinnedResources.linePrefab,
            this.pinnedResources.rowContainer);
        var refs = newRow.GetComponent<HierarchyReferences>();
        var imageData = Def.GetUISprite(species);
        if (imageData != null) {
            var icon = refs.GetReference<Image>("Icon");
            icon.sprite = imageData.first;
            icon.color = imageData.second;
        }
        refs.GetReference<LocText>("NameLabel").SetText(RobotInventoryUtils.GetTitle(
            species, type));
        refs.GetReference("NewLabel").gameObject.SetActive(false);
        var pinRow = newRow.AddComponent<PinnedRobotEntry>();
        pinRow.RobotType = type;
        pinRow.Species = species;
        refs.GetReference<MultiToggle>("PinToggle").onClick = pinRow.OnUnpin;
        if (newRow.TryGetComponent(out MultiToggle mt)) {
            mt.onClick += pinRow.OnCycleThrough;
        }
        return refs;
    }

    /// <summary>
    /// Populates the pinned critters in the resource panel, creating new rows if needed.
    /// </summary>
    internal void PopulatePinnedRows() {
        var cm = ClusterManager.Instance;
        if (cm != null && cm.activeWorld.TryGetComponent(out RobotInventory ci)) {
            var seen = HashSetPool<Tag, PinnedRobotManager>.Allocate();
            foreach (var pair in this.pinnedObjects) {
                var type = pair.Key;
                var have = pair.Value;
                foreach (var species in ci.GetPinnedSpecies(type)) {
                    // Check for existing pinned row
                    if (!have.TryGetValue(species, out var entry)) {
                        have.Add(species, entry = this.Create(species, type));
                    }
                    var row = entry.gameObject;
                    if (!row.activeSelf) {
                        row.SetActive(true);
                    }
                    seen.Add(species);
                }
                // Hide entries that have been removed from pinned list
                foreach (var speciesPair in have) {
                    var row = speciesPair.Value.gameObject;
                    if (!seen.Contains(speciesPair.Key)) {
                        if (row.activeSelf) {
                            row.SetActive(false);
                        }
                    } else {
                        // These will be traversed in sorted order
                        row.transform.SetAsLastSibling();
                    }
                }
                seen.Clear();
            }
            seen.Recycle();
            // Move the buttons to the end
            this.pinnedResources.clearNewButton.transform.SetAsLastSibling();
            this.pinnedResources.seeAllButton.transform.SetAsLastSibling();
            this.IsDirty = false;
        }
    }

    /// <summary>
    /// Sets the dirty flag to force rebuild pinned critters on the next run.
    /// </summary>
    internal void SetDirty() {
        this.IsDirty = true;
    }

    /// <summary>
    /// Updates the critter counts of visible pinned rows.
    /// </summary>
    internal void UpdateContents() {
        var cm = ClusterManager.Instance;
        if (cm != null && cm.activeWorld.TryGetComponent(out RobotInventory ci)) {
            var allCounts = DictionaryPool<Tag, RobotTotals, PinnedRobotManager>.
                Allocate();
            foreach (var pair in this.pinnedObjects) {
                allCounts.Clear();
                ci.PopulateTotals(pair.Key, allCounts);
                foreach (var speciesPair in pair.Value) {
                    // Only refresh active rows
                    var entry = speciesPair.Value;
                    if (entry.gameObject.activeSelf) {
                        int available = 0;
                        if (allCounts.TryGetValue(speciesPair.Key, out var totals)) {
                            available = totals.Available;
                        }
                        RefreshLine(entry, available);
                    }
                }
            }
            allCounts.Recycle();
        }
    }
}
