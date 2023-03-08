// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;
using System.Collections.Generic;

namespace CGSM;

public enum StartPlanetoids {
    // keep values consistent w/ WarpPlanetoids & OtherPlanetoids
    [Option("STRINGS.WORLDS.MINIMETALLICSWAMPY.NAME", "STRINGS.WORLDS.MINIMETALLICSWAMPY.DESCRIPTION")]
    MetallicSwampy = 0,
    [Option("STRINGS.WORLDS.MINIBADLANDS.NAME", "STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION")]
    Desolands = 1,
    [Option("STRINGS.WORLDS.MINIFORESTFROZEN.NAME", "STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION")]
    FrozenForest = 2,
    [Option("STRINGS.WORLDS.MINIFLIPPED.NAME", "STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION")]
    Flipped = 3,
    [Option("STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME", "STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION")]
    RadioactiveOcean = 4,
    [Option("STRINGS.WORLDS.CGSM.TUNDRAMOONLET_NAME", "STRINGS.WORLDS.CGSM.TUNDRAMOONLET_DESC")]
    Tundra = 5,
    [Option("STRINGS.WORLDS.CGSM.MARSHYMOONLET_NAME", "STRINGS.WORLDS.CGSM.MARSHYMOONLET_DESC")]
    Marshy = 6,
    [Option("STRINGS.WORLDS.TERRAMOONLET.NAME", "STRINGS.WORLDS.TERRAMOONLET.DESCRIPTION")]
    Terrania = 11,
    [Option("STRINGS.WORLDS.FORESTMOONLET.NAME", "STRINGS.WORLDS.FORESTMOONLET.DESCRIPTION")]
    Folia = 14,
    [Option("STRINGS.WORLDS.SWAMPMOONLET.NAME", "STRINGS.WORLDS.SWAMPMOONLET.DESCRIPTION")]
    Quagmiris = 17,
    [Option("STRINGS.WORLDS.VANILLAOASIS.NAME", "STRINGS.WORLDS.VANILLAOASIS.DESCRIPTION")]
    Oasisse = 19,
    [Option("STRINGS.WORLDS.VANILLAARIDIO.NAME", "STRINGS.WORLDS.VANILLAARIDIO.DESCRIPTION")]
    Aridio = 21,
    [Option("STRINGS.WORLDS.VANILLABADLANDS.NAME", "STRINGS.WORLDS.VANILLABADLANDS.DESCRIPTION")]
    Badlands = 23,
    [Option("STRINGS.WORLDS.VANILLAVOLCANIC.NAME", "STRINGS.WORLDS.VANILLAVOLCANIC.DESCRIPTION")]
    Volcanea = 24,
    [Option("STRINGS.WORLDS.VANILLAARBORIA.NAME", "STRINGS.WORLDS.VANILLAARBORIA.DESCRIPTION")]
    Arboria = 25,
    [Option("STRINGS.WORLDS.VANILLAFORESTDEFAULT.NAME", "STRINGS.WORLDS.VANILLAFORESTDEFAULT.DESCRIPTION")]
    Verdante = 27,
    [Option("STRINGS.WORLDS.VANILLASANDSTONEFROZEN.NAME", "STRINGS.WORLDS.VANILLASANDSTONEFROZEN.DESCRIPTION")]
    Rime = 28,
    [Option("STRINGS.WORLDS.VANILLASWAMPDEFAULT.NAME", "STRINGS.WORLDS.VANILLASWAMPDEFAULT.DESCRIPTION")]
    Squelchy = 30,
    [Option("STRINGS.WORLDS.VANILLAOCEANIA.NAME", "STRINGS.WORLDS.VANILLAOCEANIA.DESCRIPTION")]
    Oceania = 32,
    [Option("STRINGS.WORLDS.VANILLASANDSTONEDEFAULT.NAME", "STRINGS.WORLDS.VANILLASANDSTONEDEFAULT.DESCRIPTION")]
    TerraVanilla = 34, // "vanilla" appended to help differentiate w/ Terrania
};

