// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System.Text;

namespace CGSM;

public class Cluster {
    public int radius { get; }
    public PlanetoidPlacement start { get; }
    public PlanetoidPlacement warp { get; }
    public List<PlanetoidPlacement> others { get; }
    public List<POIGroup> poiGroups { get; }

    public Cluster(Options opts) {
        this.poiGroups = new List<POIGroup>();
        this.others = new List<PlanetoidPlacement>();
        this.radius = opts.starmapRadius;

        if (opts.warpPlanetoid == (WarpPlanetoidType) opts.startPlanetoid) {
            Util.Log("Warning: starting planetoid and warp planetoid are the same; this is untested");
        }

        this.start = new PlanetoidPlacement(opts.startPlanetoid, 2);
        this.warp = new PlanetoidPlacement(opts.warpPlanetoid, 2, 4, 4);
        addOtherPlanets(opts);
        addPOIGroups(opts);

        Util.LogDbg("Created cluster {0}", this);
    }

    private void addOtherPlanets(Options opts) {
        addOtherPlanetIfSet(opts.metallicSwampyPlanetoid, PlanetoidType.MetallicSwampy);
        addOtherPlanetIfSet(opts.desolandsPlanetoid, PlanetoidType.Desolands);
        addOtherPlanetIfSet(opts.frozenForestPlanetoid, PlanetoidType.FrozenForest);
        addOtherPlanetIfSet(opts.flippedPlanetoid, PlanetoidType.Flipped);
        addOtherPlanetIfSet(opts.radioactiveOceanPlanetoid, PlanetoidType.RadioactiveOcean);
        addOtherPlanetIfSet(opts.tundraPlanetoid, PlanetoidType.Tundra);
        addOtherPlanetIfSet(opts.marshyPlanetoid, PlanetoidType.Marshy);
        addOtherPlanetIfSet(opts.mooPlanetoid, PlanetoidType.Moo);
        addOtherPlanetIfSet(opts.waterPlanetoid, PlanetoidType.Water);
        addOtherPlanetIfSet(opts.firePlanetoid, PlanetoidType.Superconductive);
        addOtherPlanetIfSet(opts.regolithPlanetoid, PlanetoidType.Regolith);
        addOtherPlanetIfSet(opts.irradiatedForestPlanetoid, PlanetoidType.IrradiatedForest);
        addOtherPlanetIfSet(opts.irradiatedSwampPlanetoid, PlanetoidType.IrradiatedSwamp);
        addOtherPlanetIfSet(opts.irradiatedMarshPlanetoid, PlanetoidType.IrradiatedMarsh);
    }

    private void addOtherPlanetIfSet(bool optVal, PlanetoidType planetoidType) {
        if (!optVal) {
            return;
        }

        // @todo add conflict checking
        if (planetoidType == this.start.planetoid.Type()) {
            Util.Log("Skipping other planetoid {0} since it is already the starting one",
                     planetoidType);
            return;
        }
        if (planetoidType == this.warp.planetoid.Type()) {
            Util.Log("Skipping other planetoid {0} since it is already the warp one",
                     planetoidType);
            return;
        }

        this.others.Add(new PlanetoidPlacement(planetoidType, PlanetoidCategory.Other, 5,
                                               this.radius - 2, 4, false));
    }

    private void addPOIGroups(Options optsIn) {
        addTearPOIGroup();
        addHarvestPOIGroup(optsIn);
        addArtifactPOIGroup(optsIn);
    }

