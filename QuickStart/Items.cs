// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;

namespace QuickStart;

public class Items {
    public class ItemInfo {
        public string itemName;
        public float amount;
        public StartLevel minStartLevel;
        public bool dlcOnly;
        public bool isCritter;

        public ItemInfo(string itemNameIn, float amountIn, StartLevel startLevelIn,
                        bool dlcOnlyIn, bool isCritterIn) {
            this.itemName = itemNameIn;
            this.amount = amountIn;
            this.minStartLevel = startLevelIn;
            this.dlcOnly = dlcOnlyIn;
            this.isCritter = isCritterIn;
        }
    }

    private bool baseGameOnly;
    private Dictionary<string, ItemInfo> toSpawn;

    public Items(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.toSpawn = new Dictionary<string, ItemInfo>();

        // early elements
        var itemName = GameTagExtensions.Create(SimHashes.Sand).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 10000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Cuprite).ToString(); // copper ore
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.IgneousRock).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 20000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Granite).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 20000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.GoldAmalgam).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Carbon).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Algae).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Fossil).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);

        // mid elements
        itemName = GameTagExtensions.Create(SimHashes.Steel).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Polypropylene).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.CrudeOil).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 10000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Copper).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 10000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Petroleum).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.RefinedCarbon).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Diamond).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.AdvancedEarly, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Lead).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 20000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Ceramic).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Glass).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Gold).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Mid, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Sucrose).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 1000.0f, StartLevel.Mid, true, false);
        itemName = GameTagExtensions.Create(SimHashes.Sulfur).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Mid, true, false);
    
        // late elements
        itemName = GameTagExtensions.Create(SimHashes.EnrichedUranium).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Late, true, false);
        // thermium
        itemName = GameTagExtensions.Create(SimHashes.TempConductorSolid).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Tungsten).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Wolframite).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Obsidian).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Isoresin).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 2000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Niobium).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 1000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.SuperCoolant).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.Late, false, false);
        itemName = GameTagExtensions.Create(SimHashes.Fullerene).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 1000.0f, StartLevel.Late, false, false);

        // end elements
        // visco-gel spawns below its freezing temperature by default
        //itemName = GameTagExtensions.Create(SimHashes.ViscoGel).ToString();
        //this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.End, false, false);
        // insulation
        itemName = GameTagExtensions.Create(SimHashes.SuperInsulator).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.End, false, false);
        itemName = GameTagExtensions.Create(SimHashes.HardPolypropylene).ToString();
        this.toSpawn[itemName] = new ItemInfo(itemName, 5000.0f, StartLevel.End, false, false);

        // early seeds
        itemName = BasicFabricMaterialPlantConfig.SEED_ID; // thimble reed seed
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.AdvancedEarly, false, false);
        itemName = BasicSingleHarvestPlantConfig.SEED_ID; // mealwood seed
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.AdvancedEarly, false, false);
        itemName = ColdBreatherConfig.SEED_ID; // wheezewort seed
        this.toSpawn[itemName] = new ItemInfo(itemName, 5.0f, StartLevel.AdvancedEarly, false, false);
        itemName = ForestTreeConfig.SEED_ID; // arbor acron
        this.toSpawn[itemName] = new ItemInfo(itemName, 5.0f, StartLevel.AdvancedEarly, false, false);

        // mid seeds
        itemName = PrickleFlowerConfig.SEED_ID; // bristle blossom
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.Mid, false, false);
        itemName = ColdWheatConfig.SEED_ID; // sleet wheat
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.Mid, false, false);
        itemName = SpiceVineConfig.SEED_ID; // pincha peppernut
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.Mid, false, false);
        itemName = WormPlantConfig.SEED_ID; // grubfruit
        this.toSpawn[itemName] = new ItemInfo(itemName, 10.0f, StartLevel.Mid, true, false);

        // mid items
        itemName = BasicFabricConfig.ID; // reed fiber
        this.toSpawn[itemName] = new ItemInfo(itemName, 20.0f, StartLevel.Mid, false, false);

        // early critters
        itemName = HatchConfig.ID;
        this.toSpawn[itemName] = new ItemInfo(itemName, 5.0f, StartLevel.AdvancedEarly, false, true);
        itemName = HatchHardConfig.ID; // stone hatch
        this.toSpawn[itemName] = new ItemInfo(itemName, 5.0f, StartLevel.AdvancedEarly, false, true);

        // mid critters
        itemName = CrabConfig.ID + "Egg"; // pokeshell egg
        this.toSpawn[itemName] = new ItemInfo(itemName, 5.0f, StartLevel.Mid, false, true);
        itemName = SquirrelConfig.ID; // pip
        this.toSpawn[itemName] = new ItemInfo(itemName, 2.0f, StartLevel.Mid, false, true);
        itemName = DivergentBeetleConfig.ID; // sweetle
        this.toSpawn[itemName] = new ItemInfo(itemName, 2.0f, StartLevel.Mid, true, true);
    }

    public void Spawn(QuickStartOptions opts) {
        Util.Log("Spawning items for a {0} start.", opts.startLevel);

        var startWorld = ClusterManager.Instance.GetStartWorld();
        if (startWorld == null) {
            Util.Log("BUG: Could not get start world");
            return;
        }
        var startWorldId = startWorld.id;
        var telepad = GameUtil.GetTelepad(startWorldId);
        if (telepad == null) {
            Util.Log("BUG: Could not get start world's telepad");
            return;
        }
        var telepadCell = Grid.PosToCell(telepad);
        var telepadPos = Grid.CellToPosCBC(telepadCell, Grid.SceneLayer.Move);

        foreach (var kv in this.toSpawn) {
            if (this.baseGameOnly && kv.Value.dlcOnly) {
                continue;
            }
            if (kv.Value.minStartLevel > opts.startLevel) {
                continue;
            }
            if (kv.Value.isCritter && !opts.includeCritters) {
                continue;
            }

            var carePkg = new CarePackageInfo(kv.Key, kv.Value.amount, null);
            carePkg.Deliver(telepadPos);
        }
    }
}
