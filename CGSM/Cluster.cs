// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System.Text;

namespace CGSM;

public class Cluster {
    public int radius { get; set; }
    public PlanetoidPlacement start { get; set; }
    public PlanetoidPlacement warp { get; set; }
    public List<PlanetoidPlacement> others { get; set; }
    public List<POIGroup> poiGroups { get; set; }
    public string name { get; set; }
    public bool storyTraits { get; set; }
    public int difficulty { get; set; }
    public enum PlacePriority {
        Start, Warp, FirstOther
    };
    public PlacePriority placePriority;

    public Cluster(string name, Options opts) {
        this.initCommon(name);
        this.radius = opts.starmapRadius;

        if (opts.warpPlanetoid == (WarpPlanetoidType) opts.startPlanetoid) {
            Util.Log("Warning: starting planetoid and warp planetoid are the same; this is untested");
        }

        if (opts.startPlanetoid == StartPlanetoidType.Marshy ||
            opts.startPlanetoid == StartPlanetoidType.Tundra) {
            this.storyTraits = false;
        }
        this.start = new PlanetoidPlacement(opts.startPlanetoid, 2);
        this.warp = new PlanetoidPlacement(opts.warpPlanetoid, 2, 4, 4);
        addOtherPlanets(opts);
        addPOIGroups(opts);

        Util.LogDbg("Created cluster {0}", this);
    }
    // empty cluster
    public Cluster(string nameIn, int radiusIn) {
        this.initCommon(nameIn);
        this.radius = radiusIn;
    }

    public bool hasOtherPlanetoid(PlanetoidType pType) {
        foreach (var placement in this.others) {
            if (placement.planetoid.Type() == pType) {
                return true;
            }
        }

        return false;
    }

    public bool hasAnyPlanetoid(PlanetoidType pType) {
        if (this.start.planetoid.Type() == pType) {
            return true;
        }
        if (this.warp.planetoid.Type() == pType) {
            return true;
        }

        return hasOtherPlanetoid(pType);
    }

    public void remove(PlanetoidType pType) {
        int idx = 0;
        bool found = false;

        foreach (var placement in this.others) {
            if (placement.planetoid.Type() == pType) {
                found = true;
                break;
            }
            idx++;
        }
        if (found) {
            this.others.RemoveAt(idx);
        }
    }

    private void initCommon(string nameIn) {
        this.poiGroups = new List<POIGroup>();
        this.others = new List<PlanetoidPlacement>();
        this.name = nameIn;
        this.storyTraits = true;
        this.difficulty = 5;
        this.placePriority = PlacePriority.Start;
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
        addOtherPlanetIfSet(opts.baatorOilySwampy, PlanetoidType.BaatorOilySwampy);
        addOtherPlanetIfSet(opts.baatorColdTerra, PlanetoidType.BaatorColdTerra);
        addOtherPlanetIfSet(opts.marshySnakes, PlanetoidType.MarshySnakes);
        addOtherPlanetIfSet(opts.superconductiveSnakes, PlanetoidType.SuperconductiveSnakes);
        addOtherPlanetIfSet(opts.waterSnakes, PlanetoidType.WaterSnakes);
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

        this.others.Add(new PlanetoidPlacement(planetoidType, PlanetoidCategory.Other, 4, 5,
                                               this.radius - 2, false));
    }

