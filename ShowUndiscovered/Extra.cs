// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Extras {
    private class ExtraInfo {
        public string id;
        public Tag category;
        public ExtraInfo(string idIn, Tag catIn) {
            this.id = idIn;
            this.category = catIn;
        }
    }
    private List<ExtraInfo> extras;

    public Extras() {
        this.extras = new List<ExtraInfo>();

        // reed fiber
        this.extras.Add(new ExtraInfo(BasicFabricConfig.ID, GameTags.IndustrialIngredient));
        // dlc data bank
        this.extras.Add(new ExtraInfo(OrbitalResearchDatabankConfig.ID, GameTags.IndustrialIngredient));
        // vanilla data bank
        this.extras.Add(new ExtraInfo(ResearchDatabankConfig.ID, GameTags.IndustrialIngredient));

        this.extras.Add(new ExtraInfo(TableSaltConfig.ID, GameTags.Other));
        // balm lily flower
        this.extras.Add(new ExtraInfo(SwampLilyFlowerConfig.ID, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(EggShellConfig.ID, GameTags.Organics));
        this.extras.Add(new ExtraInfo(RotPileConfig.ID, GameTags.Organics));
        // neural vacilator recharge
        this.extras.Add(new ExtraInfo(GeneShufflerRechargeConfig.ID, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(RailGunPayloadConfig.ID, GameTags.IgnoreMaterialCategory));
        // microchip
        this.extras.Add(new ExtraInfo(PowerStationToolsConfig.ID, GameTags.MiscPickupable));
        // micronutrient fertilizer
        this.extras.Add(new ExtraInfo(FarmStationToolsConfig.ID, GameTags.MiscPickupable));
        // lumber
        this.extras.Add(new ExtraInfo(WoodLogConfig.ID, GameTags.IndustrialIngredient));

        // blast shot
        this.extras.Add(new ExtraInfo(MissileBasicConfig.ID, GameTags.IndustrialProduct));

        // pokeshell molts
        this.extras.Add(new ExtraInfo(CrabShellConfig.ID, GameTags.IndustrialIngredient));
        this.extras.Add(new ExtraInfo(CrabWoodShellConfig.ID, GameTags.IndustrialIngredient));
        
        // @todo dream journal?
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ExtraInfo extraInfo in this.extras) {
            if (!Util.IsPrefabEnabledForCurrentDlc(extraInfo.id)) {
                continue;
            }

            var extraTag = TagManager.Create(extraInfo.id);
            tags.Add(extraTag);
            DiscoveredResources.Instance.Discover(extraTag, extraInfo.category);
        }

        return tags;
    }
}
