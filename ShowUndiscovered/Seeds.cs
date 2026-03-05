// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Seeds {
    public Seeds() {
    }

    public List<Tag> discoverAll() {
        var tags = new HashSet<Tag>();

        // Iterate the live prefab list so new DLC/patch seeds are automatically covered.
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

            if (!prefabId.HasTag(GameTags.Seed)) {
                continue;
            }

            var seedTag = prefabId.PrefabTag;
            if (tags.Add(seedTag)) {
                Mod.Instance.gameState.Discover(seedTag, GameTags.Seed);
            }
        }

        return new List<Tag>(tags);
    }
}
