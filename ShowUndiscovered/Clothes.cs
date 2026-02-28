// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Clothes {
    public Clothes() {
    }

    public List<Tag> discoverAll() {
        var tags = new HashSet<Tag>();

        // Iterate the live prefab list so new DLC/patch clothing items are automatically
        // covered without hardcoding IDs.
        foreach (var prefab in Assets.Prefabs) {
            if (prefab == null) {
                continue;
            }

            var prefabId = prefab.GetComponent<KPrefabID>();
            if (prefabId == null) {
                continue;
            }

            if (!Game.IsCorrectDlcActiveForCurrentSave(prefabId)) {
                continue;
            }

            // Clothing items (including suits/masks) are tagged with GameTags.Clothes.
            if (!prefabId.HasTag(GameTags.Clothes)) {
                continue;
            }

            var tag = prefabId.PrefabTag;
            if (tags.Add(tag)) {
                DiscoveredResources.Instance.Discover(tag, GameTags.Clothes);
            }
        }

        return new List<Tag>(tags);
    }
}
