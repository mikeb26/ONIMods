// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.OptionsFilt;
using System.Collections.Generic;
using System.Text;

namespace CGSM;

public enum PlanetoidType {
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
    Terrania = 11,
    OilySwamp = 12,
    IrradiatedForest = 13,
    Folia = 14,
    RustyOil = 15,
    IrradiatedSwamp = 16,
    Quagmiris = 17,
    IrradiatedMarsh = 18,
    Oasisse = 19,
    RadioactiveSwamp = 20,
    Aridio = 21,
    RadioactiveTerrabog = 22,
    Badlands = 23,
    Volcanea = 24,
    Arboria = 25,
    RadioactiveTerra = 26,
    Verdante = 27,
    Rime = 28,
    StinkoSwamp = 29,
    Squelchy = 30,
    RadioactiveForest = 31,
    Oceania = 32,
    GlowoodWasteland = 33,
    TerraVanilla = 34, // "vanilla" appended to help differentiate w/ Terrania
    MiniRegolith = 35,
    Baator = 36,
    BaatorOilySwampy = 37,
    BaatorMoonlet = 38,
    BaatorColdTerra = 39,
    Fuleria = 40,
    Tetrament = 41,
    TetramentVanilla = 42,
    DryRadioactiveForest = 43,
    MarshySnakes = 44,
    SuperconductiveSnakes = 45,
    WaterSnakes = 46,
    MiniBase = 47,
    MiniBaseOily = 48,
    MiniBaseMarshy = 49,
    MiniBaseNiobium = 50,
    Skewed = 51,
};

// Baator introduces 1-way warp planetoids that don't fit cleanly into these existing categories;
// for now classify them as Other
public enum PlanetoidCategory {
    Start,
    Warp,
    Other
}

