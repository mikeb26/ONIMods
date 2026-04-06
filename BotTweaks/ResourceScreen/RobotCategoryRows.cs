/*
 * Original Copyright 2026 Peter Han; see LICENSE in this directory
 */

using PeterHan.PLib.Core;
using System;
using System.Collections.Generic;

namespace BotTweaks.ResourceScreen;

/// <summary>
/// For the new resources screen, stores references to the custom Robot rows.
/// </summary>
public sealed class RobotCategoryRows : KMonoBehaviour {
    /// <summary>
    /// The headers for each critter type.
    /// </summary>
    private readonly IList<RobotResourceRowGroup> headers;

#pragma warning disable CS0649
#pragma warning disable IDE0044
    // This field is automatically populated by KMonoBehaviour
    [MyCmpReq]
    private AllResourcesScreen allResources;
#pragma warning restore IDE0044
#pragma warning restore CS0649

    public RobotCategoryRows() {
        this.headers = new List<RobotResourceRowGroup>(4);
    }

    /// <summary>
    /// Creates a resource category header for critters.
    /// </summary>
    /// <param name="type">The critter type to create.</param>
    /// <returns>The heading for that critter type.</returns>
    private RobotResourceRowGroup Create(RobotType type) {
        var spawn = Util.KInstantiateUI(this.allResources.categoryLinePrefab, this.
            this.allResources.rootListContainer, true);
        // Create a heading for Robot (Type)
        PUtil.LogDebug("Creating Robot ({0}) category".F(type.GetProperName()));
        // Component which actually handles updating
        var rg = spawn.AddComponent<RobotResourceRowGroup>();
        rg.RobotType = type;
        if (spawn.TryGetComponent(out HierarchyReferences refs)) {
            // Set up chart
            if (refs.GetReference<SparkLayer>("Chart").TryGetComponent(
                    out GraphBase graphBase)) {
                graphBase.axis_x.min_value = 0f;
                graphBase.axis_x.max_value = 600f;
                graphBase.axis_x.guide_frequency = 120f;
                graphBase.RefreshGuides();
            }
            refs.GetReference<LocText>("NameLabel").SetText(rg.Title);
        }
        return rg;
    }

    /// <summary>
    /// Filters rows and categories by the user search query.
    /// </summary>
    /// <param name="search">The search query to use.</param>
    internal void SearchFilter(string search) {
        // Use current culture
        string searchUp = search.ToUpper();
        foreach (var header in this.headers) {
            // Runs in prefix before SetRowsActive
            header.IsVisible = RobotInventoryUtils.PassesSearchFilter(header.Title,
                searchUp);
            header.SearchFilter(searchUp);
        }
    }

    /// <summary>
    /// Shows or hides rows depending on their visibility flags.
    /// </summary>
    internal void SetRowsActive() {
        foreach (var header in this.headers) {
            header.SetRowsActive();
        }
    }

    /// <summary>
    /// Spawns the category headers for critters if necessary.
    /// </summary>
    internal void SpawnRows() {
        if (this.headers.Count < 1) {
            foreach (var type in Enum.GetValues(typeof(RobotType))) {
                if (type is RobotType ct) {
                    this.headers.Add(this.Create(ct));
                }
            }
        }
        foreach (var header in this.headers) {
            header.SpawnRows(this.allResources);
        }
    }

    /// <summary>
    /// Updates the charts for all categories.
    /// </summary>
    internal void UpdateCharts() {
        foreach (var header in this.headers) {
            header.UpdateCharts();
        }
    }

    /// <summary>
    /// Updates the critter headers. No alternation is performed, as it does not actually
    /// index the critters in-world, and the base game expects the checked state on
    /// the pinned panel to be updated immediately.
    /// </summary>
    internal void UpdateContents() {
        foreach (var header in this.headers) {
            header.UpdateContents();
        }
    }
}
