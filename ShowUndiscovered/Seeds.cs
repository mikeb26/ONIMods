// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Seeds {
    private class SeedInfo {
        public string id;
        public bool dlcOnly;
        public SeedInfo(string idIn, bool dlcOnlyIn) {
            this.id = idIn;
            this.dlcOnly = dlcOnlyIn;
        }
    }
    private bool baseGameOnly;

    private List<SeedInfo> seeds;

    public Seeds(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.seeds = new List<SeedInfo>();

        // thimble reed
        this.seeds.Add(new SeedInfo(BasicFabricMaterialPlantConfig.SEED_ID, false));
        // mealwood
        this.seeds.Add(new SeedInfo(BasicSingleHarvestPlantConfig.SEED_ID, false));
        // nosh sprout
        this.seeds.Add(new SeedInfo(BeanPlantConfig.SEED_ID, false));
        // buddy bud
        this.seeds.Add(new SeedInfo(BulbPlantConfig.SEED_ID, false));
        // joya
        this.seeds.Add(new SeedInfo(CactusPlantConfig.SEED_ID, false));
        // wheezewort
        this.seeds.Add(new SeedInfo(ColdBreatherConfig.SEED_ID, false));
        // sleet wheat
        this.seeds.Add(new SeedInfo(ColdWheatConfig.SEED_ID, false));
        // bliss burst
        this.seeds.Add(new SeedInfo(CylindricaConfig.SEED_ID, true));
        // spore child
        this.seeds.Add(new SeedInfo(EvilFlowerConfig.SEED_ID, false));
        // arbor acorn
        this.seeds.Add(new SeedInfo(ForestTreeConfig.SEED_ID, false));
        // gas gross
        this.seeds.Add(new SeedInfo(GasGrassConfig.SEED_ID, false));
        // Mirth Leaf
        this.seeds.Add(new SeedInfo(LeafyPlantConfig.SEED_ID, false));
        // mushroom
        this.seeds.Add(new SeedInfo(MushroomPlantConfig.SEED_ID, false));
        // oxyfern
        this.seeds.Add(new SeedInfo(OxyfernConfig.SEED_ID, false));
        // Bristle Blossom
        this.seeds.Add(new SeedInfo(PrickleFlowerConfig.SEED_ID, false));
        // Bluff Briar
        this.seeds.Add(new SeedInfo(PrickleGrassConfig.SEED_ID, false));
        // dasha saltvine
        this.seeds.Add(new SeedInfo(SaltPlantConfig.SEED_ID, false));
        // lettuce
        this.seeds.Add(new SeedInfo(SeaLettuceConfig.SEED_ID, false));
        // Pincha Pepperplant
        this.seeds.Add(new SeedInfo(SpiceVineConfig.SEED_ID, false));
        // bog bucket
        this.seeds.Add(new SeedInfo(SwampHarvestPlantConfig.SEED_ID, true));
        // balm lily
        this.seeds.Add(new SeedInfo(SwampLilyConfig.SEED_ID, false));
        // traquil toes
        this.seeds.Add(new SeedInfo(ToePlantConfig.SEED_ID, true));
        // Mellow Mallow
        this.seeds.Add(new SeedInfo(WineCupsConfig.SEED_ID, true));
        // grubfruit
        this.seeds.Add(new SeedInfo(WormPlantConfig.SEED_ID, true));
        // saturn critter trap
        this.seeds.Add(new SeedInfo(CritterTrapPlantConfig.ID + "Seed", true));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (SeedInfo seedInfo in this.seeds) {
            if (this.baseGameOnly && seedInfo.dlcOnly) {
                continue;
            }

            var seedTag = TagManager.Create(seedInfo.id);
            tags.Add(seedTag);
            DiscoveredResources.Instance.Discover(seedTag, GameTags.Seed);
        }

        return tags;
    }
}