public enum StartPlanetoidType {
    [Option("STRINGS.WORLDS.MINIMETALLICSWAMPY.NAME", "STRINGS.WORLDS.MINIMETALLICSWAMPY.DESCRIPTION")]
    MetallicSwampy = PlanetoidType.MetallicSwampy,
    [Option("STRINGS.WORLDS.MINIBADLANDS.NAME", "STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION")]
    Desolands = PlanetoidType.Desolands,
    [Option("STRINGS.WORLDS.MINIFORESTFROZEN.NAME", "STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION")]
    FrozenForest = PlanetoidType.FrozenForest,
    [Option("STRINGS.WORLDS.MINIFLIPPED.NAME", "STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION")]
    Flipped = PlanetoidType.Flipped,
    [Option("STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME", "STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION")]
    RadioactiveOcean = PlanetoidType.RadioactiveOcean,
    [Option("STRINGS.WORLDS.CGSM.TUNDRAMOONLET_NAME", "STRINGS.WORLDS.CGSM.TUNDRAMOONLET_DESC")]
    Tundra = PlanetoidType.Tundra,
    [Option("STRINGS.WORLDS.CGSM.MARSHYMOONLET_NAME", "STRINGS.WORLDS.CGSM.MARSHYMOONLET_DESC")]
    Marshy = PlanetoidType.Marshy,
    [Option("STRINGS.WORLDS.TERRAMOONLET.NAME", "STRINGS.WORLDS.TERRAMOONLET.DESCRIPTION")]
    Terrania = PlanetoidType.Terrania,
    [Option("STRINGS.WORLDS.FORESTMOONLET.NAME", "STRINGS.WORLDS.FORESTMOONLET.DESCRIPTION")]
    Folia = PlanetoidType.Folia,
    [Option("STRINGS.WORLDS.SWAMPMOONLET.NAME", "STRINGS.WORLDS.SWAMPMOONLET.DESCRIPTION")]
    Quagmiris = PlanetoidType.Quagmiris,
    [Option("STRINGS.WORLDS.VANILLAOASIS.NAME", "STRINGS.WORLDS.VANILLAOASIS.DESCRIPTION")]
    Oasisse = PlanetoidType.Oasisse,
    [Option("STRINGS.WORLDS.VANILLAARIDIO.NAME", "STRINGS.WORLDS.VANILLAARIDIO.DESCRIPTION")]
    Aridio = PlanetoidType.Aridio,
    [Option("STRINGS.WORLDS.VANILLABADLANDS.NAME", "STRINGS.WORLDS.VANILLABADLANDS.DESCRIPTION")]
    Badlands = PlanetoidType.Badlands,
    [Option("STRINGS.WORLDS.VANILLAVOLCANIC.NAME", "STRINGS.WORLDS.VANILLAVOLCANIC.DESCRIPTION")]
    Volcanea = PlanetoidType.Volcanea,
    [Option("STRINGS.WORLDS.VANILLAARBORIA.NAME", "STRINGS.WORLDS.VANILLAARBORIA.DESCRIPTION")]
    Arboria = PlanetoidType.Arboria,
    [Option("STRINGS.WORLDS.VANILLAFORESTDEFAULT.NAME", "STRINGS.WORLDS.VANILLAFORESTDEFAULT.DESCRIPTION")]
    Verdante = PlanetoidType.Verdante,
    [Option("STRINGS.WORLDS.VANILLASANDSTONEFROZEN.NAME", "STRINGS.WORLDS.VANILLASANDSTONEFROZEN.DESCRIPTION")]
    Rime = PlanetoidType.Rime,
    [Option("STRINGS.WORLDS.VANILLASWAMPDEFAULT.NAME", "STRINGS.WORLDS.VANILLASWAMPDEFAULT.DESCRIPTION")]
    Squelchy = PlanetoidType.Squelchy,
    [Option("STRINGS.WORLDS.VANILLAOCEANIA.NAME", "STRINGS.WORLDS.VANILLAOCEANIA.DESCRIPTION")]
    Oceania = PlanetoidType.Oceania,
    [Option("STRINGS.WORLDS.VANILLASANDSTONEDEFAULT.NAME", "STRINGS.WORLDS.VANILLASANDSTONEDEFAULT.DESCRIPTION")]
    TerraVanilla = PlanetoidType.TerraVanilla,
    [Option("STRINGS.WORLDS.BAATOR.NAME", "STRINGS.WORLDS.BAATOR.DESCRIPTION")]
    [RequireMod("Baator_BumminsMod")]
    Baator = PlanetoidType.Baator,
    [Option("STRINGS.WORLDS.BAATORMOONLET.NAME", "STRINGS.WORLDS.BAATORMOONLET.DESCRIPTION")]
    [RequireMod("Baator_BumminsMod")]
    BaatorMoonlet = PlanetoidType.BaatorMoonlet,
    [Option("STRINGS.WORLDS.FULERIA.NAME", "STRINGS.WORLDS.FULERIA.DESCRIPTION")]
    [RequireMod("AllBiomesWorld")]
    Fuleria = PlanetoidType.Fuleria,
    [Option("STRINGS.WORLDS.CGSM.TETRAMENT_NAME", "STRINGS.WORLDS.CGSM.TETRAMENT_DESC")]
    [RequireMod("test447.RollerSnake")]
    Tetrament = PlanetoidType.Tetrament,
    [Option("STRINGS.WORLDS.CGSM.VANILLATETRAMENT_NAME", "STRINGS.WORLDS.CGSM.VANILLATETRAMENT_DESC")]
    [RequireMod("test447.RollerSnake")]
    VanillaTetrament = PlanetoidType.TetramentVanilla,
    [Option("STRINGS.WORLDS.STRANGE_ASTEROID_KF23.NAME", "STRINGS.WORLDS.STRANGE_ASTEROID_KF23.DESCRIPTION")]
    Skewed = PlanetoidType.Skewed,
};