    private void addHarvestPOIGroup(Options optsIn) {
        var harvestPoiGroup = new POIGroup();
        harvestPoiGroup.minRadius = 4;
        harvestPoiGroup.maxRadius = this.radius - 1;

        harvestPoiGroup.AddIfSet(optsIn.carbonAsteroid, HarvestablePOIs.CarbonAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.chlorineCloud, HarvestablePOIs.ChlorineCloud);
        harvestPoiGroup.AddIfSet(optsIn.forestyOreField, HarvestablePOIs.ForestyOreField);
        harvestPoiGroup.AddIfSet(optsIn.frozenOreField, HarvestablePOIs.FrozenOreField);
        harvestPoiGroup.AddIfSet(optsIn.gasGiantCloud, HarvestablePOIs.GasGiantCloud);
        harvestPoiGroup.AddIfSet(optsIn.gildedAsteroidField, HarvestablePOIs.GildedAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.glimmeringAsteroidField,
                                 HarvestablePOIs.GlimmeringAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.heliumCloud, HarvestablePOIs.HeliumCloud);
        harvestPoiGroup.AddIfSet(optsIn.iceAsteroidField, HarvestablePOIs.IceAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.interstellarIceField, HarvestablePOIs.InterstellarIceField);
        harvestPoiGroup.AddIfSet(optsIn.interstellarOcean, HarvestablePOIs.InterstellarOcean);
        harvestPoiGroup.AddIfSet(optsIn.metallicAsteroidField,
        HarvestablePOIs.MetallicAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.oilyAsteroidField, HarvestablePOIs.OilyAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.organicMassField, HarvestablePOIs.OrganicMassField);
        harvestPoiGroup.AddIfSet(optsIn.oxidizedAsteroidField,
                                 HarvestablePOIs.OxidizedAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.oxygenRichAsteroidField,
                                 HarvestablePOIs.OxygenRichAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.radioactiveAsteroidField,
                                 HarvestablePOIs.RadioactiveAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.radioactiveGasCloud, HarvestablePOIs.RadioactiveGasCloud);
        harvestPoiGroup.AddIfSet(optsIn.rockyAsteroidField, HarvestablePOIs.RockyAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.saltyAsteroidField, HarvestablePOIs.SaltyAsteroidField);
        harvestPoiGroup.AddIfSet(optsIn.sandyOreField, HarvestablePOIs.SandyOreField);
        harvestPoiGroup.AddIfSet(optsIn.satelliteField, HarvestablePOIs.SatelliteField);
        harvestPoiGroup.AddIfSet(optsIn.swampyOreField, HarvestablePOIs.SwampyOreField);

        this.poiGroups.Add(harvestPoiGroup);
    }

    private void addArtifactPOIGroup(Options optsIn) {
        var artifactPoiGroup = new POIGroup();
        artifactPoiGroup.minRadius = 4;
        artifactPoiGroup.maxRadius = this.radius - 1;

        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation1);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation2);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation3);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation4);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation5);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation6);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation7);
        artifactPoiGroup.Add(ArtifactPOIs.GravitasSpaceStation8);
        artifactPoiGroup.Add(ArtifactPOIs.RussellsTeapot);

        artifactPoiGroup.numToSpawn = optsIn.numArtifactPOIs;

        this.poiGroups.Add(artifactPoiGroup);
    }

    private void addTearPOIGroup() {
        var tearPoiGroup = new POIGroup();
        tearPoiGroup.minRadius = 11;
        tearPoiGroup.maxRadius = this.radius - 1;
        tearPoiGroup.avoidClumping = false;
        tearPoiGroup.Add(POIType.TemporalTear);

        this.poiGroups.Add(tearPoiGroup);
    }

    public override string ToString() {
        var content = new StringBuilder();
        content.Append("Cluster:\n");
        content.Append(string.Format("  radius:{0}\n", this.radius));
        content.Append(string.Format("  start:{0}\n", this.start));
        content.Append(string.Format("  warp:{0}\n", this.warp));
        int count = 0;
        foreach (var placement in this.others) {
            content.Append(string.Format("  other[{0}]: {1}\n", count, placement));
            count++;
        }
        count = 0;
        foreach (var poiGroup in this.poiGroups) {
            content.Append(string.Format("  poiGroup[{0}]: {1}\n", count, poiGroup));
            count++;
        }

        return content.ToString();
    }
}
