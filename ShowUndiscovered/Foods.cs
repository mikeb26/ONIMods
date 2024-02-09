// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Foods {
    private bool baseGameOnly;
    private Dictionary<Tag, Tag> dehyrdratedTags;

    public Foods(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
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
            if (this.baseGameOnly && foodInfo.DlcId != "") {
                continue;
            }
            // this food was never released
            if (foodInfo.Id == GammaMushConfig.ID) {
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
            tags.Add(dehydTag);
            DiscoveredResources.Instance.Discover(dehydTag, GameTags.Dehydrated);
            
        }

        return tags;
    }
}
