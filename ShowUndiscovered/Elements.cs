// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Elements {
    private class ElementInfo {
        public SimHashes simHash;
        public bool dlcOnly;
        public bool excludeFromDlc;
        public Tag category;

        public ElementInfo(SimHashes simHashIn, bool dlcOnlyIn, bool excludeDlcIn, Tag catIn) {
            this.simHash = simHashIn;
            this.dlcOnly = dlcOnlyIn;
            this.excludeFromDlc = excludeDlcIn;
            this.category = catIn;
        }
    }
    private bool baseGameOnly;

    private List<ElementInfo> elements;
    // cat solid.yaml | yq eval '.elements[] | [.elementId, .materialCategory // "MISSING"] | join(",")' > /tmp/solid.csv
    public Elements(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.elements = new List<ElementInfo>();

        // solids
        this.elements.Add(new ElementInfo(SimHashes.Algae, false, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Aluminum, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.AluminumOre, false, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.BleachStone, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.BrineIce, false, false, GameTags.Liquifiable));
        // frozen brackene
        this.elements.Add(new ElementInfo(SimHashes.MilkIce, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.Carbon, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Ceramic, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Clay, false, false, GameTags.Farmable));
        this.elements.Add(new ElementInfo(SimHashes.Copper, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.CrushedIce, false, false, GameTags.Liquifiable));
        // crushed rock can be mined on Arid Planet in base game
        this.elements.Add(new ElementInfo(SimHashes.CrushedRock, false, true, GameTags.BuildableProcessed));
        // copper ore
        this.elements.Add(new ElementInfo(SimHashes.Cuprite, false, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.DepletedUranium, true, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Diamond, false, false, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Dirt, false, false, GameTags.Farmable));
        // polluted ice
        this.elements.Add(new ElementInfo(SimHashes.DirtyIce, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.EnrichedUranium, true, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Fertilizer, false, false, GameTags.Agriculture));
        // Pyrite can be mined on Gilded Asteroid in base game
        this.elements.Add(new ElementInfo(SimHashes.FoolsGold, false, true, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Fossil, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Fullerene, false, false, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Glass, false, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Gold, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.GoldAmalgam, false, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Granite, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Ice, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.IgneousRock, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Iron, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.IronOre, false, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Cobalt, true, false, GameTags.RefinedMetal));
        // cobalt ore
        this.elements.Add(new ElementInfo(SimHashes.Cobaltite, true, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Isoresin, false, false, GameTags.RareMaterials));
        // abyssalite
        this.elements.Add(new ElementInfo(SimHashes.Katairite, false, false, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Lead, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Lime, false, false, GameTags.ConsumableOre));
        // brackwax
        this.elements.Add(new ElementInfo(SimHashes.MilkFat, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.MaficRock, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Niobium, false, false, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Corium, true, false, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Obsidian, false, false, GameTags.BuildableRaw));
        // oxylite
        this.elements.Add(new ElementInfo(SimHashes.OxyRock, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Phosphorite, false, false, GameTags.Agriculture));
        // refined phorphorus
        this.elements.Add(new ElementInfo(SimHashes.Phosphorus, false, false, GameTags.ConsumableOre));
        // plastium
        this.elements.Add(new ElementInfo(SimHashes.HardPolypropylene, false, false, GameTags.ManufacturedMaterial));
        // plastic
        this.elements.Add(new ElementInfo(SimHashes.Polypropylene, false, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.RefinedCarbon, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Regolith, false, false, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.Rust, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Salt, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Sand, false, false, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.SandStone, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SedimentaryRock, false, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SlimeMold, false, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Snow, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCarbonDioxide, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidChlorine, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCrudeOil, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidHydrogen, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidMethane, false, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.SolidNaphtha, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidOxygen, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidPetroleum, false, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidResin, true, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.SolidSuperCoolant, false, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.SolidViscoGel, false, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Steel, false, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Sulfur, false, false, GameTags.Other));
        // insulation
        this.elements.Add(new ElementInfo(SimHashes.SuperInsulator, false, false, GameTags.ManufacturedMaterial));
        // thermium
        this.elements.Add(new ElementInfo(SimHashes.TempConductorSolid, false, false, GameTags.ManufacturedMaterial));
        // polluted dirt
        this.elements.Add(new ElementInfo(SimHashes.ToxicSand, false, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Tungsten, false, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.UraniumOre, true, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Wolframite, false, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.SolidEthanol, false, false, GameTags.Liquifiable));
        // polluted mud
        this.elements.Add(new ElementInfo(SimHashes.ToxicMud, true, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Mud, true, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Sucrose, true, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Graphite, true, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SolidNuclearWaste, true, false, GameTags.Other));

        // liquids
        this.elements.Add(new ElementInfo(SimHashes.Brine, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Chlorine, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.CrudeOil, false, false, GameTags.Liquid));
        // polluted water
        this.elements.Add(new ElementInfo(SimHashes.DirtyWater, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidCarbonDioxide, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidHydrogen, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidMethane, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidOxygen, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidPhosphorus, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidSulfur, false, false, GameTags.Liquid));
        // brackene
        this.elements.Add(new ElementInfo(SimHashes.Milk, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Magma, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenAluminum, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCarbon, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCopper, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGlass, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGold, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenIron, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCobalt, true, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenLead, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenNiobium, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSalt, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSteel, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenTungsten, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenUranium, true, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Naphtha, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.NuclearWaste, true, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Petroleum, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Resin, true, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SaltWater, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolant, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.ViscoGel, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Water, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Ethanol, false, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSucrose, true, false, GameTags.Liquid));

        // gasses
        this.elements.Add(new ElementInfo(SimHashes.AluminumGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonDioxide, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.ChlorineGas, false, false, GameTags.Unbreathable));
        // polluted oxygen
        this.elements.Add(new ElementInfo(SimHashes.ContaminatedOxygen, false, false, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.CopperGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.GoldGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Hydrogen, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.IronGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CobaltGas, true, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.LeadGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Methane, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.NiobiumGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Fallout, true, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Oxygen, false, false, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.PhosphorusGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.RockGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SaltGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SourGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Steam, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SteelGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SulfurGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolantGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.TungstenGas, false, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.EthanolGas, false, false, GameTags.Unbreathable));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ElementInfo elementInfo in this.elements) {
            if (this.baseGameOnly && elementInfo.dlcOnly) {
                continue;
            }
            if (!this.baseGameOnly && elementInfo.excludeFromDlc) {
                continue;
            }

            var elementTag = GameTagExtensions.Create(elementInfo.simHash);
            tags.Add(elementTag);
            DiscoveredResources.Instance.Discover(elementTag, elementInfo.category);
        }

        return tags;
    }
}