public enum WarpPlanetoids {
    // keep values consistent w/ StartPlanetoids & OtherPlanetoids
    [Option("STRINGS.WORLDS.MINIBADLANDS.NAME", "STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION")]
    Desolands = 1,
    [Option("STRINGS.WORLDS.MINIFORESTFROZEN.NAME", "STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION")]
    FrozenForest = 2,
    [Option("STRINGS.WORLDS.MINIFLIPPED.NAME", "STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION")]
    Flipped = 3,
    [Option("STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME", "STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION")]
    RadioactiveOcean = 4,
    [Option("STRINGS.WORLDS.WARPOILYSWAMP.NAME", "STRINGS.WORLDS.WARPOILYSWAMP.DESCRIPTION")]
    OilySwamp = 12,
    [Option("STRINGS.WORLDS.OILRICHWARPTARGET.NAME", "STRINGS.WORLDS.OILRICHWARPTARGET.DESCRIPTION")]
    RustyOil = 15,
    [Option("STRINGS.WORLDS.MEDIUMRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveSwamp = 20,
    [Option("STRINGS.WORLDS.MEDIUMSANDYSWAMP.NAME", "STRINGS.WORLDS.MEDIUMSANDYSWAMP.DESCRIPTION")]
    RadioactiveTerrabog = 22,
    [Option("STRINGS.WORLDS.MEDIUMSANDYRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMSANDYRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveTerra = 26,
    [Option("STRINGS.WORLDS.MEDIUMSWAMPY.NAME", "STRINGS.WORLDS.MEDIUMSWAMPY.DESCRIPTION")]
    StinkoSwamp = 29,
    [Option("STRINGS.WORLDS.MEDIUMFORESTYRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMFORESTYRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveForest = 31,
    [Option("STRINGS.WORLDS.MEDIUMFORESTYWASTELAND.NAME", "STRINGS.WORLDS.MEDIUMFORESTYWASTELAND.DESCRIPTION")]
    GlowoodWasteland = 33,
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
    Regolith = 10,
    IrradiatedForest = 13,
    IrradiatedSwamp = 16,
    IrradiatedMarsh = 18,
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
        {StartPlanetoids.Tundra, "worlds/CGSM.TundraMoonletStart"},
        {StartPlanetoids.Marshy, "worlds/CGSM.MarshyMoonletStart"},
        {StartPlanetoids.Terrania, "expansion1::worlds/TerraMoonlet"},
        {StartPlanetoids.Folia, "expansion1::worlds/ForestMoonlet"},
        {StartPlanetoids.Quagmiris, "expansion1::worlds/SwampMoonlet"},
        {StartPlanetoids.Oasisse, "expansion1::worlds/VanillaOasis"},
        {StartPlanetoids.Aridio, "expansion1::worlds/VanillaAridio"},
        {StartPlanetoids.Badlands, "expansion1::worlds/VanillaBadlands"},
        {StartPlanetoids.Volcanea, "expansion1::worlds/VanillaVolcanic"},
        {StartPlanetoids.Arboria, "expansion1::worlds/VanillaArboria"},
        {StartPlanetoids.Verdante, "expansion1::worlds/VanillaForestDefault"},
        {StartPlanetoids.Rime, "expansion1::worlds/VanillaSandstoneFrozen"},
        {StartPlanetoids.Squelchy, "expansion1::worlds/VanillaSwampDefault"},
        {StartPlanetoids.Oceania, "expansion1::worlds/VanillaOceania"},
        {StartPlanetoids.TerraVanilla, "expansion1::worlds/VanillaSandstoneDefault"},
    };

    private static Dictionary<WarpPlanetoids, string> wpMap = new Dictionary<WarpPlanetoids, string>
    {
        {WarpPlanetoids.Desolands, "expansion1::worlds/MiniBadlandsWarp"},
        {WarpPlanetoids.FrozenForest, "expansion1::worlds/MiniForestFrozenWarp"},
        {WarpPlanetoids.Flipped, "expansion1::worlds/MiniFlippedWarp"},
        {WarpPlanetoids.RadioactiveOcean, "expansion1::worlds/MiniRadioactiveOceanWarp"},
        {WarpPlanetoids.OilySwamp, "expansion1::worlds/WarpOilySwamp"},
        {WarpPlanetoids.RustyOil, "expansion1::worlds/OilRichWarpTarget"},
        {WarpPlanetoids.RadioactiveSwamp, "expansion1::worlds/MediumRadioactiveVanillaWarpPlanet"},
        {WarpPlanetoids.RadioactiveTerrabog, "expansion1::worlds/MediumSandySwamp"},
        {WarpPlanetoids.RadioactiveTerra, "expansion1::worlds/MediumSandyRadioactiveVanillaWarpPlanet"},
        {WarpPlanetoids.StinkoSwamp, "expansion1::worlds/MediumSwampy"},
        {WarpPlanetoids.RadioactiveForest, "expansion1::worlds/MediumForestyRadioactiveVanillaWarpPlanet"},
        {WarpPlanetoids.GlowoodWasteland, "expansion1::worlds/MediumForestyWasteland"},
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
        {OtherPlanetoids.IrradiatedForest, "expansion1::worlds/IdealLandingSite"},
        {OtherPlanetoids.IrradiatedSwamp, "expansion1::worlds/SwampyLandingSite"},
        {OtherPlanetoids.IrradiatedMarsh, "expansion1::worlds/MetalHeavyLandingSite"},
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
