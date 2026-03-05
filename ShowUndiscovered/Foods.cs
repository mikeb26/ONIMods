// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Foods {
    public Foods() {
    }

    // Avoid log spam: only log categorization fallbacks once per food id per session.
    private static readonly HashSet<Tag> LoggedFoodCategoryFallback = new HashSet<Tag>();

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes()) {
            // this food was never released
            if (foodInfo.Id == GammaMushConfig.ID) {
                continue;
            }

            // Even though GetAllFoodTypes() is already filtered by DLC, ensure the backing
            // entity prefab is actually present and valid for the current save's DLC set.
            // (Some entries may exist in data tables while their prefabs are gated.)
            if (!Util.IsPrefabEnabledForCurrentDlc(foodInfo.Id)) {
                continue;
            }

            var tag = TagManager.Create(foodInfo.Id);
            tags.Add(tag);

            // Categorize based on the prefab's tags rather than hardcoded dehydrated mappings.
            // This allows new DLC/patch foods (and their dehydrated variants) to be handled
            // automatically.
            var prefab = Assets.TryGetPrefab(tag);
            var prefabId = prefab != null ? prefab.GetComponent<KPrefabID>() : null;

            Tag catTag;
            if (prefabId != null && prefabId.HasTag(GameTags.Dehydrated)) {
                catTag = GameTags.Dehydrated;
            } else if (prefabId == null) {
                // We filtered via IsPrefabEnabledForCurrentDlc, so this should be rare.
                // Fall back to calories-based categorization and log once.
                if (LoggedFoodCategoryFallback.Add(tag)) {
                    Util.Log("Foods: {0} has no prefab/KPrefabID; categorizing from calories (Cal={1})",
                        tag, foodInfo.CaloriesPerUnit);
                }
                catTag = foodInfo.CaloriesPerUnit <= 0.0 ? GameTags.CookingIngredient : GameTags.Edible;
            } else if (foodInfo.CaloriesPerUnit <= 0.0) {
                catTag = GameTags.CookingIngredient;
            } else {
                catTag = GameTags.Edible;
            }

	    Mod.Instance.gameState.Discover(tag, catTag);
        }

        return tags;
    }
}
