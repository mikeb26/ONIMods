// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Extras {
    private class ExtraInfo {
        public string id;
        public bool dlcOnly;
        public bool excludeFromDlc;
        public Tag category;
        public ExtraInfo(string idIn, bool dlcOnlyIn, bool excludeIn, Tag catIn) {
            this.id = idIn;
            this.dlcOnly = dlcOnlyIn;
            this.excludeFromDlc = excludeIn;
            this.category = catIn;
        }
    }
    private bool baseGameOnly;

    private List<ExtraInfo> extras;

    public Extras(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.extras = new List<ExtraInfo>();

        // reed fiber
        this.extras.Add(new ExtraInfo(BasicFabricConfig.ID, false, false, GameTags.IndustrialIngredient));
        // dlc data bank
        this.extras.Add(new ExtraInfo(OrbitalResearchDatabankConfig.ID, true, false, GameTags.IndustrialIngredient));
        // vanilla data bank
        this.extras.Add(new ExtraInfo(ResearchDatabankConfig.ID, false, true, GameTags.IndustrialIngredient));

        this.extras.Add(new ExtraInfo(TableSaltConfig.ID, false, false, GameTags.Other));
        // balm lily flower
        this.extras.Add(new ExtraInfo(SwampLilyFlowerConfig.ID, false, false, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(EggShellConfig.ID, false, false, GameTags.Organics));
        this.extras.Add(new ExtraInfo(RotPileConfig.ID, false, false, GameTags.Organics));
        // neural vacilator recharge
        this.extras.Add(new ExtraInfo(GeneShufflerRechargeConfig.ID, false, false, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(RailGunPayloadConfig.ID, true, false, GameTags.IgnoreMaterialCategory));
        // microchip
        this.extras.Add(new ExtraInfo(PowerStationToolsConfig.ID, false, false, GameTags.MiscPickupable));
        // micronutrient fertilizer
        this.extras.Add(new ExtraInfo(FarmStationToolsConfig.ID, false, false, GameTags.MiscPickupable));
        // lumber
        this.extras.Add(new ExtraInfo(WoodLogConfig.ID, false, false, GameTags.IndustrialIngredient));

        // blast shot
        this.extras.Add(new ExtraInfo(MissileBasicConfig.ID, false, false, GameTags.IndustrialProduct));

        // pokeshell molts
        this.extras.Add(new ExtraInfo(CrabShellConfig.ID, false, false, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(BabyCrabShellConfig.ID, false, false, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(CrabWoodShellConfig.ID, false, false, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(BabyCrabWoodShellConfig.ID, false, false, GameTags.IndustrialIngredient));
        
        // @todo dream journal?
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ExtraInfo extraInfo in this.extras) {
            if (this.baseGameOnly && extraInfo.dlcOnly) {
                continue;
            }
            if (!this.baseGameOnly && extraInfo.excludeFromDlc) {
                continue;
            }

            var extraTag = TagManager.Create(extraInfo.id);
            tags.Add(extraTag);
            DiscoveredResources.Instance.Discover(extraTag, extraInfo.category);
        }

        return tags;
    }
}
