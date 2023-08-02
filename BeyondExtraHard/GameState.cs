// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System;
using Klei.CustomSettings;
using Klei.AI;

namespace BeyondExtraHard;

public class GameState
{
    public CustomGameSettings cgs {get; set;}

    private struct GeneratorInfo {
        public string ID;
        public float baseWatts;

        public GeneratorInfo(string IDin, float baseWattsIn) {
            this.ID = IDin;
            this.baseWatts = baseWattsIn;
        }
    }

    private BEHOptions opts;
    private Toggles toggles;
    private Dictionary<PowerSetting, float> powerFactors;
    private Dictionary<GeneratorType, GeneratorInfo> powerInfos;
    private Dictionary<OxygenSetting, float> oxyIncrs;
    private Dictionary<HealthSetting, float> healthIncrs;
    private Dictionary<MassSetting, float> massFactors;
    private OxygenSetting oxySetting;
    private HealthSetting healthSetting;
    private PowerSetting powerSetting;
    private TechSetting techSetting;
    private MassSetting massSetting;
    private List<TechItem> removedTechItems;
    private Dictionary<string, string> building2Tech;
    private Dictionary<TechSetting, List<string>> buildings2Remove;

    public GameState() {
        this.opts = null;
        this.cgs = null;
        this.toggles = new Toggles();
        this.oxySetting = OxygenSetting.Normal;
        this.healthSetting = HealthSetting.Normal;
        this.powerSetting = PowerSetting.Normal;
        this.techSetting = TechSetting.Normal;
        this.massSetting = MassSetting.Normal;

        this.powerFactors = new Dictionary<PowerSetting, float>();
        this.powerFactors[PowerSetting.Normal] = 1.0f;
        this.powerFactors[PowerSetting.Brownout] = 0.75f;
        this.powerFactors[PowerSetting.Blackout] = 0.50f;

        this.powerInfos = new Dictionary<GeneratorType, GeneratorInfo>();
        this.powerInfos[GeneratorType.Manual] = new GeneratorInfo(ManualGeneratorConfig.ID, 400.0f);
        this.powerInfos[GeneratorType.Coal] = new GeneratorInfo(GeneratorConfig.ID, 600.0f);
        this.powerInfos[GeneratorType.NatGas] = new GeneratorInfo(MethaneGeneratorConfig.ID,
                                                                  800.0f);
        this.powerInfos[GeneratorType.Hydrogen] = new GeneratorInfo(HydrogenGeneratorConfig.ID,
                                                                    800.0f);
        this.powerInfos[GeneratorType.Petrol] = new GeneratorInfo(PetroleumGeneratorConfig.ID,
                                                                  2000.0f);
        this.powerInfos[GeneratorType.Wood] = new GeneratorInfo(WoodGasGeneratorConfig.ID, 300.0f);
        this.powerInfos[GeneratorType.Solar] = new GeneratorInfo(SolarPanelConfig.ID, 380.0f);
        this.powerInfos[GeneratorType.Turbine] = new GeneratorInfo(SteamTurbineConfig.ID,
                                                                   850.0f);
        this.powerInfos[GeneratorType.PlugSlug] = new GeneratorInfo(StaterpillarGeneratorConfig.ID,
                                                                    850.0f);

        this.oxyIncrs = new Dictionary<OxygenSetting, float>();
        this.oxyIncrs[OxygenSetting.Normal] = 0.0f;
        this.oxyIncrs[OxygenSetting.Gasping] = 0.05f; // 50 g/s
        this.oxyIncrs[OxygenSetting.Hyperventilating] = 0.1f; // 100 g/s

        this.healthIncrs = new Dictionary<HealthSetting, float>();
        this.healthIncrs[HealthSetting.Normal] = 0.0f;
        this.healthIncrs[HealthSetting.Delicate] = -25.0f;
        this.healthIncrs[HealthSetting.Fragile] = -50.0f;

        this.removedTechItems = new List<TechItem>();
        this.building2Tech = new Dictionary<string, string>();
        this.building2Tech[ElectrolyzerConfig.ID] = "ImprovedOxygen";
        this.building2Tech[SolarPanelConfig.ID] = "RenewableEnergy";
        this.building2Tech[DesalinatorConfig.ID] = "LiquidFiltering";
        this.building2Tech[WaterPurifierConfig.ID] = "Distillation";
        this.building2Tech[SteamTurbineConfig2.ID] = "RenewableEnergy";

        this.buildings2Remove = new Dictionary<TechSetting, List<string>>();
        this.buildings2Remove[TechSetting.Normal] = new List<string>();
        this.buildings2Remove[TechSetting.NoSpom] = new List<string>();
        this.buildings2Remove[TechSetting.NoSpom].Add(ElectrolyzerConfig.ID);
        this.buildings2Remove[TechSetting.NoSpom].Add(SolarPanelConfig.ID);
        this.buildings2Remove[TechSetting.ManualSieve] = new List<string>();
        this.buildings2Remove[TechSetting.ManualSieve].Add(DesalinatorConfig.ID);
        this.buildings2Remove[TechSetting.ManualSieve].Add(WaterPurifierConfig.ID);
        this.buildings2Remove[TechSetting.NoTurbine] = new List<string>();
        this.buildings2Remove[TechSetting.NoTurbine].Add(SteamTurbineConfig2.ID);

        this.massFactors = new Dictionary<MassSetting, float>();
        this.massFactors[MassSetting.Normal] = 1.0f;
        this.massFactors[MassSetting.LightTax] = 0.80f;
        this.massFactors[MassSetting.HeavyTax] = 0.60f;
    }

