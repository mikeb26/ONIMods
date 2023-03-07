// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;

namespace CGSM;

public enum StartPlanetoids {
    // keep values consistent w/ WarpPlanetoids & OtherPlanetoids
    MetallicSwampy = 0,
    Desolands = 1,
    FrozenForest = 2,
    Flipped = 3,
    RadioactiveOcean = 4
};

public enum WarpPlanetoids {
    // keep values consistent w/ StartPlanetoids & OtherPlanetoids
    Desolands = 1,
    FrozenForest = 2,
    Flipped = 3,
    RadioactiveOcean = 4
};

public enum OtherPlanetoids {
    // keep values consistent w/ StartPlanetoids & WarpPlanetoids
    MetallicSwampy = 0,
    Desolands = 1,
    FrozenForest = 2,
    Flipped = 3,
    RadioactiveOcean = 4,
    Tundra = 5,
    Marshy = 6,
    Moo = 7,
    Water = 8,
    Superconductive = 9,
    Regolith = 10
};

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

public static class Enum {
    private static Dictionary<StartPlanetoids, string> spMap = new Dictionary<StartPlanetoids, string>
    {
        {StartPlanetoids.MetallicSwampy, "expansion1::worlds/MiniMetallicSwampyStart"},
        {StartPlanetoids.Desolands, "expansion1::worlds/MiniBadlandsStart"},
        {StartPlanetoids.FrozenForest, "expansion1::worlds/MiniForestFrozenStart"},
        {StartPlanetoids.Flipped, "expansion1::worlds/MiniFlippedStart"},
        {StartPlanetoids.RadioactiveOcean, "expansion1::worlds/MiniRadioactiveOceanStart"},
    };

    private static Dictionary<WarpPlanetoids, string> wpMap = new Dictionary<WarpPlanetoids, string>
    {
        {WarpPlanetoids.Desolands, "expansion1::worlds/MiniBadlandsWarp"},
        {WarpPlanetoids.FrozenForest, "expansion1::worlds/MiniForestFrozenWarp"},
        {WarpPlanetoids.Flipped, "expansion1::worlds/MiniFlippedWarp"},
        {WarpPlanetoids.RadioactiveOcean, "expansion1::worlds/MiniRadioactiveOceanWarp"},
    };

    private static Dictionary<OtherPlanetoids, string> opMap = new Dictionary<OtherPlanetoids, string>
    {
        {OtherPlanetoids.MetallicSwampy, "expansion1::worlds/MiniMetallicSwampy"},
        {OtherPlanetoids.Desolands, "expansion1::worlds/MiniBadlands"},
        {OtherPlanetoids.FrozenForest, "expansion1::worlds/MiniForestFrozen"},
        {OtherPlanetoids.Flipped, "expansion1::worlds/MiniFlipped"},
        {OtherPlanetoids.RadioactiveOcean, "expansion1::worlds/MiniRadioactiveOcean"},
        {OtherPlanetoids.Tundra, "expansion1::worlds/TundraMoonlet"},
        {OtherPlanetoids.Marshy, "expansion1::worlds/MarshyMoonlet"},
        {OtherPlanetoids.Moo, "expansion1::worlds/MooMoonlet"},
        {OtherPlanetoids.Water, "expansion1::worlds/WaterMoonlet"},
        {OtherPlanetoids.Superconductive, "expansion1::worlds/NiobiumMoonlet"},
        {OtherPlanetoids.Regolith, "expansion1::worlds/RegolithMoonlet"},
    };

    public static string planetoidToYamlString(StartPlanetoids planetoid) {
        return spMap[planetoid];
    }
    public static string planetoidToYamlString(WarpPlanetoids planetoid) {
        return wpMap[planetoid];
    }
    public static string planetoidToYamlString(OtherPlanetoids planetoid) {
        return opMap[planetoid];
    }
    public static string poiToYamlString(HarvestablePOIs poi) {
        return string.Format("HarvestableSpacePOI_{0}", poi.ToString());
    }
}
