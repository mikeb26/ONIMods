// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;

namespace QuickStart;

public class TechBufs {
    public class TechBufInfo {
        public string buildingId;
        public bool dlcOnly;
        public bool vanillaOnly;
        public StartLevel minStartLevel;

        public TechBufInfo(string buildingIdIn, bool dlcOnlyIn, bool vanillaOnlyIn,
                           StartLevel startLevelIn) {
            this.buildingId = buildingIdIn;
            this.dlcOnly = dlcOnlyIn;
            this.vanillaOnly = vanillaOnlyIn;
            this.minStartLevel = startLevelIn;
        }
    }
    private bool baseGameOnly;
    private Dictionary<string, TechBufInfo> techs2Unlock;

    public TechBufs(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.techs2Unlock = new Dictionary<string, TechBufInfo>();

        // Advanced Early Techs
        // grill
        var techInfo = new TechBufInfo(CookingStationConfig.ID, false, false,
                                       StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // grooming station
        techInfo = new TechBufInfo(RanchStationConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(BatterySmartConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(FirePoleConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(RockCrusherConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // super computer
        techInfo = new TechBufInfo(AdvancedResearchCenterConfig.ID, false, false,
                                   StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // coal generator
        techInfo = new TechBufInfo(GeneratorConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(FlushToiletConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // insulated liquid pipe
        techInfo = new TechBufInfo(InsulatedLiquidConduitConfig.ID, false, false,
                                   StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(InsulationTileConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // deodorizer
        techInfo = new TechBufInfo(AirFilterConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // insulated gas pipe
        techInfo = new TechBufInfo(InsulatedGasConduitConfig.ID, false, false,
                                   StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(ElectrolyzerConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // water sieve
        techInfo = new TechBufInfo(WaterPurifierConfig.ID, false, false, StartLevel.AdvancedEarly);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);

        // Mid Techs
        techInfo = new TechBufInfo(FarmStationConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(PetroleumGeneratorConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(OilWellCapConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // heavy-watt conductive wire
        techInfo = new TechBufInfo(WireRefinedHighWattageConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(SteamTurbineConfig2.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // auto-sweeper
        techInfo = new TechBufInfo(SolidTransferArmConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // conveyor loader
        techInfo = new TechBufInfo(SolidConduitInboxConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(MetalRefineryConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(SodaFountainConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // thermo aquatuner
        techInfo = new TechBufInfo(LiquidConditionerConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // atmosuit dock
        techInfo = new TechBufInfo(SuitLockerConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // plastic ladder
        techInfo = new TechBufInfo(LadderFastConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(LogicGateAndConfig.ID, false, false, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // small petrol engine
        techInfo = new TechBufInfo(KeroseneEngineClusterSmallConfig.ID, true, false,
                                   StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // vanilla steam engine
        techInfo = new TechBufInfo(SteamEngineConfig.ID, false, true, StartLevel.Mid);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);

        // Late Techs
        // large petrol engine
        techInfo = new TechBufInfo(KeroseneEngineClusterConfig.ID, true, false,
                                   StartLevel.Late);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        // large petrol engine (vanilla)
        techInfo = new TechBufInfo(KeroseneEngineConfig.ID, false, true, StartLevel.Late);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
        techInfo = new TechBufInfo(NuclearReactorConfig.ID, true, false, StartLevel.Late);
        this.techs2Unlock.Add(techInfo.buildingId, techInfo);
    }

    public void UnlockResearch(QuickStartOptions opts) {
        Util.Log("Unlocking research for a {0} start.", opts.startLevel);

        foreach (var tech in Db.Get().Techs.resources) {
            // Util.Log("tech item:{0}", tech.Id);

            foreach (var buildingId in tech.unlockedItemIDs) {

                // Util.Log("\tbuilding:{0}", buildingId);

                if (this.techs2Unlock.TryGetValue(buildingId, out TechBufInfo tBufInfo) == false) {
                    continue;
                }
                if (tBufInfo.minStartLevel > opts.startLevel) {
                    continue;
                }
                if (tBufInfo.dlcOnly && this.baseGameOnly) {
                    continue;
                }
                if (tBufInfo.vanillaOnly && !this.baseGameOnly) {
                    continue;
                }
                    
                doUnlock(tech);
                break;
            }
        }
    }

    private void doUnlock(Tech tech) {
        foreach (var parentTech in tech.requiredTech) {
            if (parentTech.IsComplete() == false) {
                doUnlock(parentTech);
            }
        }

        Research.Instance.GetOrAdd(tech).Purchased();
        // see Research.checkBuyResearch(); several modules have callbacks queued on research
        // complete events so we need to trigger them here. e.g. PlanScreen.OnPrefabInit()
        // registers one
        Game.Instance.Trigger((int)GameHashes.ResearchComplete, tech);
    }
}