    public void applyGameSettings() {
        applyPowerSetting();
        applyTechSetting();

        OxygenSetting oxySettingTemp = OxygenSetting.Normal;
        SettingLevel oxySettingLvl =
            this.cgs.GetCurrentQualitySetting(Constants.ModPrefix + "Oxygen");
        if (oxySettingLvl == null) {
            Util.Log("apply: no oxy setting found; defaulting to Normal");
        } else if (!Enum.TryParse<OxygenSetting>(oxySettingLvl.id, out oxySettingTemp)) {
            Util.Log("apply: unable to parse oxygen setting {0}; defaulting to Normal",
                     oxySettingLvl.id);
        }
        this.oxySetting = oxySettingTemp;

        HealthSetting healthSettingTemp = HealthSetting.Normal;
        SettingLevel healthSettingLvl =
            this.cgs.GetCurrentQualitySetting(Constants.ModPrefix + "Health");
        if (healthSettingLvl == null) {
            Util.Log("apply: no health setting found; defaulting to Normal");
        } else if (!Enum.TryParse<HealthSetting>(healthSettingLvl.id, out healthSettingTemp)) {
            Util.Log("apply: unable to parse health setting {0}; defaulting to Normal",
                     healthSettingLvl.id);
        }
        this.healthSetting = healthSettingTemp;

        MassSetting massSettingTemp = MassSetting.Normal;
        SettingLevel massSettingLvl =
            this.cgs.GetCurrentQualitySetting(Constants.ModPrefix + "Mass");
        if (massSettingLvl == null) {
            Util.Log("apply: no mass setting found; defaulting to Normal");
        } else if (!Enum.TryParse<MassSetting>(massSettingLvl.id, out massSettingTemp)) {
            Util.Log("apply: unable to parse mass setting {0}; defaulting to Normal",
                     massSettingLvl.id);
        }
        this.massSetting = massSettingTemp;

        Util.Log("Game started with power:{0} oxygen:{1} health:{2} tech:{3} mass:{4}",
                 this.powerSetting, this.oxySetting, this.healthSetting, this.techSetting,
                 this.massSetting);
    }

    private void applyPowerSetting() {
        PowerSetting powerSettingTemp = PowerSetting.Normal;

        SettingLevel powerSettingLvl =
            this.cgs.GetCurrentQualitySetting(Constants.ModPrefix + "Power");
        if (powerSettingLvl == null) {
            Util.Log("apply: no power setting found; defaulting to Normal");
        } else if (!Enum.TryParse<PowerSetting>(powerSettingLvl.id, out powerSettingTemp)) {
            Util.Log("apply: unable to parse power setting {0}; defaulting to Normal",
                     powerSettingLvl.id);
        }

        foreach (GeneratorType genType in Enum.GetValues(typeof(GeneratorType))) {
            BuildingDef buildingDef = Assets.GetBuildingDef(this.powerInfos[genType].ID);
            if (buildingDef == null) {
                Util.Log("apply: could not find buildingDef for generator {0}({1})", genType,
                         this.powerInfos[genType].ID);
                continue;
            }
            buildingDef.GeneratorWattageRating =
                this.powerInfos[genType].baseWatts * this.powerFactors[powerSettingTemp];
        }
        this.powerSetting = powerSettingTemp;
    }

