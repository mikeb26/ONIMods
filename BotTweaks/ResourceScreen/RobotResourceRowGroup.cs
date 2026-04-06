/*
 * Original Copyright 2026 Peter Han; see LICENSE in this directory
 */

#if DEBUG
using PeterHan.PLib.Core;
#endif
using System.Collections.Generic;
using UnityEngine.UI;

namespace BotTweaks.ResourceScreen;

/// <summary>
/// A marker class used to annotate additional information regarding the critter
/// information to be displayed by a category in the new resources screen.
/// </summary>
public sealed class RobotResourceRowGroup : KMonoBehaviour {
    /// <summary>
    /// The critter type for this resource group.
    /// </summary>
    public RobotType RobotType { get; set; }

    /// <summary>
    /// Whether this row group should be visible as a whole.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// The title displayed on screen.
    /// </summary>
    public string Title => RobotInventoryUtils.GetTitle(GameTags.BagableCreature,
        this.RobotType);

    /// <summary>
    /// The resources being displayed by this group.
    /// </summary>
    private readonly IDictionary<Tag, RobotResourceRow> resources;

#pragma warning disable CS0649
#pragma warning disable IDE0044
    // This field is automatically populated by KMonoBehaviour
    [MyCmpReq]
    private HierarchyReferences refs;
#pragma warning restore IDE0044
#pragma warning restore CS0649

    public RobotResourceRowGroup() {
        this.IsVisible = true;
        this.resources = new SortedList<Tag, RobotResourceRow>(32, TagComparer.INSTANCE);
    }

    /// <summary>
    /// Creates a resource category row for critters.
    /// </summary>
    /// <param name="allResources">The resources screen where the row should be added.</param>
    /// <param name="species">The critter species to create.</param>
    /// <returns>The row for that critter species.</returns>
    private RobotResourceRow Create(AllResourcesScreen allResources, Tag species) {
        var spawn = Util.KInstantiateUI(allResources.resourceLinePrefab, this.refs.
            GetComponent<FoldOutPanel>().container, true);
#if DEBUG
        PUtil.LogDebug("Creating resource row for {0}".F(species.ProperNameStripLink()));
#endif
        // Component which actually handles updating
        var cr = spawn.AddComponent<RobotResourceRow>();
        cr.RobotType = this.RobotType;
        cr.Species = species;
        if (spawn.TryGetComponent(out MultiToggle toggle)) {
            toggle.onClick += cr.OnPinToggle;
        }
        if (spawn.TryGetComponent(out HierarchyReferences newRefs)) {
            var image = Def.GetUISprite(species);
            // Tint icon the correct color
            if (image != null) {
                var icon = newRefs.GetReference<Image>("Icon");
                icon.sprite = image.first;
                icon.color = image.second;
            }
            // Set up chart
            if (newRefs.GetReference<SparkLayer>("Chart").TryGetComponent(out GraphBase
                    graphBase)) {
                graphBase.axis_x.min_value = 0f;
                graphBase.axis_x.max_value = 600f;
                graphBase.axis_x.guide_frequency = 120f;
                graphBase.RefreshGuides();
            }
            cr.References = newRefs;
            newRefs.GetReference<LocText>("NameLabel").SetText(cr.Title);
            // Checkmark to pin to resource list
            newRefs.GetReference<MultiToggle>("PinToggle").onClick += cr.OnPinToggle;
        }
        return cr;
    }

    /// <summary>
    /// Filters rows by the user search query.
    /// </summary>
    /// <param name="search">The search query to use.</param>
    internal void SearchFilter(string search) {
        foreach (var resource in this.resources) {
            var cr = resource.Value;
            cr.IsVisible = RobotInventoryUtils.PassesSearchFilter(cr.Title, search);
        }
    }

    /// <summary>
    /// Shows or hides rows depending on their visibility flags.
    /// </summary>
    internal void SetRowsActive() {
        bool visible = this.IsVisible && this.resources.Count > 0;
        foreach (var resource in this.resources) {
            var cr = resource.Value;
            bool showRow = cr.IsVisible;
            // If any row is visible, header must also be
            var go = cr.gameObject;
            if (go != null && showRow != go.activeSelf) {
                go.SetActive(showRow);
            }
            visible |= showRow;
        }
        // Update visibility if dirty
        if (this.gameObject.activeSelf != visible) {
            this.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// Creates new rows if necessary for each critter species, and sorts them by name.
    /// </summary>
    /// <param name="allResources">The parent window for the rows.</param>
    internal void SpawnRows(AllResourcesScreen allResources) {
        var cm = ClusterManager.Instance;
        if (cm != null && cm.activeWorld.TryGetComponent(out RobotInventory ci)) {
            var allRobots = DictionaryPool<Tag, RobotTotals, RobotResourceRowGroup>.
                Allocate();
            ci.PopulateTotals(this.RobotType, allRobots);
            bool dirty = false;
            // Insert new rows where necessary
            foreach (var pair in allRobots) {
                var species = pair.Key;
                if (!this.resources.ContainsKey(species)) {
                    this.resources.Add(species, this.Create(allResources, species));
                    dirty = true;
                }
            }
            // Iterate and place in SORTED order in the UI
            if (dirty) {
                foreach (var resource in this.resources) {
                    resource.Value.gameObject.transform.SetAsLastSibling();
                }
            }
            allRobots.Recycle();
            if (dirty) {
                this.UpdateContents();
            }
        }
    }

    /// <summary>
    /// Updates the graphs for the entire category.
    /// </summary>
    internal void UpdateCharts() {
        const float HISTORY = RobotInventoryUtils.CYCLES_TO_CHART *
            Constants.SECONDS_PER_CYCLE;
        float currentTime = GameClock.Instance.GetTime();
        var tracker = RobotInventoryUtils.GetTracker<AllRobotTracker>(ClusterManager.
            Instance.activeWorldId, this.RobotType);
        var chart = this.refs.GetReference<SparkLayer>("Chart");
        var chartableData = tracker.ChartableData(HISTORY);
        ref var xAxis = ref chart.graph.axis_x;
        xAxis.max_value = chartableData.Length > 0 ? chartableData[chartableData.
            Length - 1].first : 0f;
        xAxis.min_value = currentTime - HISTORY;
        chart.RefreshLine(chartableData, "resourceAmount");
        foreach (var resource in this.resources) {
            resource.Value.UpdateChart(currentTime);
        }
    }

    /// <summary>
    /// Updates the headings for the entire category.
    /// </summary>
    internal void UpdateContents() {
        var cm = ClusterManager.Instance;
        if (cm != null && cm.activeWorld.TryGetComponent(out RobotInventory ci)) {
            var allTotals = DictionaryPool<Tag, RobotTotals, RobotResourceRowGroup>.
                Allocate();
            var totals = ci.PopulateTotals(this.RobotType, allTotals);
            this.refs.GetReference<LocText>("AvailableLabel").SetText(GameUtil.
                GetFormattedSimple(totals.Available));
            this.refs.GetReference<LocText>("TotalLabel").SetText(GameUtil.
                GetFormattedSimple(totals.Total));
            this.refs.GetReference<LocText>("ReservedLabel").SetText(GameUtil.
                GetFormattedSimple(totals.Reserved));
            foreach (var resource in this.resources) {
                resource.Value.UpdateContents(allTotals, ci);
            }
            allTotals.Recycle();
        }
    }
}