public enum WarpPlanetoidType {
    [Option("STRINGS.WORLDS.MINIBADLANDS.NAME", "STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION")]
    Desolands = PlanetoidType.Desolands,
    [Option("STRINGS.WORLDS.MINIFORESTFROZEN.NAME", "STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION")]
    FrozenForest = PlanetoidType.FrozenForest,
    [Option("STRINGS.WORLDS.MINIFLIPPED.NAME", "STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION")]
    Flipped = PlanetoidType.Flipped,
    [Option("STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME", "STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION")]
    RadioactiveOcean = PlanetoidType.RadioactiveOcean,
    [Option("STRINGS.WORLDS.CGSM.TUNDRAMOONLET_NAME", "STRINGS.WORLDS.CGSM.TUNDRAMOONLET_DESC")]
    Tundra = PlanetoidType.Tundra,
    [Option("STRINGS.WORLDS.WARPOILYSWAMP.NAME", "STRINGS.WORLDS.WARPOILYSWAMP.DESCRIPTION")]
    OilySwamp = PlanetoidType.OilySwamp,
    [Option("STRINGS.WORLDS.OILRICHWARPTARGET.NAME", "STRINGS.WORLDS.OILRICHWARPTARGET.DESCRIPTION")]
    RustyOil = PlanetoidType.RustyOil,
    [Option("STRINGS.WORLDS.MEDIUMRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveSwamp = PlanetoidType.RadioactiveSwamp,
    [Option("STRINGS.WORLDS.MEDIUMSANDYSWAMP.NAME", "STRINGS.WORLDS.MEDIUMSANDYSWAMP.DESCRIPTION")]
    RadioactiveTerrabog = PlanetoidType.RadioactiveTerrabog,
    [Option("STRINGS.WORLDS.MEDIUMSANDYRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMSANDYRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveTerra = PlanetoidType.RadioactiveTerra,
    [Option("STRINGS.WORLDS.MEDIUMSWAMPY.NAME", "STRINGS.WORLDS.MEDIUMSWAMPY.DESCRIPTION")]
    StinkoSwamp = PlanetoidType.StinkoSwamp,
    [Option("STRINGS.WORLDS.MEDIUMFORESTYRADIOACTIVEVANILLAWARPPLANET.NAME", "STRINGS.WORLDS.MEDIUMFORESTYRADIOACTIVEVANILLAWARPPLANET.DESCRIPTION")]
    RadioactiveForest = PlanetoidType.RadioactiveForest,
    [Option("STRINGS.WORLDS.MEDIUMFORESTYWASTELAND.NAME", "STRINGS.WORLDS.MEDIUMFORESTYWASTELAND.DESCRIPTION")]
    GlowoodWasteland = PlanetoidType.GlowoodWasteland,
    [Option("STRINGS.WORLDS.TETRAMENT_WARP.NAME", "STRINGS.WORLDS.TETRAMENT_WARP.DESCRIPTION")]
    [RequireMod("test447.RollerSnake")]
    DryRadioactiveForest = PlanetoidType.DryRadioactiveForest,
    [Option("STRINGS.WORLDS.CGSM.MARSHYMOONLET_NAME", "STRINGS.WORLDS.CGSM.MARSHYMOONLET_DESC")]
    Marshy = PlanetoidType.Marshy,
};

/* captures dynamic planetoid properties not known until runtime that the player may be customizing
 * such as geysers, sunlight level, radiation level, meteors, etc
 */
public class Planetoid {
    private PlanetoidInfo info;
    public PlanetoidCategory category;

    public Planetoid(StartPlanetoidType startType) : this((PlanetoidType) startType, PlanetoidCategory.Start) {}
    public Planetoid(WarpPlanetoidType warpType) : this((PlanetoidType) warpType, PlanetoidCategory.Warp) {}
    public Planetoid(PlanetoidType typeIn, PlanetoidCategory categoryIn) {
        category = categoryIn;
        info = PlanetoidInfos.lookup(typeIn);
    }

    public string ToYamlString() {
        return info.yamlMap[category];
    }

    public PlanetoidType Type() {
        return info.type;
    }

    public override string ToString() {
        return string.Format("{0}/{1}", this.Type(), this.category);
    }
}

/* captures static planetoid properties known at compile time */
public class PlanetoidInfo {
    public readonly PlanetoidType type;
    public readonly Dictionary<PlanetoidCategory, string> yamlMap;

    public PlanetoidInfo(PlanetoidType typeIn, Dictionary<PlanetoidCategory, string> yamlMapIn) {
        type = typeIn;
        yamlMap = yamlMapIn;
    }
}

public static class PlanetoidInfos {
    private static readonly Dictionary<PlanetoidType, PlanetoidInfo> infos =
        new Dictionary<PlanetoidType, PlanetoidInfo>{
            {PlanetoidType.MetallicSwampy, new PlanetoidInfo(PlanetoidType.MetallicSwampy,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/MiniMetallicSwampyStart"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniMetallicSwampy"},
                })},
            {PlanetoidType.Desolands, new PlanetoidInfo(PlanetoidType.Desolands,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/MiniBadlandsStart"},
                    {PlanetoidCategory.Warp, "expansion1::worlds/MiniBadlandsWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniBadlands"},
                })},
            {PlanetoidType.FrozenForest, new PlanetoidInfo(PlanetoidType.FrozenForest,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/MiniForestFrozenStart"},
                    {PlanetoidCategory.Warp, "expansion1::worlds/MiniForestFrozenWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniForestFrozen"},
                })},
            {PlanetoidType.Flipped, new PlanetoidInfo(PlanetoidType.Flipped,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/MiniFlippedStart"},
                    {PlanetoidCategory.Warp, "expansion1::worlds/MiniFlippedWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniFlipped"},
                })},
            {PlanetoidType.RadioactiveOcean, new PlanetoidInfo(PlanetoidType.RadioactiveOcean,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/MiniRadioactiveOceanStart"},
                    {PlanetoidCategory.Warp, "expansion1::worlds/MiniRadioactiveOceanWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniRadioactiveOcean"},
                })},
            {PlanetoidType.Tundra, new PlanetoidInfo(PlanetoidType.Tundra,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "worlds/CGSM.TundraMoonletStart"},
                    {PlanetoidCategory.Warp, "worlds/CGSM.TundraMoonletWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/TundraMoonlet"},
                })},
            {PlanetoidType.Marshy, new PlanetoidInfo(PlanetoidType.Marshy,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "worlds/CGSM.MarshyMoonletStart"},
                    {PlanetoidCategory.Warp, "worlds/CGSM.MarshyMoonletWarp"},
                    {PlanetoidCategory.Other, "expansion1::worlds/MarshyMoonlet"},
                })},
            {PlanetoidType.Moo, new PlanetoidInfo(PlanetoidType.Moo,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/MooMoonlet"},
                })},
            {PlanetoidType.Water, new PlanetoidInfo(PlanetoidType.Water,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/WaterMoonlet"},
                })},
            {PlanetoidType.Superconductive, new PlanetoidInfo(PlanetoidType.Superconductive,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/NiobiumMoonlet"},
                })},
            {PlanetoidType.Regolith, new PlanetoidInfo(PlanetoidType.Regolith,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/RegolithMoonlet"},
                })},
            {PlanetoidType.Terrania, new PlanetoidInfo(PlanetoidType.Terrania,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/TerraMoonlet"},
                })},
            {PlanetoidType.OilySwamp, new PlanetoidInfo(PlanetoidType.OilySwamp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/WarpOilySwamp"},
                })},
            {PlanetoidType.IrradiatedForest, new PlanetoidInfo(PlanetoidType.IrradiatedForest,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/IdealLandingSite"},
                })},
            {PlanetoidType.Folia, new PlanetoidInfo(PlanetoidType.Folia,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/ForestMoonlet"},
                })},
            {PlanetoidType.RustyOil, new PlanetoidInfo(PlanetoidType.RustyOil,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/OilRichWarpTarget"},
                })},
            {PlanetoidType.IrradiatedSwamp, new PlanetoidInfo(PlanetoidType.IrradiatedSwamp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/SwampyLandingSite"},
                })},
            {PlanetoidType.Quagmiris, new PlanetoidInfo(PlanetoidType.Quagmiris,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/SwampMoonlet"},
                })},
            {PlanetoidType.IrradiatedMarsh, new PlanetoidInfo(PlanetoidType.IrradiatedMarsh,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/MetalHeavyLandingSite"},
                })},
            {PlanetoidType.Oasisse, new PlanetoidInfo(PlanetoidType.Oasisse,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaOasis"},
                })},
            {PlanetoidType.RadioactiveSwamp, new PlanetoidInfo(PlanetoidType.RadioactiveSwamp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumRadioactiveVanillaWarpPlanet"},
                })},
            {PlanetoidType.Aridio, new PlanetoidInfo(PlanetoidType.Aridio,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaAridio"},
                })},
            {PlanetoidType.RadioactiveTerrabog, new PlanetoidInfo(PlanetoidType.RadioactiveTerrabog,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumSandySwamp"},
                })},
            {PlanetoidType.Badlands, new PlanetoidInfo(PlanetoidType.Badlands,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaBadlands"},
                })},
            {PlanetoidType.Volcanea, new PlanetoidInfo(PlanetoidType.Volcanea,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaVolcanic"},
                })},
            {PlanetoidType.Arboria, new PlanetoidInfo(PlanetoidType.Arboria,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaArboria"},
                })},
            {PlanetoidType.RadioactiveTerra, new PlanetoidInfo(PlanetoidType.RadioactiveTerra,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumSandyRadioactiveVanillaWarpPlanet"},
                })},
            {PlanetoidType.Verdante, new PlanetoidInfo(PlanetoidType.Verdante,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaForestDefault"},
                })},
            {PlanetoidType.Rime, new PlanetoidInfo(PlanetoidType.Rime,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaSandstoneFrozen"},
                })},
            {PlanetoidType.StinkoSwamp, new PlanetoidInfo(PlanetoidType.StinkoSwamp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumSwampy"},
                })},
            {PlanetoidType.Squelchy, new PlanetoidInfo(PlanetoidType.Squelchy,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaSwampDefault"},
                })},
            {PlanetoidType.RadioactiveForest, new PlanetoidInfo(PlanetoidType.RadioactiveForest,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumForestyRadioactiveVanillaWarpPlanet"},
                })},
            {PlanetoidType.Oceania, new PlanetoidInfo(PlanetoidType.Oceania,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaOceania"},
                })},
            {PlanetoidType.GlowoodWasteland, new PlanetoidInfo(PlanetoidType.GlowoodWasteland,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/MediumForestyWasteland"},
                })},
            {PlanetoidType.TerraVanilla, new PlanetoidInfo(PlanetoidType.TerraVanilla,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/VanillaSandstoneDefault"},
                })},
            {PlanetoidType.MiniRegolith, new PlanetoidInfo(PlanetoidType.Regolith,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/MiniRegolithMoonlet"},
                })},
            {PlanetoidType.Baator, new PlanetoidInfo(PlanetoidType.Baator,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/Baator"},
                })},
            {PlanetoidType.BaatorOilySwampy, new PlanetoidInfo(PlanetoidType.BaatorOilySwampy,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/Baator_oilyswampy"},
                })},
            {PlanetoidType.BaatorMoonlet, new PlanetoidInfo(PlanetoidType.BaatorMoonlet,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/Baator_moonlet"},
                })},
            {PlanetoidType.BaatorColdTerra, new PlanetoidInfo(PlanetoidType.BaatorColdTerra,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "expansion1::worlds/Baator_coldterra"},
                })},
            {PlanetoidType.Fuleria, new PlanetoidInfo(PlanetoidType.Fuleria,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/FuleriaDLC"},
                })},
            {PlanetoidType.Tetrament, new PlanetoidInfo(PlanetoidType.Tetrament,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "worlds/TetramentStart"},
                })},
            {PlanetoidType.TetramentVanilla, new PlanetoidInfo(PlanetoidType.TetramentVanilla,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "worlds/VanillaTetrament"},
                })},
            {PlanetoidType.DryRadioactiveForest, new PlanetoidInfo(PlanetoidType.DryRadioactiveForest,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "worlds/TetramentClusterWarpPlanet"},
                })},
            {PlanetoidType.MarshySnakes, new PlanetoidInfo(PlanetoidType.MarshySnakes,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/MarshyMoonletAlt"},
                })},
            {PlanetoidType.SuperconductiveSnakes, new PlanetoidInfo(PlanetoidType.SuperconductiveSnakes,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/NiobiumMoonletFixed"},
                })},
            {PlanetoidType.WaterSnakes, new PlanetoidInfo(PlanetoidType.WaterSnakes,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/WaterMoonletHope"},
                })},
            {PlanetoidType.MiniBase, new PlanetoidInfo(PlanetoidType.MiniBase,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/MiniBase"},
                })},
            {PlanetoidType.MiniBaseOily, new PlanetoidInfo(PlanetoidType.MiniBaseOily,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/BabyOilyMoonlet"},
                })},
            {PlanetoidType.MiniBaseMarshy, new PlanetoidInfo(PlanetoidType.MiniBaseMarshy,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/BabyMarshyMoonlet"},
                })},
            {PlanetoidType.MiniBaseNiobium, new PlanetoidInfo(PlanetoidType.MiniBaseNiobium,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "worlds/BabyNiobiumMoonlet"},
                })},
            {PlanetoidType.Skewed, new PlanetoidInfo(PlanetoidType.Skewed,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "expansion1::worlds/StrangeAsteroidKleiFest2023Cluster"},
                })},
        };

    public static PlanetoidInfo lookup(PlanetoidType planetoidType) {
        return infos[planetoidType];
    }
}