    private void applyTechSetting() {
        TechSetting techSettingTemp = TechSetting.Normal;
        SettingLevel techSettingLvl =
            this.cgs.GetCurrentQualitySetting(Constants.ModPrefix + "Tech");
        if (techSettingLvl == null) {
            Util.Log("apply: no tech setting found; defaulting to Normal");
        } else if (!Enum.TryParse<TechSetting>(techSettingLvl.id, out techSettingTemp)) {
            Util.Log("apply: unable to parse tech setting {0}; defaulting to Normal",
                     techSettingLvl.id);
        }
        this.techSetting = techSettingTemp;

        // ensure clean slate
        restoreTechTree();

        foreach (TechSetting techSetting in Enum.GetValues(typeof(TechSetting))) {
            foreach (string buildingId in this.buildings2Remove[techSetting]) {
                Tech tech = Db.Get().Techs.Get(this.building2Tech[buildingId]);
                if (tech == null) {
                    Util.Log("apply: could not find technology {0}; skipping {1}",
                             this.building2Tech[buildingId], buildingId);
                    continue;
                }
                foreach (TechItem item in new List<TechItem>(tech.unlockedItems)) {
                    if (item.Id == buildingId) {
                        Util.LogDbg("Removing {0} from {1}", buildingId,
                                    this.building2Tech[buildingId]);
                        tech.unlockedItems.Remove(item);
                        this.removedTechItems.Add(item);
                        break;
                    }
                }
            }

            if (this.techSetting == techSetting) {
                break;
            }
        }
    }

    // restore any tech items we've previously removed from the tech tree. this can happen
    // if a player loads another .sav with different settings
    private void restoreTechTree() {
        foreach (TechItem item in this.removedTechItems) {
            Tech tech = Db.Get().Techs.Get(this.building2Tech[item.Id]);
            if (tech == null) {
                Util.Log("restore: could not find technology {0}; skipping {1}",
                         this.building2Tech[item.Id], item.Id);
                continue;
            }
            Util.LogDbg("Restoring {0} to {1}", item.Id, this.building2Tech[item.Id]);
            tech.unlockedItems.Add(item);
        }

        this.removedTechItems.Clear();
    }

    // @todo list is small but consider making lookups O(1) as this is frequently invoked
    public bool shouldHideBuilding(BuildingDef def) {
        foreach (TechItem item in this.removedTechItems) {
            if (item.Id == def.PrefabID) {
                return true;
            }
        }

        return false;
    }

    public void applyDupeSettings(ref MinionIdentity dupe) {
        MinionModifiers modifiers = dupe.GetComponent<MinionModifiers>();
        if (modifiers == null) {
            Util.Log("apply: could not find dupe modifiers; skipping");
            return;
        }

        applyOxygenSetting(ref dupe, modifiers);
        applyHealthSetting(ref dupe, modifiers);
    }

    private void applyOxygenSetting(ref MinionIdentity dupe, MinionModifiers modifiers) {
        if (this.oxySetting == OxygenSetting.Normal) {
            return;
        }

        var oxyIncr = this.oxyIncrs[this.oxySetting];
        var airConsumptionId = Db.Get().Attributes.AirConsumptionRate.Id;
        AttributeModifier modifier =
            new AttributeModifier(airConsumptionId, oxyIncr, Constants.ModName);
        modifiers.attributes.Add(modifier);
    }

    private void applyHealthSetting(ref MinionIdentity dupe, MinionModifiers modifiers) {
        if (this.healthSetting == HealthSetting.Normal) {
            return;
        }

        var healthIncr = this.healthIncrs[this.healthSetting];
        var maxHitpointsId = Db.Get().Amounts.HitPoints.maxAttribute.Id;
        AttributeModifier modifier =
            new AttributeModifier(maxHitpointsId, healthIncr, Constants.ModName);
        modifiers.attributes.Add(modifier);

        Health health = dupe.GetComponent<Health>();
        if (health == null) {
            Util.Log("apply: could not find dupe health; skipping");
            return;
        }
        if (health.hitPoints > health.maxHitPoints) {
            health.hitPoints = health.maxHitPoints;
        }
    }

    public void applyMassSetting(ref float mass) {
        mass *= this.massFactors[this.massSetting];
    }

    public void addToggleSettings(ref CustomGameSettings cgs) {
        this.toggles.addAllToggles(ref cgs);
    }

    public void toggleSetting(ref CustomGameSettings cgs, SettingConfig config, string value) {
        this.toggles.toggleSetting(ref cgs, config, value);
    }
}
