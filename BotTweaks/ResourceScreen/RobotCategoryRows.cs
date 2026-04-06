/*
 * Original Copyright 2026 Peter Han; see LICENSE in this package directory
 * Adaptations for BotTweaks Copyright © 2026 Mike Brown; see LICENSE at the root of this
 *   package
 */

using System.Collections.Generic;

namespace BotTweaks.ResourceScreen;

// derived from https://github.com/peterhaneve/ONIMods/blob/main/CritterInventory/NewResourceScreen/CritterCategoryRows.cs

/// <summary>
/// For the new resources screen, stores references to custom Robot rows.
/// </summary>
public sealed class RobotCategoryRows : KMonoBehaviour {
    private readonly IList<RobotResourceRowGroup> headers;

    [MyCmpReq]
    private AllResourcesScreen allResources;

    public RobotCategoryRows() {
        this.headers = new List<RobotResourceRowGroup>();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        this.allResources = GetComponent<AllResourcesScreen>();
    }

    private RobotResourceRowGroup Create() {
        var spawn = global::Util.KInstantiateUI(this.allResources.categoryLinePrefab, this.allResources.rootListContainer, true);
        var rg = spawn.AddComponent<RobotResourceRowGroup>();

        if (spawn.TryGetComponent(out HierarchyReferences refs)) {
            if (refs.GetReference<SparkLayer>("Chart").TryGetComponent(out GraphBase graphBase)) {
                graphBase.axis_x.min_value = 0f;
                graphBase.axis_x.max_value = 600f;
                graphBase.axis_x.guide_frequency = 120f;
                graphBase.RefreshGuides();
            }
            refs.GetReference<LocText>("NameLabel").SetText(Strings.UI.FRONTEND.BOTTWEAKS.ROBOTS_CAT);
        }

        return rg;
    }

    internal void SearchFilter(string search) {
        string searchUp = (search ?? string.Empty);
        foreach (var header in this.headers) {
            header.IsVisible = RobotTrackingUtils.PassesSearchFilterForRow(Strings.UI.FRONTEND.BOTTWEAKS.ROBOTS_CAT, searchUp);
            header.SearchFilter(searchUp);
        }
    }

    internal void SetRowsActive() {
        foreach (var header in this.headers) {
            header.SetRowsActive();
        }
    }

    internal void SpawnRows() {
        if (this.headers.Count < 1) {
            this.headers.Add(this.Create());
        }

        foreach (var header in this.headers) {
            header.SpawnRows(this.allResources);
        }
    }

    internal void UpdateContents() {
        foreach (var header in this.headers) {
            header.UpdateContents();
        }
    }
}
