// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Elements {
    private class ElementInfo {
        public SimHashes simHash;
        public bool dlcOnly;
        public Tag category;

        public ElementInfo(SimHashes simHashIn, bool dlcOnlyIn, Tag catIn) {
            this.simHash = simHashIn;
            this.dlcOnly = dlcOnlyIn;
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
        this.elements.Add(new ElementInfo(SimHashes.Algae, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Aluminum, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.AluminumOre, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.BleachStone, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.BrineIce, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.Carbon, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Ceramic, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Clay, false, GameTags.Farmable));
        this.elements.Add(new ElementInfo(SimHashes.Copper, false, GameTags.RefinedMetal));
        // copper ore
        this.elements.Add(new ElementInfo(SimHashes.Cuprite, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.DepletedUranium, true, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Diamond, false, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Dirt, false, GameTags.Farmable));
        // polluted ice
        this.elements.Add(new ElementInfo(SimHashes.DirtyIce, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.EnrichedUranium, true, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Fertilizer, false, GameTags.Agriculture));
        this.elements.Add(new ElementInfo(SimHashes.Fossil, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Fullerene, false, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Glass, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Gold, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.GoldAmalgam, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Granite, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Ice, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.IgneousRock, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Iron, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.IronOre, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Cobalt, true, GameTags.RefinedMetal));
        // cobalt ore
        this.elements.Add(new ElementInfo(SimHashes.Cobaltite, true, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Isoresin, false, GameTags.RareMaterials));
        // abyssalite
        this.elements.Add(new ElementInfo(SimHashes.Katairite, false, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Lead, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.Lime, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.MaficRock, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.Niobium, false, GameTags.RareMaterials));
        this.elements.Add(new ElementInfo(SimHashes.Corium, true, GameTags.Other));
        this.elements.Add(new ElementInfo(SimHashes.Obsidian, false, GameTags.BuildableRaw));
        // oxylite
        this.elements.Add(new ElementInfo(SimHashes.OxyRock, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Phosphorite, false, GameTags.Agriculture));
        // refined phorphorus
        this.elements.Add(new ElementInfo(SimHashes.Phosphorus, false, GameTags.ConsumableOre));
        // plastic
        this.elements.Add(new ElementInfo(SimHashes.Polypropylene, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.RefinedCarbon, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Regolith, false, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.Rust, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Salt, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Sand, false, GameTags.Filter));
        this.elements.Add(new ElementInfo(SimHashes.SandStone, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SedimentaryRock, false, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SlimeMold, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Snow, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCarbonDioxide, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidChlorine, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidCrudeOil, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidHydrogen, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidMethane, false, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.SolidNaphtha, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidOxygen, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidPetroleum, false, GameTags.Liquifiable));
        this.elements.Add(new ElementInfo(SimHashes.SolidResin, true, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.SolidSuperCoolant, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.SolidViscoGel, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Steel, false, GameTags.ManufacturedMaterial));
        this.elements.Add(new ElementInfo(SimHashes.Sulfur, false, GameTags.Other));
        // insulation
        this.elements.Add(new ElementInfo(SimHashes.SuperInsulator, false, GameTags.ManufacturedMaterial));
        // thermium
        this.elements.Add(new ElementInfo(SimHashes.TempConductorSolid, false, GameTags.ManufacturedMaterial));
        // polluted dirt
        this.elements.Add(new ElementInfo(SimHashes.ToxicSand, false, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Tungsten, false, GameTags.RefinedMetal));
        this.elements.Add(new ElementInfo(SimHashes.UraniumOre, true, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.Wolframite, false, GameTags.Metal));
        this.elements.Add(new ElementInfo(SimHashes.SolidEthanol, false, GameTags.Liquifiable));
        // polluted mud
        this.elements.Add(new ElementInfo(SimHashes.ToxicMud, true, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Mud, true, GameTags.Organics));
        this.elements.Add(new ElementInfo(SimHashes.Sucrose, true, GameTags.ConsumableOre));
        this.elements.Add(new ElementInfo(SimHashes.Graphite, true, GameTags.BuildableRaw));
        this.elements.Add(new ElementInfo(SimHashes.SolidNuclearWaste, true, GameTags.Other));

        // liquids
        this.elements.Add(new ElementInfo(SimHashes.Brine, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Chlorine, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.CrudeOil, false, GameTags.Liquid));
        // polluted water
        this.elements.Add(new ElementInfo(SimHashes.DirtyWater, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidCarbonDioxide, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidHydrogen, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidMethane, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidOxygen, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidPhosphorus, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.LiquidSulfur, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Magma, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenAluminum, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCarbon, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCopper, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGlass, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenGold, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenIron, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenCobalt, true, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenLead, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenNiobium, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSalt, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSteel, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenTungsten, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenUranium, true, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Naphtha, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.NuclearWaste, true, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Petroleum, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Resin, true, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SaltWater, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolant, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.ViscoGel, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Water, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.Ethanol, false, GameTags.Liquid));
        this.elements.Add(new ElementInfo(SimHashes.MoltenSucrose, true, GameTags.Liquid));

        // gasses
        this.elements.Add(new ElementInfo(SimHashes.AluminumGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonDioxide, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CarbonGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.ChlorineGas, false, GameTags.Unbreathable));
        // polluted oxygen
        this.elements.Add(new ElementInfo(SimHashes.ContaminatedOxygen, false, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.CopperGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.GoldGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Hydrogen, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.IronGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.CobaltGas, true, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.LeadGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Methane, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.NiobiumGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Fallout, true, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Oxygen, false, GameTags.Breathable));
        this.elements.Add(new ElementInfo(SimHashes.PhosphorusGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.RockGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SaltGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SourGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.Steam, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SteelGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SulfurGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.SuperCoolantGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.TungstenGas, false, GameTags.Unbreathable));
        this.elements.Add(new ElementInfo(SimHashes.EthanolGas, false, GameTags.Unbreathable));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (ElementInfo elementInfo in this.elements) {
            if (this.baseGameOnly && elementInfo.dlcOnly) {
                continue;
            }

            var elementTag = GameTagExtensions.Create(elementInfo.simHash);
            tags.Add(elementTag);
            DiscoveredResources.Instance.Discover(elementTag, elementInfo.category);
        }

        return tags;
    }
}