    private void addPOIGroups(Options optsIn) {
        addTearPOIGroup(11, this.radius - 1);
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

    public void addTearPOIGroup(int minRadius, int maxRadius) {
        var tearPoiGroup = new POIGroup();
        tearPoiGroup.minRadius = minRadius;
        tearPoiGroup.maxRadius = maxRadius;
        tearPoiGroup.avoidClumping = false;
        tearPoiGroup.Add(POIType.TemporalTear);

        this.poiGroups.Add(tearPoiGroup);
    }

    public override string ToString() {
        var content = new StringBuilder();
        content.Append(string.Format("Cluster {0}:\n", this.name));
        content.Append(string.Format("  radius:{0}\n", this.radius));
        content.Append(string.Format("  storyTraitsEnabled:{0}\n", this.storyTraits));
        content.Append(string.Format("  difficulty:{0}\n", this.difficulty));
        content.Append(string.Format("  placePriority:{0}\n", this.placePriority));
        content.Append(string.Format("  start:{0}\n", this.start));
        content.Append(string.Format("  warp:{0}\n", this.warp));
        int count = 0;
        foreach (var placement in this.others) {
            content.Append(string.Format("  other[{0}]:{1}\n", count, placement));
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

public static class BuiltinClusters {
    private static Dictionary<string, Cluster> clusterMap;

    static BuiltinClusters() {
        clusterMap = new Dictionary<string, Cluster>();
        reinit();
    }

    static public void reinit() {
        // vanilla style
        addVanillaSandstone();
        addVanillaOceania();
        addVanillaSwamp();
        addVanillaRime();
        addVanillaForest();
        addVanillaArboria();
        addVanillaVolcanea();
        addVanillaBadlands();
        addVanillaAridio();
        addVanillaOasis();

        // spaced out style
        addSOSandstone();
        addSOForest();
        addSOSwamp();
        addSODesolands();
        addSOFlipped();
        addSOForestFrozen();
        addSOMetallicSwampy();
        addSORadioactiveOcean();

        // mod clusters
        addBaatorStartCluster();
        addBaatorMoonletCluster();
        addFuleriaCluster();
        addVanillaTetramentCluster();
        addTetramentCluster();
    }

    // vanilla clusters
    private static void addVanillaSandstone() {
        var cluster = new Cluster("expansion1::clusters/VanillaSandstoneCluster", 12);
        cluster.difficulty = 0; // ideal
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.TerraVanilla, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveSwamp, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void  addVanillaOceania() {
        var cluster = new Cluster("expansion1::clusters/VanillaOceaniaCluster", 12);
        cluster.difficulty = 1;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Oceania, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.GlowoodWasteland, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    public static void addVanillaSwamp() {
        var cluster = new Cluster("expansion1::clusters/VanillaSwampCluster", 12);
        cluster.difficulty = 2;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Squelchy, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveForest, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
     }

    public static void addVanillaRime(){
        var cluster = new Cluster("expansion1::clusters/VanillaSandstoneFrozenCluster", 12);
        cluster.difficulty = 2;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Rime, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.StinkoSwamp, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    public static void addVanillaForest(){
        var cluster = new Cluster("expansion1::clusters/VanillaForestCluster", 12);
        cluster.difficulty = 2;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Verdante, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerra, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }
    public static void addVanillaArboria(){
        var cluster = new Cluster("expansion1::clusters/VanillaArboriaCluster", 12);
        cluster.difficulty = 3;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Arboria, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerra, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }
    public static void addVanillaVolcanea(){
        var cluster = new Cluster("expansion1::clusters/VanillaVolcanicCluster", 12);
        cluster.difficulty = 3;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Volcanea, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerra, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }
    public static void addVanillaBadlands(){
        var cluster = new Cluster("expansion1::clusters/VanillaBadlandsCluster", 12);
        cluster.difficulty = 4;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Badlands, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerra, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }
    public static void addVanillaAridio(){
        var cluster = new Cluster("expansion1::clusters/VanillaAridioCluster", 12);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Aridio, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerrabog, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }
    public static void addVanillaOasis(){
        var cluster = new Cluster("expansion1::clusters/VanillaOasisCluster", 12);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Oasisse, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveSwamp, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    // spaced out clusters
    private static void addSOSandstone() {
        var cluster = new Cluster("expansion1::clusters/SandstoneStartCluster", 12);
        cluster.difficulty = 0; // ideal
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Terrania, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.OilySwamp, 4, 5, 5);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.IrradiatedForest,
                                                  PlanetoidCategory.Other, 2, 3, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 10, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 10, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSOForest() {
        var cluster = new Cluster("expansion1::clusters/ForestStartCluster", 12);
        cluster.difficulty = 0; // ideal
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Folia, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RustyOil, 3, 4, 4);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.IrradiatedSwamp,
                                                  PlanetoidCategory.Other, 2, 3, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 10, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 10, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSOSwamp() {
        var cluster = new Cluster("expansion1::clusters/SwampStartCluster", 12);
        cluster.difficulty = 0; // ideal
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Quagmiris, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RustyOil, 3, 4, 4);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.IrradiatedMarsh,
                                                  PlanetoidCategory.Other, 2, 3, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 10, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 10, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSODesolands() {
        var cluster = new Cluster("expansion1::clusters/MiniClusterBadlandsStart", 14);
        cluster.difficulty = 3;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Desolands, 2, 1);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveOcean, 2, 2, 4);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MetallicSwampy,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.FrozenForest,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Flipped,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 8, 11, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 11, false));
        cluster.addTearPOIGroup(9, 12);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        addSize14POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSOFlipped() {
        var cluster = new Cluster("expansion1::clusters/MiniClusterFlippedStart", 14);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Flipped, 2, 2, 4);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.Desolands, 2, 0, 1);
        cluster.placePriority = Cluster.PlacePriority.Warp;
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MetallicSwampy,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.FrozenForest,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.RadioactiveOcean,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 8, 11, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 11, false));
        cluster.addTearPOIGroup(9, 12);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        addSize14POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSOForestFrozen() {
        var cluster = new Cluster("expansion1::clusters/MiniClusterForestFrozenStart", 14);
        cluster.difficulty = 3;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.FrozenForest, 2, 1, 3);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.Desolands, 2, 0, 1);
        cluster.placePriority = Cluster.PlacePriority.Warp;
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MetallicSwampy,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Flipped,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.RadioactiveOcean,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 8, 11, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 11, false));
        cluster.addTearPOIGroup(9, 12);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        addSize14POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSOMetallicSwampy() {
        var cluster = new Cluster("expansion1::clusters/MiniClusterMetallicSwampyStart", 14);
        cluster.difficulty = 2;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.MetallicSwampy, 2, 1, 3);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.FrozenForest, 2, 1, 3);
        cluster.placePriority = Cluster.PlacePriority.FirstOther;
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Desolands,
                                                  PlanetoidCategory.Other, 2, 0, 1, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Flipped,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.RadioactiveOcean,
                                                  PlanetoidCategory.Other, 2, 2, 4, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 8, 11, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 11, false));
        cluster.addTearPOIGroup(9, 12);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        addSize14POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSORadioactiveOcean() {
        var cluster = new Cluster("expansion1::clusters/MiniClusterRadioactiveOceanStart", 14);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.RadioactiveOcean, 2, 2, 4);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.Flipped, 2, 2, 4);
        cluster.placePriority = Cluster.PlacePriority.FirstOther;
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Desolands,
                                                  PlanetoidCategory.Other, 2, 0, 1, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MetallicSwampy,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.FrozenForest,
                                                  PlanetoidCategory.Other, 2, 1, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 7, 9, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 8, 11, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 11, false));
        cluster.addTearPOIGroup(9, 12);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        addSize14POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    // modded clusters
    private static void addBaatorStartCluster() {
        var cluster = new Cluster("expansion1::clusters/BaatorStartCluster", 12);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Baator, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveSwamp, 2, 4, 4);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.BaatorOilySwampy,
                                               PlanetoidCategory.Other, 2, 1, 3, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MetallicSwampy,
                                                  PlanetoidCategory.Other, 1, 2, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.IrradiatedForest,
                                                  PlanetoidCategory.Other, 2, 3, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 8, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 7, 10, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 10, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SandyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addBaatorMoonletCluster() {
        var cluster = new Cluster("expansion1::clusters/BaatorMoonletCluster", 12);
        cluster.difficulty = 5;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.BaatorMoonlet, 1);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveSwamp, 2, 4, 4);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.BaatorColdTerra,
                                                  PlanetoidCategory.Other, 1, 1, 4, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.BaatorOilySwampy,
                                                  PlanetoidCategory.Other, 2, 3, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.IrradiatedForest,
                                                  PlanetoidCategory.Other, 2, 3, 3, true));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Regolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 10, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addFuleriaCluster() {
        var cluster = new Cluster("expansion1::clusters/FuleriaDLC", 12);
        cluster.difficulty = 1;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Fuleria, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveTerra, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addVanillaTetramentCluster() {
        var cluster = new Cluster("clusters/TetramentClassic", 12);
        cluster.difficulty = 2;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.VanillaTetrament, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.RadioactiveSwamp, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Marshy, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Superconductive,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Water,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addTetramentCluster() {
        var cluster = new Cluster("clusters/TetramentCluster", 12);
        cluster.difficulty = 3;
        cluster.start = new PlanetoidPlacement(StartPlanetoidType.Tetrament, 2);
        cluster.warp = new PlanetoidPlacement(WarpPlanetoidType.DryRadioactiveForest, 2, 3, 3);
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Tundra, PlanetoidCategory.Other,
                                                  4, 5, 5, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MarshySnakes, PlanetoidCategory.Other,
                                                  4, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.SuperconductiveSnakes,
                                                  PlanetoidCategory.Other,
                                                  3, 5, 6, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.Moo,
                                                  PlanetoidCategory.Other,
                                                  3, 6, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.WaterSnakes,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 7, false));
        cluster.others.Add(new PlanetoidPlacement(PlanetoidType.MiniRegolith,
                                                  PlanetoidCategory.Other,
                                                  4, 5, 8, false));
        cluster.addTearPOIGroup(8, 11);

        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        addSize12POICommon(cluster);

        clusterMap[cluster.name] = cluster;
    }

    private static void addSize12POICommon(Cluster cluster) {
        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.OrganicMassField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 5;
        poiGroup.maxRadius = 7;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.GildedAsteroidField);
        poiGroup.Add(HarvestablePOIs.GlimmeringAsteroidField);
        poiGroup.Add(HarvestablePOIs.HeliumCloud);
        poiGroup.Add(HarvestablePOIs.OilyAsteroidField);
        poiGroup.Add(HarvestablePOIs.FrozenOreField);
        poiGroup.minRadius = 8;
        poiGroup.maxRadius = 11;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(HarvestablePOIs.RadioactiveGasCloud);
        poiGroup.Add(HarvestablePOIs.RadioactiveAsteroidField);
        poiGroup.minRadius = 10;
        poiGroup.maxRadius = 11;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.RockyAsteroidField);
        poiGroup.Add(HarvestablePOIs.InterstellarIceField);
        poiGroup.Add(HarvestablePOIs.InterstellarOcean);
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.Add(HarvestablePOIs.OrganicMassField);
        poiGroup.numToSpawn = 5;
        poiGroup.allowDupes = true;
        poiGroup.minRadius = 5;
        poiGroup.maxRadius = 7;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.CarbonAsteroidField);
        poiGroup.Add(HarvestablePOIs.MetallicAsteroidField);
        poiGroup.Add(HarvestablePOIs.SatelliteField);
        poiGroup.Add(HarvestablePOIs.IceAsteroidField);
        poiGroup.Add(HarvestablePOIs.GasGiantCloud);
        poiGroup.Add(HarvestablePOIs.ChlorineCloud);
        poiGroup.Add(HarvestablePOIs.OxidizedAsteroidField);
        poiGroup.Add(HarvestablePOIs.SaltyAsteroidField);
        poiGroup.Add(HarvestablePOIs.OxygenRichAsteroidField);
        poiGroup.Add(HarvestablePOIs.GildedAsteroidField);
        poiGroup.Add(HarvestablePOIs.GlimmeringAsteroidField);
        poiGroup.Add(HarvestablePOIs.HeliumCloud);
        poiGroup.Add(HarvestablePOIs.OilyAsteroidField);
        poiGroup.Add(HarvestablePOIs.FrozenOreField);
        poiGroup.Add(HarvestablePOIs.RadioactiveGasCloud);
        poiGroup.Add(HarvestablePOIs.RadioactiveAsteroidField);
        poiGroup.numToSpawn = 10;
        poiGroup.allowDupes = true;
        poiGroup.minRadius = 7;
        poiGroup.maxRadius = 11;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation1);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation4);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation6);
        poiGroup.numToSpawn = 1;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 3;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.RussellsTeapot);
        poiGroup.minRadius = 9;
        poiGroup.maxRadius = 11;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation2);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation3);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation5);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation7);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation8);
        poiGroup.numToSpawn = 4;
        poiGroup.minRadius = 4;
        poiGroup.maxRadius = 11;
        cluster.poiGroups.Add(poiGroup);
    }

    private static void addSize14POICommon(Cluster cluster) {
        var poiGroup = new POIGroup();
        poiGroup.Add(HarvestablePOIs.OrganicMassField);
        poiGroup.avoidClumping = false;
        poiGroup.minRadius = 6;
        poiGroup.maxRadius = 8;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.GildedAsteroidField);
        poiGroup.Add(HarvestablePOIs.GlimmeringAsteroidField);
        poiGroup.Add(HarvestablePOIs.HeliumCloud);
        poiGroup.Add(HarvestablePOIs.OilyAsteroidField);
        poiGroup.Add(HarvestablePOIs.FrozenOreField);
        poiGroup.minRadius = 9;
        poiGroup.maxRadius = 12;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(HarvestablePOIs.RadioactiveGasCloud);
        poiGroup.Add(HarvestablePOIs.RadioactiveAsteroidField);
        poiGroup.minRadius = 11;
        poiGroup.maxRadius = 12;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.RockyAsteroidField);
        poiGroup.Add(HarvestablePOIs.InterstellarIceField);
        poiGroup.Add(HarvestablePOIs.InterstellarOcean);
        poiGroup.Add(HarvestablePOIs.ForestyOreField);
        poiGroup.Add(HarvestablePOIs.SwampyOreField);
        poiGroup.Add(HarvestablePOIs.OrganicMassField);
        poiGroup.numToSpawn = 5;
        poiGroup.allowDupes = true;
        poiGroup.minRadius = 6;
        poiGroup.maxRadius = 8;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = false;
        poiGroup.Add(HarvestablePOIs.CarbonAsteroidField);
        poiGroup.Add(HarvestablePOIs.MetallicAsteroidField);
        poiGroup.Add(HarvestablePOIs.SatelliteField);
        poiGroup.Add(HarvestablePOIs.IceAsteroidField);
        poiGroup.Add(HarvestablePOIs.GasGiantCloud);
        poiGroup.Add(HarvestablePOIs.ChlorineCloud);
        poiGroup.Add(HarvestablePOIs.OxidizedAsteroidField);
        poiGroup.Add(HarvestablePOIs.SaltyAsteroidField);
        poiGroup.Add(HarvestablePOIs.OxygenRichAsteroidField);
        poiGroup.Add(HarvestablePOIs.GildedAsteroidField);
        poiGroup.Add(HarvestablePOIs.GlimmeringAsteroidField);
        poiGroup.Add(HarvestablePOIs.HeliumCloud);
        poiGroup.Add(HarvestablePOIs.OilyAsteroidField);
        poiGroup.Add(HarvestablePOIs.FrozenOreField);
        poiGroup.Add(HarvestablePOIs.RadioactiveGasCloud);
        poiGroup.Add(HarvestablePOIs.RadioactiveAsteroidField);
        poiGroup.numToSpawn = 10;
        poiGroup.allowDupes = true;
        poiGroup.minRadius = 8;
        poiGroup.maxRadius = 12;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation1);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation4);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation6);
        poiGroup.numToSpawn = 1;
        poiGroup.minRadius = 2;
        poiGroup.maxRadius = 4;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.RussellsTeapot);
        poiGroup.minRadius = 10;
        poiGroup.maxRadius = 12;
        cluster.poiGroups.Add(poiGroup);
        poiGroup = new POIGroup();
        poiGroup.avoidClumping = true;
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation2);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation3);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation5);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation7);
        poiGroup.Add(ArtifactPOIs.GravitasSpaceStation8);
        poiGroup.numToSpawn = 4;
        poiGroup.minRadius = 5;
        poiGroup.maxRadius = 12;
        cluster.poiGroups.Add(poiGroup);
    }

    public static Cluster lookup(string clusterName) {
        if (clusterMap.ContainsKey(clusterName)) {
            return clusterMap[clusterName];
        }

        return null;
    }
}