public class PlanetoidPlacement {
    public Planetoid planetoid { get; }
    public int minRadius { get; set; }
    public int maxRadius { get; set; }
    public int buffer { get; set; }
    public bool isInner { get; set; }

    public PlanetoidPlacement(StartPlanetoidType startType, int bufferIn) {
        this.planetoid = new Planetoid(startType);
        initCommon(0, 0, bufferIn, false);
    }
    public PlanetoidPlacement(StartPlanetoidType startType, int bufferIn, int maxRadiusIn) {
        this.planetoid = new Planetoid(startType);
        initCommon(0, maxRadiusIn, bufferIn, false);
    }
    public PlanetoidPlacement(StartPlanetoidType startType, int bufferIn, int minRadiusIn,
                              int maxRadiusIn) {
        this.planetoid = new Planetoid(startType);
        initCommon(minRadiusIn, maxRadiusIn, bufferIn, false);
    }
    public PlanetoidPlacement(WarpPlanetoidType warpType, int bufferIn, int minRadiusIn,
                              int maxRadiusIn) {
        this.planetoid = new Planetoid(warpType);
        initCommon(minRadiusIn, maxRadiusIn, bufferIn, true);
    }
    public PlanetoidPlacement(PlanetoidType pType, PlanetoidCategory pCat, int bufferIn,
                              int minRadiusIn, int maxRadiusIn, bool isInnerIn) {
        this.planetoid = new Planetoid(pType, pCat);
        initCommon(minRadiusIn, maxRadiusIn, bufferIn, isInnerIn);
    }
    public PlanetoidPlacement(Planetoid planetoidIn, int minRadiusIn, int maxRadiusIn,
                              int bufferIn, bool isInnerIn) {
        this.planetoid = planetoidIn;
        initCommon(minRadiusIn, maxRadiusIn, bufferIn, isInnerIn);
    }
    private void initCommon(int minRadiusIn, int maxRadiusIn, int bufferIn, bool isInnerIn) {
        this.minRadius = minRadiusIn;
        this.maxRadius = maxRadiusIn;
        this.buffer = bufferIn;
        this.isInner = isInnerIn;
    }

    public string ToYamlString() {
        var yamlContent = new StringBuilder();

        yamlContent.Append(string.Format("- world: {0}\n", this.planetoid.ToYamlString()));
        yamlContent.Append(string.Format("  buffer: {0}\n", this.buffer));
        if (this.planetoid.category == PlanetoidCategory.Start) {
            yamlContent.Append("  locationType: StartWorld\n");
        } else if (this.isInner) {
            yamlContent.Append("  locationType: InnerCluster\n");
        }
        yamlContent.Append("  allowedRings:\n");
        yamlContent.Append(string.Format("    min: {0}\n", this.minRadius));
        yamlContent.Append(string.Format("    max: {0}\n", this.maxRadius));

        return yamlContent.ToString();
    }

    public override string ToString() {
        return string.Format("{0} min:{1} max:{2} buf:{3} inner:{4}", this.planetoid,
                             this.minRadius, this.maxRadius, this.buffer, this.isInner);
    }

}
