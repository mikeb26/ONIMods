// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Clothes {
    private class ClothingInfo {
        public string id;
        public string wornId;
        public bool dlcOnly;

        public ClothingInfo(string idIn, string wornIdIn, bool dlcOnlyIn) {
            this.id = idIn;
            this.wornId = wornIdIn;
            this.dlcOnly = dlcOnlyIn;
        }
    }
    private bool baseGameOnly;

    private List<ClothingInfo> clothes;

    public Clothes(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.clothes = new List<ClothingInfo>();

        this.clothes.Add(new ClothingInfo(AtmoSuitConfig.ID, AtmoSuitConfig.WORN_ID, false));
        this.clothes.Add(new ClothingInfo(CoolVestConfig.ID, "", false));
        this.clothes.Add(new ClothingInfo(CustomClothingConfig.ID, "", false)); // primo garb
        this.clothes.Add(new ClothingInfo(FunkyVestConfig.ID, "", false)); // snazzy suit
        this.clothes.Add(new ClothingInfo(JetSuitConfig.ID, JetSuitConfig.WORN_ID, false));
        this.clothes.Add(new ClothingInfo(LeadSuitConfig.ID, LeadSuitConfig.WORN_ID, true));
        this.clothes.Add(new ClothingInfo(OxygenMaskConfig.ID, OxygenMaskConfig.WORN_ID, false));
        this.clothes.Add(new ClothingInfo(SleepClinicPajamas.ID, "", false));
        this.clothes.Add(new ClothingInfo(WarmVestConfig.ID, "", false));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ClothingInfo clothingInfo in this.clothes) {
            if (this.baseGameOnly && clothingInfo.dlcOnly) {
                continue;
            }

            var clothingTag = TagManager.Create(clothingInfo.id);
            tags.Add(clothingTag);
            DiscoveredResources.Instance.Discover(clothingTag, GameTags.Clothes);
            if (clothingInfo.wornId == "") {
                continue;
            }
            var wornTag = TagManager.Create(clothingInfo.wornId);
            tags.Add(wornTag);
            DiscoveredResources.Instance.Discover(wornTag, GameTags.Clothes);
        }

        return tags;
    }
}
