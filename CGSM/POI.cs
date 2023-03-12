// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System.Text;

namespace CGSM;

public enum POIType {
    CarbonAsteroidField = 0,
    ChlorineCloud,
    ForestyOreField,
    FrozenOreField,
    GasGiantCloud,
    GildedAsteroidField,
    GlimmeringAsteroidField,
    HeliumCloud,
    IceAsteroidField,
    InterstellarIceField,
    InterstellarOcean,
    MetallicAsteroidField,
    OilyAsteroidField,
    OrganicMassField,
    OxidizedAsteroidField,
    OxygenRichAsteroidField,
    RadioactiveAsteroidField,
    RadioactiveGasCloud,
    RockyAsteroidField,
    SaltyAsteroidField,
    SandyOreField,
    SatelliteField,
    SwampyOreField,
    TemporalTear,
    GravitasSpaceStation1,
    GravitasSpaceStation2,
    GravitasSpaceStation3,
    GravitasSpaceStation4,
    GravitasSpaceStation5,
    GravitasSpaceStation6,
    GravitasSpaceStation7,
    GravitasSpaceStation8,
    RussellsTeapot,
};

public enum HarvestablePOIs {
    CarbonAsteroidField = POIType.CarbonAsteroidField,
    ChlorineCloud = POIType.ChlorineCloud,
    ForestyOreField = POIType.ForestyOreField,
    FrozenOreField = POIType.FrozenOreField,
    GasGiantCloud = POIType.GasGiantCloud,
    GildedAsteroidField = POIType.GildedAsteroidField,
    GlimmeringAsteroidField = POIType.GlimmeringAsteroidField,
    HeliumCloud = POIType.HeliumCloud,
    IceAsteroidField = POIType.IceAsteroidField,
    InterstellarIceField = POIType.InterstellarIceField,
    InterstellarOcean = POIType.InterstellarOcean,
    MetallicAsteroidField = POIType.MetallicAsteroidField,
    OilyAsteroidField = POIType.OilyAsteroidField,
    OrganicMassField = POIType.OrganicMassField,
    OxidizedAsteroidField = POIType.OxidizedAsteroidField,
    OxygenRichAsteroidField = POIType.OxygenRichAsteroidField,
    RadioactiveAsteroidField = POIType.RadioactiveAsteroidField,
    RadioactiveGasCloud = POIType.RadioactiveGasCloud,
    RockyAsteroidField = POIType.RockyAsteroidField,
    SaltyAsteroidField = POIType.SaltyAsteroidField,
    SandyOreField = POIType.SandyOreField,
    SatelliteField = POIType.SatelliteField,
    SwampyOreField = POIType.SwampyOreField,
};

public enum ArtifactPOIs {
    GravitasSpaceStation1 = POIType.GravitasSpaceStation1,
    GravitasSpaceStation2 = POIType.GravitasSpaceStation2,
    GravitasSpaceStation3 = POIType.GravitasSpaceStation3,
    GravitasSpaceStation4 = POIType.GravitasSpaceStation4,
    GravitasSpaceStation5 = POIType.GravitasSpaceStation5,
    GravitasSpaceStation6 = POIType.GravitasSpaceStation6,
    GravitasSpaceStation7 = POIType.GravitasSpaceStation7,
    GravitasSpaceStation8 = POIType.GravitasSpaceStation8,
    RussellsTeapot = POIType.RussellsTeapot,
};

public static class POI {
    public static bool isArtifact(POIType poi) {
        if (poi >= POIType.GravitasSpaceStation1 &&
            poi <= POIType.RussellsTeapot) {
            return true;
        }

        return false;
    }

    public static bool isHarvestable(POIType poi) {
        if (poi >= POIType.CarbonAsteroidField &&
            poi <= POIType.SwampyOreField) {
            return true;
        }

        return false;
    }

    public static string ToYamlString(POIType poi) {
        if (POI.isHarvestable(poi)) {
            return POI.ToYamlString((HarvestablePOIs) poi);
        } else if (POI.isArtifact(poi)) {
            return POI.ToYamlString((ArtifactPOIs) poi);
        }

        return poi.ToString(); // tear
    }

    public static string ToYamlString(HarvestablePOIs poi) {
        return string.Format("HarvestableSpacePOI_{0}", poi.ToString());
    }
    public static string ToYamlString(ArtifactPOIs poi) {
        return string.Format("ArtifactSpacePOI_{0}", poi.ToString());
    }
}

public class POIGroup {
    public List<POIType> poiList { get; }
    public int numToSpawn {get; set; }
    public bool avoidClumping {get; set; }
    public int minRadius {get; set; }
    public int maxRadius {get; set; }
    public bool allowDupes {get; set; }

    public POIGroup() {
        this.poiList = new List<POIType>();
        this.numToSpawn = 0;
        this.avoidClumping = true;
        this.minRadius = 0;
        this.maxRadius = 12; // vanilla cluster default
        this.allowDupes = false;
    }

    public void Add(POIType poi) {
        this.poiList.Add(poi);
        this.numToSpawn++;
    }
    public void Add(HarvestablePOIs poi) {
        this.Add((POIType) poi);
    }
    public void AddIfSet(bool optVal, HarvestablePOIs poi) {
        if (!optVal) {
            return;
        }
        this.Add(poi);
    }
    public void Add(ArtifactPOIs poi) {
        this.Add((POIType) poi);
    }

    public string ToYamlString() {
        var yamlContent = new StringBuilder();

        yamlContent.Append("  - pois:\n");
        foreach (var poi in this.poiList) {
            yamlContent.Append(string.Format("      - {0}\n", POI.ToYamlString(poi)));
        }
        yamlContent.Append(string.Format("    numToSpawn: {0}\n", this.numToSpawn));
        if (this.avoidClumping) {
            yamlContent.Append("    avoidClumping: true\n");
        } else {
            yamlContent.Append("    avoidClumping: false\n");
        }
        if (this.allowDupes) {
            yamlContent.Append("    canSpawnDuplicates: true\n");
        } else {
            yamlContent.Append("    canSpawnDuplicates: false\n");
        }
        yamlContent.Append("    allowedRings:\n");
        yamlContent.Append(string.Format("      min: {0}\n", this.minRadius));
        yamlContent.Append(string.Format("      max: {0}\n", this.maxRadius));

        return yamlContent.ToString();
    }

    public override string ToString() {
        var content = new StringBuilder();

        content.Append(string.Format("POIGroup[spawn:{0} clump:{1} dupe:{2} min:{3} max:{4}]: ",
                                     this.numToSpawn, this.avoidClumping, this.allowDupes,
                                     this.minRadius, this.maxRadius));
        foreach (var poi in this.poiList) {
            content.Append(poi);
            content.Append(", ");
        }

        return content.ToString();
    }
}
