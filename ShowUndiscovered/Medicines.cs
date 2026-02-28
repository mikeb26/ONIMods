// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Medicines {
    public Medicines() {
    }

    public List<Tag> discoverAll() {
        var tags = new HashSet<Tag>();

        // Iterate the live prefab list so new DLC/patch medicines are automatically covered.
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

            if (!prefabId.HasTag(GameTags.Medicine)) {
                continue;
            }

            var tag = prefabId.PrefabTag;
            if (tags.Add(tag)) {
                DiscoveredResources.Instance.Discover(tag, GameTags.Medicine);
            }
        }

        return new List<Tag>(tags);
    }
}
