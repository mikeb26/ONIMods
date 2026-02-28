// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Critters {
    public Critters() {
    }

    // Keep this ordered (HashSet iteration order is undefined).
    private static readonly Tag[] AllowedCritterCategories = new Tag[] {
        GameTags.SwimmingCreature,
        GameTags.BagableCreature,
    };

    // Base game critter discover list uses the resource categories (BagableCreature / SwimmingCreature).
    // Determine the category from prefab tags.
    private static Tag GetCritterCategory(KPrefabID prefabId) {
        foreach (var cat in AllowedCritterCategories) {
            if (prefabId.HasTag(cat)) {
                return cat;
            }
        }
        return Tag.Invalid;
    }

    public List<Tag> discoverAll() {
        var tags = new HashSet<Tag>();

        // Iterate the live prefab list so new DLC/patch critters are automatically covered.
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

            // Eggs: discover separately under the egg category.
            if (prefabId.HasTag(GameTags.Egg)) {
                var eggTag = prefabId.PrefabTag;
                if (tags.Add(eggTag)) {
                    DiscoveredResources.Instance.Discover(eggTag, GameTags.Egg);
                }
                continue;
            }

            var critterCategory = GetCritterCategory(prefabId);
            if (critterCategory == Tag.Invalid) {
                continue;
            }

            var critterTag = prefabId.PrefabTag;
            if (tags.Add(critterTag)) {
                DiscoveredResources.Instance.Discover(critterTag, critterCategory);
            }
        }

        return new List<Tag>(tags);
    }
}

