// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Clothes {
    private class ClothingInfo {
        public string id;
        public string wornId;

        public ClothingInfo(string idIn, string wornIdIn) {
            this.id = idIn;
            this.wornId = wornIdIn;
        }
    }

    private List<ClothingInfo> clothes;

    public Clothes() {
        this.clothes = new List<ClothingInfo>();

        this.clothes.Add(new ClothingInfo(AtmoSuitConfig.ID, AtmoSuitConfig.WORN_ID));
        this.clothes.Add(new ClothingInfo(CustomClothingConfig.ID, "")); // primo garb
        this.clothes.Add(new ClothingInfo(FunkyVestConfig.ID, "")); // snazzy suit
        this.clothes.Add(new ClothingInfo(JetSuitConfig.ID, JetSuitConfig.WORN_ID));
        this.clothes.Add(new ClothingInfo(LeadSuitConfig.ID, LeadSuitConfig.WORN_ID));
        this.clothes.Add(new ClothingInfo(OxygenMaskConfig.ID, OxygenMaskConfig.WORN_ID));
        this.clothes.Add(new ClothingInfo(SleepClinicPajamas.ID, ""));
        this.clothes.Add(new ClothingInfo(WarmVestConfig.ID, ""));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ClothingInfo clothingInfo in this.clothes) {
            if (!Util.IsPrefabEnabledForCurrentDlc(clothingInfo.id)) {
                continue;
            }

            var clothingTag = TagManager.Create(clothingInfo.id);
            tags.Add(clothingTag);
            DiscoveredResources.Instance.Discover(clothingTag, GameTags.Clothes);
            if (clothingInfo.wornId == "") {
                continue;
            }

            // Worn variants are gated the same way via their prefab.
            if (!Util.IsPrefabEnabledForCurrentDlc(clothingInfo.wornId)) {
                continue;
            }
            var wornTag = TagManager.Create(clothingInfo.wornId);
            tags.Add(wornTag);
            DiscoveredResources.Instance.Discover(wornTag, GameTags.Clothes);
        }

        return tags;
    }
}
