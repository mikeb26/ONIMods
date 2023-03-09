// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

namespace CGSM;

public enum HarvestablePOIs {
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
    SwampyOreField
};

public static class POI {
    public static string ToYamlString(HarvestablePOIs poi) {
        return string.Format("HarvestableSpacePOI_{0}", poi.ToString());
    }
}
