// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CGSM;

public static class Planetoids
{
    // Some planetoids have multiple "sizes" / variants that should be treated as equivalent
    // for the purposes of world mixing.
    //
    // In particular, Regolith and MiniRegolith should be considered interchangeable: if a
    // cluster already contains one, it should satisfy mixing settings for the other.
    public static PlanetoidType NormalizeForMixing(PlanetoidType type)
    {
        return type switch
        {
            PlanetoidType.MiniRegolith => PlanetoidType.Regolith,
            PlanetoidType.Regolith => PlanetoidType.Regolith,
            _ => type,
        };
    }

    public static bool AreMixingEquivalent(PlanetoidType a, PlanetoidType b)
        => NormalizeForMixing(a) == NormalizeForMixing(b);

    public static bool IsStartWorld(ProcGen.WorldPlacement wp, string startWorldPath)
    {
        return (string.Equals(wp.world, startWorldPath, StringComparison.OrdinalIgnoreCase))
               || wp.locationType == ProcGen.WorldPlacement.LocationType.Startworld
               || wp.startWorld;
    }

    public static bool IsWarpWorld(string worldPath)
    {
        if (PlanetoidInfos.TryLookupCategoryByWorldPath(worldPath, out var category))
            return category == PlanetoidCategory.Warp;

        // Fallback
        return worldPath.IndexOf("Warp", StringComparison.OrdinalIgnoreCase) >= 0
               || worldPath.IndexOf("Teleport", StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
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
    // DLC2
    CeresSpacedOut = 52,
    CeresClassic = 53,
    CeresClassicShattered = 54,
    MiniShatteredStart = 55,
    MiniShatteredWarp = 56,
    MiniShatteredGeo = 57,
    // DLC4
    PrehistoricSpacedOut = 58,
    PrehistoricClassic = 59,
    PrehistoricShatteredClassic = 60,
    WarpOilySandySwamp = 61,
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

    // DLC2
    [Option("STRINGS.WORLDS.CERESSPACEDOUT.NAME", "STRINGS.WORLDS.CERESSPACEDOUT.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC2_ID)]
    CeresSpacedOut = PlanetoidType.CeresSpacedOut,
    [Option("STRINGS.WORLDS.CERESCLASSIC.NAME", "STRINGS.WORLDS.CERESCLASSIC.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC2_ID)]
    CeresClassic = PlanetoidType.CeresClassic,
    [Option("STRINGS.WORLDS.CERESCLASSICSHATTERED.NAME", "STRINGS.WORLDS.CERESCLASSICSHATTERED.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC2_ID)]
    CeresClassicShattered = PlanetoidType.CeresClassicShattered,
    [Option("STRINGS.WORLDS.MINISHATTEREDSTART.NAME", "STRINGS.WORLDS.MINISHATTEREDSTART.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC2_ID)]
    MiniShatteredStart = PlanetoidType.MiniShatteredStart,

    // DLC4
    [Option("STRINGS.WORLDS.PREHISTORICSPACEDOUT.NAME", "STRINGS.WORLDS.PREHISTORICSPACEDOUT.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC4_ID)]
    PrehistoricSpacedOut = PlanetoidType.PrehistoricSpacedOut,
    [Option("STRINGS.WORLDS.PREHISTORICCLASSIC.NAME", "STRINGS.WORLDS.PREHISTORICCLASSIC.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC4_ID)]
    PrehistoricClassic = PlanetoidType.PrehistoricClassic,
    [Option("STRINGS.WORLDS.PREHISTORICSHATTERED.NAME", "STRINGS.WORLDS.PREHISTORICSHATTERED.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC4_ID)]
    PrehistoricShatteredClassic = PlanetoidType.PrehistoricShatteredClassic,
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

    // DLC2
    [Option("STRINGS.WORLDS.MINISHATTEREDWARP.NAME", "STRINGS.WORLDS.MINISHATTEREDWARP.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC2_ID)]
    MiniShatteredWarp = PlanetoidType.MiniShatteredWarp,

    // DLC4
    [Option("STRINGS.WORLDS.WARPOILYSANDYSWAMP.NAME", "STRINGS.WORLDS.WARPOILYSANDYSWAMP.DESCRIPTION")]
    [RequireDLC(DlcManager.DLC4_ID)]
    WarpOilySandySwamp = PlanetoidType.WarpOilySandySwamp,
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

    // If this planetoid requires an additional DLC beyond EXPANSION1, return the
    // corresponding DLC token (e.g. "DLC2_ID" or "DLC4_ID"). Otherwise null.
    public string RequiredDlcIdToken() {
        return info.requiredDlcIdToken;
    }

    public override string ToString() {
        return string.Format("{0}/{1}", this.Type(), this.category);
    }
}

/* captures static planetoid properties known at compile time */
public class PlanetoidInfo {
    public readonly PlanetoidType type;
    public readonly Dictionary<PlanetoidCategory, string> yamlMap;
    // DLC token (as used in Klei YAML headers), or null if none.
    public readonly string requiredDlcIdToken;

    public PlanetoidInfo(PlanetoidType typeIn, Dictionary<PlanetoidCategory, string> yamlMapIn,
                         string requiredDlcIdTokenIn = null) {
        type = typeIn;
        yamlMap = yamlMapIn;
        requiredDlcIdToken = requiredDlcIdTokenIn;
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
            {PlanetoidType.MiniRegolith, new PlanetoidInfo(PlanetoidType.MiniRegolith,
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

            // DLC2
            {PlanetoidType.CeresSpacedOut, new PlanetoidInfo(PlanetoidType.CeresSpacedOut,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc2::worlds/CeresSpacedOutAsteroid"},
                }, "DLC2_ID")},
            {PlanetoidType.CeresClassic, new PlanetoidInfo(PlanetoidType.CeresClassic,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc2::worlds/CeresClassicAsteroid"},
                }, "DLC2_ID")},
            {PlanetoidType.CeresClassicShattered, new PlanetoidInfo(PlanetoidType.CeresClassicShattered,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc2::worlds/CeresClassicShatteredAsteroid"},
                }, "DLC2_ID")},
            {PlanetoidType.MiniShatteredStart, new PlanetoidInfo(PlanetoidType.MiniShatteredStart,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc2::worlds/MiniShatteredStartAsteroid"},
                }, "DLC2_ID")},
            {PlanetoidType.MiniShatteredWarp, new PlanetoidInfo(PlanetoidType.MiniShatteredWarp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "dlc2::worlds/MiniShatteredWarpAsteroid"},
                }, "DLC2_ID")},
            {PlanetoidType.MiniShatteredGeo, new PlanetoidInfo(PlanetoidType.MiniShatteredGeo,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Other, "dlc2::worlds/MiniShatteredGeoAsteroid"},
                }, "DLC2_ID")},

            // DLC4
            {PlanetoidType.PrehistoricSpacedOut, new PlanetoidInfo(PlanetoidType.PrehistoricSpacedOut,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc4::worlds/PrehistoricSpacedOutAsteroid"},
                }, "DLC4_ID")},
            {PlanetoidType.PrehistoricClassic, new PlanetoidInfo(PlanetoidType.PrehistoricClassic,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc4::worlds/PrehistoricClassicAsteroid"},
                }, "DLC4_ID")},
            {PlanetoidType.PrehistoricShatteredClassic, new PlanetoidInfo(PlanetoidType.PrehistoricShatteredClassic,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Start, "dlc4::worlds/PrehistoricShatteredClassicAsteroid"},
                }, "DLC4_ID")},
            {PlanetoidType.WarpOilySandySwamp, new PlanetoidInfo(PlanetoidType.WarpOilySandySwamp,
                new Dictionary<PlanetoidCategory, string>{
                    {PlanetoidCategory.Warp, "expansion1::worlds/WarpOilySandySwamp"},
                }, "DLC4_ID")},
        };

    private static readonly Dictionary<string, PlanetoidCategory> worldPathToCategory = BuildWorldPathToCategoryIndex();
    private static readonly Dictionary<string, PlanetoidType> worldPathToType = BuildWorldPathToTypeIndex();

    private static Dictionary<string, PlanetoidCategory> BuildWorldPathToCategoryIndex()
    {
        var index = new Dictionary<string, PlanetoidCategory>(StringComparer.OrdinalIgnoreCase);

        foreach (var infoKvp in infos)
        {
            var info = infoKvp.Value;
            if (info?.yamlMap == null)
                continue;

            foreach (var mapKvp in info.yamlMap)
            {
                var category = mapKvp.Key;
                var path = mapKvp.Value;
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                // Index the raw path.
                index[path] = category;

                // Also index a no-content-prefix form to tolerate callers passing either
                // "expansion1::worlds/Foo" or "worlds/Foo".
                var noPrefix = StripContentPrefix(path);
                if (!string.IsNullOrWhiteSpace(noPrefix))
                    index[noPrefix] = category;
            }
        }

        return index;
    }

    private static Dictionary<string, PlanetoidType> BuildWorldPathToTypeIndex()
    {
        var index = new Dictionary<string, PlanetoidType>(StringComparer.OrdinalIgnoreCase);

        foreach (var infoKvp in infos)
        {
            var typeKey = infoKvp.Key;
            var info = infoKvp.Value;
            if (info?.yamlMap == null)
                continue;

            foreach (var mapKvp in info.yamlMap)
            {
                var path = mapKvp.Value;
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                // Index the raw path.
                index[path] = typeKey;

                // Also index a no-content-prefix form to tolerate callers passing either
                // "expansion1::worlds/Foo" or "worlds/Foo".
                var noPrefix = StripContentPrefix(path);
                if (!string.IsNullOrWhiteSpace(noPrefix))
                    index[noPrefix] = typeKey;
            }
        }

        return index;
    }

    private static string StripContentPrefix(string worldPath)
    {
        if (string.IsNullOrWhiteSpace(worldPath))
            return worldPath;

        int idx = worldPath.IndexOf("::", StringComparison.Ordinal);
        if (idx < 0)
            return worldPath;

        return worldPath.Substring(idx + 2);
    }

    public static bool TryLookupCategoryByWorldPath(string worldPath, out PlanetoidCategory category)
    {
        category = default;

        if (string.IsNullOrWhiteSpace(worldPath))
            return false;

        if (worldPathToCategory.TryGetValue(worldPath, out category))
            return true;

        var noPrefix = StripContentPrefix(worldPath);
        if (!string.IsNullOrWhiteSpace(noPrefix) && worldPathToCategory.TryGetValue(noPrefix, out category))
            return true;

        return false;
    }

    public static bool TryLookupTypeByWorldPath(string worldPath, out PlanetoidType planetoidType)
    {
        planetoidType = default;

        if (string.IsNullOrWhiteSpace(worldPath))
            return false;

        if (worldPathToType.TryGetValue(worldPath, out planetoidType))
            return true;

        var noPrefix = StripContentPrefix(worldPath);
        if (!string.IsNullOrWhiteSpace(noPrefix) && worldPathToType.TryGetValue(noPrefix, out planetoidType))
            return true;

        return false;
    }

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

        if (this.planetoid.category == PlanetoidCategory.Other) {
            // WorldMixing slots in Klei cluster YAMLs often have extra rules (templates, seasons,
            // subworlds, etc). We emit the common required tags plus any world-specific extras
            // (based on MiniClusterFlippedStart.yaml).
            yamlContent.Append("  worldMixing:\n");
            yamlContent.Append("    requiredTags:\n");
            yamlContent.Append("      - Mixing\n");

            switch (this.planetoid.Type()) {
                case PlanetoidType.Tundra:
                    yamlContent.Append("      - SmallWorld\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("          - expansion1::poi/poi_temporal_tear_opener_lab # temporal tear opener\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 201\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: Replace\n");
                    yamlContent.Append("            tagcommand: DistanceFromTag\n");
                    yamlContent.Append("            tag: AtSurface\n");
                    yamlContent.Append("            minDistance: 2\n");
                    yamlContent.Append("            maxDistance: 3\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("          - expansion1::poi/genericGravitas/poi_gift_shop # artifacts\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 200\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    break;

                case PlanetoidType.Marshy:
                    yamlContent.Append("      - SmallWorld\n");
                    yamlContent.Append("    forbiddenTags:\n");
                    yamlContent.Append("      - Challenge\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::poi/worldmixing/sap_tree_room # Experiment 52B\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 500\n");
                    yamlContent.Append("        allowExtremeTemperatureOverlap: true # has Abyssalite border\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::geysers/molten_tungsten_compact\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 150\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    break;

                case PlanetoidType.Moo:
                    yamlContent.Append("      - SmallWorld\n");
                    yamlContent.Append("    forbiddenTags:\n");
                    yamlContent.Append("      - NoExtraSeasons\n");
                    yamlContent.Append("      - ModifiedSurfaceHeight\n");
                    yamlContent.Append("      - SurfaceSubworldReserved\n");
                    yamlContent.Append("    additionalSubworldFiles:\n");
                    yamlContent.Append("      - name: expansion1::subworlds/moo/MooCaverns\n");
                    yamlContent.Append("        minCount: 2\n");
                    yamlContent.Append("    additionalUnknownCellFilters:\n");
                    yamlContent.Append("      - tagcommand: DistanceFromTag # surface\n");
                    yamlContent.Append("        tag: AtSurface\n");
                    yamlContent.Append("        minDistance: 2\n");
                    yamlContent.Append("        maxDistance: 2\n");
                    yamlContent.Append("        command: Replace\n");
                    yamlContent.Append("        sortOrder: 1000 # apply last so world traits and subworld mixing do not override it\n");
                    yamlContent.Append("        subworldNames:\n");
                    yamlContent.Append("          - expansion1::subworlds/moo/MooCaverns\n");
                    yamlContent.Append("    additionalSeasons:\n");
                    yamlContent.Append("      - GassyMooteorShowers\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::poi/genericGravitas/poi_genetics_lab # artifacts\n");
                    yamlContent.Append("        - geysers/chlorine_gas\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 150\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    break;

                case PlanetoidType.Water:
                    yamlContent.Append("      - SmallWorld\n");
                    yamlContent.Append("    forbiddenTags:\n");
                    yamlContent.Append("      - AboveCoreSubworldReserved\n");
                    yamlContent.Append("    additionalSubworldFiles:\n");
                    yamlContent.Append("      - name: expansion1::subworlds/aquatic/GraphiteCaves\n");
                    yamlContent.Append("        minCount: 2\n");
                    yamlContent.Append("    additionalUnknownCellFilters:\n");
                    yamlContent.Append("      - tagcommand: DistanceFromTag\n");
                    yamlContent.Append("        tag: AtDepths\n");
                    yamlContent.Append("        minDistance: 1\n");
                    yamlContent.Append("        maxDistance: 1\n");
                    yamlContent.Append("        command: Replace\n");
                    yamlContent.Append("        sortOrder: 1000 # apply last so world traits and subworld mixing do not override it\n");
                    yamlContent.Append("        subworldNames:\n");
                    yamlContent.Append("          - expansion1::subworlds/aquatic/GraphiteCaves\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::poi/genericGravitas/poi_thermo_building\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 450\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    break;

                case PlanetoidType.Superconductive:
                    yamlContent.Append("      - SmallWorld\n");
                    // Niobium requires the Challenge tag in addition to the common Mixing + SmallWorld.
                    yamlContent.Append("      - Challenge\n");
                    yamlContent.Append("    forbiddenTags:\n");
                    yamlContent.Append("      - AboveCoreSubworldReserved\n");
                    yamlContent.Append("    additionalSubworldFiles:\n");
                    yamlContent.Append("      - name: expansion1::subworlds/niobium/NiobiumPatch\n");
                    yamlContent.Append("        minCount: 2\n");
                    yamlContent.Append("        maxCount: 3\n");
                    yamlContent.Append("    additionalUnknownCellFilters:\n");
                    yamlContent.Append("      - tagcommand: DistanceFromTag\n");
                    yamlContent.Append("        tag: AtDepths\n");
                    yamlContent.Append("        minDistance: 1\n");
                    yamlContent.Append("        maxDistance: 1\n");
                    yamlContent.Append("        command: Replace\n");
                    yamlContent.Append("        sortOrder: 1000 # apply last so world traits and subworld mixing do not override it\n");
                    yamlContent.Append("        subworldNames:\n");
                    yamlContent.Append("          - expansion1::subworlds/niobium/NiobiumPatch\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("          - expansion1::geysers/molten_niobium\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        allowExtremeTemperatureOverlap: true # has Abyssalite border\n");
                    yamlContent.Append("        priority: 150\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: Replace\n");
                    yamlContent.Append("            subworldNames:\n");
                    yamlContent.Append("              - expansion1::subworlds/niobium/NiobiumPatch\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::poi/genericGravitas/poi_mining_room # artifacts\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 150\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoMixingTemplateSpawning\n");
                    break;

                case PlanetoidType.MiniRegolith:
                    yamlContent.Append("      - SmallWorld\n");
                    yamlContent.Append("    forbiddenTags:\n");
                    yamlContent.Append("      - SubsurfaceSubworldReserved\n");
                    yamlContent.Append("      - NoDamagingMeteorShowers\n");
                    yamlContent.Append("    additionalSubworldFiles:\n");
                    yamlContent.Append("      - name: expansion1::subworlds/regolith/BarrenDust\n");
                    yamlContent.Append("        minCount: 2\n");
                    yamlContent.Append("        overridePower: 1\n");
                    yamlContent.Append("    additionalUnknownCellFilters:\n");
                    yamlContent.Append("      - tagcommand: DistanceFromTag # surface\n");
                    yamlContent.Append("        tag: AtSurface\n");
                    yamlContent.Append("        minDistance: 2\n");
                    yamlContent.Append("        maxDistance: 2\n");
                    yamlContent.Append("        command: Replace\n");
                    yamlContent.Append("        sortOrder: 1000 # apply last so world traits and subworld mixing do not override it\n");
                    yamlContent.Append("        subworldNames:\n");
                    yamlContent.Append("          - expansion1::subworlds/regolith/BarrenDust\n");
                    yamlContent.Append("    additionalSeasons:\n");
                    yamlContent.Append("      - RegolithMoonMeteorShowers\n");
                    yamlContent.Append("    additionalWorldTemplateRules:\n");
                    yamlContent.Append("      - names:\n");
                    yamlContent.Append("        - expansion1::poi/regolith/bunker_lab\n");
                    yamlContent.Append("        listRule: GuaranteeAll\n");
                    yamlContent.Append("        priority: 200\n");
                    yamlContent.Append("        allowedCellsFilter:\n");
                    yamlContent.Append("          - command: All\n");
                    yamlContent.Append("          - command: ExceptWith\n");
                    yamlContent.Append("            tagcommand: AtTag\n");
                    yamlContent.Append("            tag: NoGlobalFeatureSpawning\n");
                    break;

                default:
                    break;
            }
        }

        return yamlContent.ToString();
    }

    public override string ToString() {
        return string.Format("{0} min:{1} max:{2} buf:{3} inner:{4}", this.planetoid,
                             this.minRadius, this.maxRadius, this.buffer, this.isInner);
    }

}
