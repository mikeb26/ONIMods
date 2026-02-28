// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Elements {
    private class ElementInfo {
        public SimHashes simHash;
        public Tag category;

        public ElementInfo(SimHashes simHashIn, Tag catIn) {
            this.simHash = simHashIn;
            this.category = catIn;
        }
    }

    private List<ElementInfo> elements;
    // cat solid.yaml | yq eval '.elements[] | [.elementId, .materialCategory // "MISSING"] | join(",")' > /tmp/solid.csv
    public Elements() {
        this.elements = new List<ElementInfo>();

        // solids
        this.elements.Add(new ElementInfo(SimHashes.Algae, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Aluminum, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.AluminumOre, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.BleachStone, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.BrineIce, GameTags.Liquifiable));
        // frozen brackene
        this.elements.Add(new ElementInfo(SimHashes.MilkIce, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.Carbon, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Ceramic, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Clay, GameTags.Farmable));
        this.elements.Add(new ElementInfo(SimHashes.Copper, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.CrushedIce, GameTags.Liquifiable));
        // crushed rock can be mined on Arid Planet in base game
        this.elements.Add(new ElementInfo(SimHashes.CrushedRock, GameTags.BuildableProcessed));
        // copper ore
        this.elements.Add(new ElementInfo(SimHashes.Cuprite, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.DepletedUranium, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Diamond, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Dirt, GameTags.Farmable));
        // polluted ice
        this.elements.Add(new ElementInfo(SimHashes.DirtyIce, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.EnrichedUranium, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Fertilizer, GameTags.Agriculture));
        // Pyrite can be mined on Gilded Asteroid in base game
        this.elements.Add(new ElementInfo(SimHashes.FoolsGold, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Fossil, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Fullerene, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Glass, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Gold, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.GoldAmalgam, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Granite, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Ice, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.IgneousRock, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Iron, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.IronOre, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Cobalt, GameTags.RefinedMetal));
        // cobalt ore
        this.elements.Add(new ElementInfo(SimHashes.Cobaltite, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Isoresin, GameTags.RareMaterials));
        // abyssalite
        this.elements.Add(new ElementInfo(SimHashes.Katairite, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Lead, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Lime, GameTags.ConsumableOre));
        // brackwax
        this.elements.Add(new ElementInfo(SimHashes.MilkFat, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.MaficRock, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Niobium, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Corium, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Obsidian, GameTags.BuildableRaw));
        // oxylite
        this.elements.Add(new ElementInfo(SimHashes.OxyRock, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Phosphorite, GameTags.Agriculture));
        // refined phorphorus
        this.elements.Add(new ElementInfo(SimHashes.Phosphorus, GameTags.ConsumableOre));
        // plastium
        this.elements.Add(new ElementInfo(SimHashes.HardPolypropylene, GameTags.ManufacturedMaterial));
        // plastic
        this.elements.Add(new ElementInfo(SimHashes.Polypropylene, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.RefinedCarbon, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Regolith, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.Rust, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Salt, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Sand, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.SandStone, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SedimentaryRock, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SlimeMold, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Snow, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCarbonDioxide, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidChlorine, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCrudeOil, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidHydrogen, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidMethane, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.SolidNaphtha, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidOxygen, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidPetroleum, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidResin, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.SolidSuperCoolant, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.SolidViscoGel, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Steel, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Sulfur, GameTags.Other));
        // insulation
        this.elements.Add(new ElementInfo(SimHashes.SuperInsulator, GameTags.ManufacturedMaterial));
        // thermium
        this.elements.Add(new ElementInfo(SimHashes.TempConductorSolid, GameTags.ManufacturedMaterial));
        // polluted dirt
        this.elements.Add(new ElementInfo(SimHashes.ToxicSand, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Tungsten, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.UraniumOre, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Wolframite, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.SolidEthanol, GameTags.Liquifiable));
        // polluted mud
        this.elements.Add(new ElementInfo(SimHashes.ToxicMud, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Mud, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Sucrose, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Graphite, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SolidNuclearWaste, GameTags.Other));

        // liquids
        this.elements.Add(new ElementInfo(SimHashes.Brine, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Chlorine, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.CrudeOil, GameTags.Liquid));
        // polluted water
        this.elements.Add(new ElementInfo(SimHashes.DirtyWater, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidCarbonDioxide, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidHydrogen, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidMethane, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidOxygen, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidPhosphorus, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidSulfur, GameTags.Liquid));
        // brackene
        this.elements.Add(new ElementInfo(SimHashes.Milk, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Magma, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenAluminum, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCarbon, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCopper, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGlass, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGold, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenIron, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCobalt, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenLead, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenNiobium, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSalt, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSteel, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenTungsten, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenUranium, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Naphtha, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.NuclearWaste, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Petroleum, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Resin, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SaltWater, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolant, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.ViscoGel, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Water, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Ethanol, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSucrose, GameTags.Liquid));

        // gasses
        this.elements.Add(new ElementInfo(SimHashes.AluminumGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonDioxide, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.ChlorineGas, GameTags.Unbreathable));
        // polluted oxygen
        this.elements.Add(new ElementInfo(SimHashes.ContaminatedOxygen, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.CopperGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.GoldGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Hydrogen, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.IronGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CobaltGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.LeadGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Methane, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.NiobiumGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Fallout, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Oxygen, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.PhosphorusGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.RockGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SaltGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SourGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Steam, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SteelGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SulfurGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolantGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.TungstenGas, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.EthanolGas, GameTags.Unbreathable));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ElementInfo elementInfo in this.elements) {
            var elementTag = GameTagExtensions.Create(elementInfo.simHash);
            // Element DLC gating is driven by the element table's dlcId and by which
            // elements are actually loaded for the current content. Don't attempt to
            // discover elements that aren't present.
            if (ElementLoader.GetElement(elementTag) == null) {
                continue;
            }
            tags.Add(elementTag);
            DiscoveredResources.Instance.Discover(elementTag, elementInfo.category);
        }

        return tags;
    }
}
