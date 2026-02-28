// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Foods {
    private Dictionary<Tag, Tag> dehyrdratedTags;

    public Foods() {
        this.dehyrdratedTags = new Dictionary<Tag, Tag>();

        this.dehyrdratedTags[SalsaConfig.ID] = DehydratedSalsaConfig.ID; // stuffed berry
        this.dehyrdratedTags[MushroomWrapConfig.ID] = DehydratedMushroomWrapConfig.ID;
        this.dehyrdratedTags[SurfAndTurfConfig.ID] = DehydratedSurfAndTurfConfig.ID;
        this.dehyrdratedTags[SpiceBreadConfig.ID] = DehydratedSpiceBreadConfig.ID; // pepper bread
        this.dehyrdratedTags[QuicheConfig.ID] = DehydratedQuicheConfig.ID;
        this.dehyrdratedTags[CurryConfig.ID] = DehydratedCurryConfig.ID;
        this.dehyrdratedTags[SpicyTofuConfig.ID] = DehydratedSpicyTofuConfig.ID;
        this.dehyrdratedTags[BurgerConfig.ID] = DehydratedFoodPackageConfig.ID; // frost burger
        this.dehyrdratedTags[BerryPieConfig.ID] = DehydratedBerryPieConfig.ID;
    }

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
            var catTag = GameTags.Edible;
            if (foodInfo.CaloriesPerUnit <= 0.0) {
                catTag = GameTags.CookingIngredient;
            }
            DiscoveredResources.Instance.Discover(tag, catTag);

            if (this.dehyrdratedTags.TryGetValue(tag, out Tag dehydTag) == false) {
                continue;
            }

            // Only discover dehydrated variants if the dehydrated prefab exists and is valid.
            if (!Util.IsPrefabEnabledForCurrentDlc(dehydTag)) {
                continue;
            }
            tags.Add(dehydTag);
            DiscoveredResources.Instance.Discover(dehydTag, GameTags.Dehydrated);
            
        }

        return tags;
    }
}
