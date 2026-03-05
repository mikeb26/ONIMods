// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Extras {
    public Extras() {
    }

    // Avoid log spam: only log category corrections once per tag per session.
    private static readonly HashSet<Tag> LoggedCategoryCorrections = new HashSet<Tag>();

    private static readonly HashSet<Tag> ExtraCategories = new HashSet<Tag>() {
        GameTags.IndustrialIngredient,
        GameTags.IndustrialProduct,
        GameTags.MiscPickupable,
    };

    private static readonly HashSet<Tag> ExcludedTags = new HashSet<Tag>() {
        // Already handled by other discoverers.
        GameTags.Edible,
        GameTags.CookingIngredient,
        GameTags.Dehydrated,
        GameTags.Egg,
        GameTags.Seed,
        GameTags.Medicine,
        GameTags.Clothes,
        GameTags.BagableCreature,
        GameTags.SwimmingCreature,
	HeatCubeConfig.ID,
    };

    private static bool IsExtraCategory(Tag categoryTag) {
        return ExtraCategories.Contains(categoryTag);
    }

    public List<Tag> discoverAll() {
        var tags = new HashSet<Tag>();

        // Discover "extra" pickupables (non-element, non-food, etc) without hardcoding IDs.
        // Keep the category list tight to avoid polluting the resource list.
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

            // Only consider pickupable items (avoid buildings, creatures, etc).
            if (prefab.GetComponent<Pickupable>() == null) {
                continue;
            }

            // Skip items that belong to other top-level discovery categories.
            bool excluded = false;
            foreach (var excludedTag in ExcludedTags) {
                if (prefabId.HasTag(excludedTag)) {
                    excluded = true;
                    break;
                }
            }
            if (excluded) {
                continue;
            }

            // Skip boosters
            if (prefabId.PrefabTag.ToString().StartsWith("Booster_", StringComparison.OrdinalIgnoreCase)) {
                continue;
            }

            // Skip artifacts and keepsakes.
            // (These are collectible decor items and clutter the resources list.)
            if (prefabId.PrefabTag.ToString().StartsWith("artifact_", StringComparison.OrdinalIgnoreCase)) {
                continue;
            }
            if (prefabId.PrefabTag.ToString().StartsWith("keepsake_", StringComparison.OrdinalIgnoreCase)) {
                continue;
            }

            // Prefer the category stored in PrimaryElement (this is what the resource screen uses).
            // ONI API note: different game versions expose this as either a field or property
            // with different names (ElementCategory / elementCategory). Use reflection so we
            // can build against whichever is present.
            Tag categoryFromPrimary = Tag.Invalid;
            var primary = prefabId.GetComponent<PrimaryElement>();
            if (primary != null) {
                var t = primary.GetType();
                var field = t.GetField("ElementCategory") ?? t.GetField("elementCategory");
                if (field != null && field.FieldType == typeof(Tag)) {
                    categoryFromPrimary = (Tag)field.GetValue(primary);
                } else {
                    var prop = t.GetProperty("ElementCategory") ?? t.GetProperty("elementCategory");
                    if (prop != null && prop.PropertyType == typeof(Tag)) {
                        categoryFromPrimary = (Tag)prop.GetValue(primary, null);
                    }
                }
            }

            // Fallback: sometimes category is only expressed as tags on the prefab id.
            Tag categoryFromTags = Tag.Invalid;
            foreach (var cat in ExtraCategories) {
                if (prefabId.HasTag(cat)) {
                    categoryFromTags = cat;
                    break;
                }
            }

            // Choose a category. Prefer PrimaryElement when available.
            Tag categoryTag = categoryFromPrimary != Tag.Invalid ? categoryFromPrimary : categoryFromTags;

            // If PrimaryElement is missing/invalid but the prefab has a known category tag,
            // log that we're using the fallback category.
            if (categoryFromPrimary == Tag.Invalid && categoryFromTags != Tag.Invalid) {
                Util.Log("Extras: {0} missing PrimaryElement category; using tag-derived category '{1}'",
                    prefabId.PrefabTag, categoryFromTags);
            }

            if (!IsExtraCategory(categoryTag)) {
                continue;
            }

            var extraTag = prefabId.PrefabTag;
            if (tags.Add(extraTag)) {
                Mod.Instance.gameState.Discover(extraTag, categoryTag);
            }
        }

        return new List<Tag>(tags);
    }
}
