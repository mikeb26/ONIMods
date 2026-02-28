// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Seeds {
    private class SeedInfo {
        public string id;
        public SeedInfo(string idIn) {
            this.id = idIn;
        }
    }
    private List<SeedInfo> seeds;

    public Seeds() {
        this.seeds = new List<SeedInfo>();

        // thimble reed
        this.seeds.Add(new SeedInfo(BasicFabricMaterialPlantConfig.SEED_ID));
        // mealwood
        this.seeds.Add(new SeedInfo(BasicSingleHarvestPlantConfig.SEED_ID));
        // nosh sprout
        this.seeds.Add(new SeedInfo(BeanPlantConfig.SEED_ID));
        // buddy bud
        this.seeds.Add(new SeedInfo(BulbPlantConfig.SEED_ID));
        // joya
        this.seeds.Add(new SeedInfo(CactusPlantConfig.SEED_ID));
        // wheezewort
        this.seeds.Add(new SeedInfo(ColdBreatherConfig.SEED_ID));
        // sleet wheat
        this.seeds.Add(new SeedInfo(ColdWheatConfig.SEED_ID));
        // bliss burst
        this.seeds.Add(new SeedInfo(CylindricaConfig.SEED_ID));
        // spore child
        this.seeds.Add(new SeedInfo(EvilFlowerConfig.SEED_ID));
        // arbor acorn
        this.seeds.Add(new SeedInfo(ForestTreeConfig.SEED_ID));
        // gas gross
        this.seeds.Add(new SeedInfo(GasGrassConfig.SEED_ID));
        // Mirth Leaf
        this.seeds.Add(new SeedInfo(LeafyPlantConfig.SEED_ID));
        // mushroom
        this.seeds.Add(new SeedInfo(MushroomPlantConfig.SEED_ID));
        // oxyfern
        this.seeds.Add(new SeedInfo(OxyfernConfig.SEED_ID));
        // Bristle Blossom
        this.seeds.Add(new SeedInfo(PrickleFlowerConfig.SEED_ID));
        // Bluff Briar
        this.seeds.Add(new SeedInfo(PrickleGrassConfig.SEED_ID));
        // dasha saltvine
        this.seeds.Add(new SeedInfo(SaltPlantConfig.SEED_ID));
        // lettuce
        this.seeds.Add(new SeedInfo(SeaLettuceConfig.SEED_ID));
        // Pincha Pepperplant
        this.seeds.Add(new SeedInfo(SpiceVineConfig.SEED_ID));
        // bog bucket
        this.seeds.Add(new SeedInfo(SwampHarvestPlantConfig.SEED_ID));
        // balm lily
        this.seeds.Add(new SeedInfo(SwampLilyConfig.SEED_ID));
        // traquil toes
        this.seeds.Add(new SeedInfo(ToePlantConfig.SEED_ID));
        // Mellow Mallow
        this.seeds.Add(new SeedInfo(WineCupsConfig.SEED_ID));
        // grubfruit
        this.seeds.Add(new SeedInfo(WormPlantConfig.SEED_ID));
        // saturn critter trap
        this.seeds.Add(new SeedInfo(CritterTrapPlantConfig.ID + "Seed"));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (SeedInfo seedInfo in this.seeds) {
            if (!Util.IsPrefabEnabledForCurrentDlc(seedInfo.id)) {
                continue;
            }

            var seedTag = TagManager.Create(seedInfo.id);
            tags.Add(seedTag);
            DiscoveredResources.Instance.Discover(seedTag, GameTags.Seed);
        }

        return tags;
    }
}
