// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

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

    // other planets are now effectively "mix-in placeholders", so just pick some reasonable
    // defaults here.
    private void addOtherPlanets(Options opts) {
        // add 9 total so we have the mix-in slots available; should be 1 inner and the
	// rest outters
        int addedCount = 0;
	bool addInner = false;

        addedCount += addOtherPlanet(PlanetoidType.Tundra, false);
        addedCount += addOtherPlanet(PlanetoidType.Marshy, false);
        addedCount += addOtherPlanet(PlanetoidType.Moo, false);
        addedCount += addOtherPlanet(PlanetoidType.Water, false);
        addedCount += addOtherPlanet(PlanetoidType.Superconductive, false);
        addedCount += addOtherPlanet(PlanetoidType.MiniRegolith, false);
	addInner = true;
        if (addOtherPlanet(PlanetoidType.Desolands, addInner) > 0) {
	    addInner = false;
	    addedCount++;
	}
        if (addOtherPlanet(PlanetoidType.Flipped, addInner) > 0) {
	    addInner = false;
	    addedCount++;
	}
        if (addOtherPlanet(PlanetoidType.MetallicSwampy, addInner) > 0) {
	    addInner = false;
	    addedCount++;
	}
        if (addedCount < 9 && addOtherPlanet(PlanetoidType.FrozenForest, addInner) > 0) {
	    addInner = false;
	    addedCount++;
	}
        if (addedCount < 9 && addOtherPlanet(PlanetoidType.RadioactiveOcean, addInner) > 0) {
	    addInner = false;
	    addedCount++;
	}
    }

    private int addOtherPlanet(PlanetoidType planetoidType, bool isInner) {
        // @todo add conflict checking
        if (planetoidType == this.start.planetoid.Type()) {
            Util.LogDbg("Skipping other planetoid {0} since it is already the starting one",
                     planetoidType);
            return 0;
        }
        if (planetoidType == this.warp.planetoid.Type()) {
            Util.LogDbg("Skipping other planetoid {0} since it is already the warp one",
                     planetoidType);
            return 0;
        }

        if (isInner) {
            this.others.Add(new PlanetoidPlacement(planetoidType, PlanetoidCategory.Other, 2, 2,
                                                   this.radius - 5, true));
	} else {
            this.others.Add(new PlanetoidPlacement(planetoidType, PlanetoidCategory.Other, 4, 5,
                                                   this.radius - 2, isInner));
        }
        return 1;
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
        // These POIs only exist when the corresponding DLC is active.
        if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID)) {
            harvestPoiGroup.AddIfSet(optsIn.dlc2CeresOreField, HarvestablePOIs.DLC2CeresOreField);
        }
        if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID)) {
            harvestPoiGroup.AddIfSet(optsIn.dlc4PrehistoricOreField, HarvestablePOIs.DLC4PrehistoricOreField);
        }

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
