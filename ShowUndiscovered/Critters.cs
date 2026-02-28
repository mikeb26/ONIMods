// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Critters {
    private class CritterInfo {
        public string id;
        public bool hasBaby;
        public bool hasEgg;
        public Tag category;
        public CritterInfo(string idIn, bool hasBabyIn, bool hasEggIn, Tag catIn) {
            this.id = idIn;
            this.hasBaby = hasBabyIn;
            this.hasEgg = hasEggIn;
            this.category = catIn;
        }
    }

    private List<CritterInfo> critters;

    public Critters() {
        this.critters = new List<CritterInfo>();

        // pokeshells
        this.critters.Add(new CritterInfo(CrabConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(CrabFreshWaterConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(CrabWoodConfig.ID, true, true, GameTags.BagableCreature));

        // dreckos
        this.critters.Add(new CritterInfo(DreckoConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(DreckoPlasticConfig.ID, true, true, GameTags.BagableCreature));

        // hatches
        this.critters.Add(new CritterInfo(HatchConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(HatchHardConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(HatchMetalConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(HatchVeggieConfig.ID, true, true, GameTags.BagableCreature));

        // shine bugs
        this.critters.Add(new CritterInfo(LightBugBlackConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugBlueConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugCrystalConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugOrangeConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugPinkConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(LightBugPurpleConfig.ID, true, true, GameTags.BagableCreature));

        // voles
        this.critters.Add(new CritterInfo(MoleConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(MoleDelicacyConfig.ID, true, true, GameTags.BagableCreature));

        // slicksters
        this.critters.Add(new CritterInfo(OilFloaterConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(OilFloaterDecorConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(OilFloaterHighTempConfig.ID, true, true, GameTags.BagableCreature));

        // pacu / gulp fish
        this.critters.Add(new CritterInfo(PacuCleanerConfig.ID, true, true, GameTags.SwimmingCreature));
        this.critters.Add(new CritterInfo(PacuConfig.ID, true, true, GameTags.SwimmingCreature));
        this.critters.Add(new CritterInfo(PacuTropicalConfig.ID, true, true, GameTags.SwimmingCreature));

        // pufts
        this.critters.Add(new CritterInfo(PuftAlphaConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(PuftBleachstoneConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(PuftConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(PuftOxyliteConfig.ID, true, true, GameTags.BagableCreature));

        // pips
        this.critters.Add(new CritterInfo(SquirrelConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(SquirrelHugConfig.ID, true, true, GameTags.BagableCreature));

        // gassy moo
        this.critters.Add(new CritterInfo(MooConfig.ID, false, false, GameTags.BagableCreature));

        // morb
        this.critters.Add(new CritterInfo(GlomConfig.ID, false, false, GameTags.BagableCreature));

        // divergents
        this.critters.Add(new CritterInfo(DivergentBeetleConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(DivergentWormConfig.ID, true, true, GameTags.BagableCreature));

        // plug slugs
        this.critters.Add(new CritterInfo(StaterpillarConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(StaterpillarGasConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(StaterpillarLiquidConfig.ID, true, true, GameTags.BagableCreature));
        this.critters.Add(new CritterInfo(StaterpillarLiquidConfig.ID, true, true, GameTags.BagableCreature));

        // beeta
        this.critters.Add(new CritterInfo(BeeConfig.ID, true, false, GameTags.BagableCreature));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (CritterInfo critterInfo in this.critters) {
            if (!Util.IsPrefabEnabledForCurrentDlc(critterInfo.id)) {
                continue;
            }

            var creatureTag = TagManager.Create(critterInfo.id);
            tags.Add(creatureTag);
            DiscoveredResources.Instance.Discover(creatureTag, critterInfo.category);
            if (!critterInfo.hasBaby) {
                continue;
            }
            var babyId = critterInfo.id + "Baby";
            if (!Util.IsPrefabEnabledForCurrentDlc(babyId)) {
                continue;
            }
            var babyTag = TagManager.Create(babyId);
            tags.Add(babyTag);
            DiscoveredResources.Instance.Discover(babyTag, critterInfo.category);
            if (!critterInfo.hasEgg) {
                continue;
            }
            var eggId = critterInfo.id + "Egg";
            if (!Util.IsPrefabEnabledForCurrentDlc(eggId)) {
                continue;
            }
            var eggTag = TagManager.Create(eggId);
            tags.Add(eggTag);
            DiscoveredResources.Instance.Discover(eggTag, GameTags.Egg);
        }

        return tags;
    }
}

