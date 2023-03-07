// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.ComponentModel;
using System.Text;
using System.Reflection;

namespace CGSM;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/CGSM")]
[RestartRequired]
public sealed class Options {
    [Option("STRINGS.UI.FRONTEND.CGSM.STARMAP_OPT", "STRINGS.UI.FRONTEND.CGSM.STARMAP_OPT_DESC")]
    [Limit(14, 20)]
    [JsonProperty]
    public int starmapRadius { get; set; }

    [Option("STRINGS.UI.FRONTEND.CGSM.START_OPT", "STRINGS.UI.FRONTEND.CGSM.START_OPT")]
    [JsonProperty]
    public StartPlanetoids startPlanetoid { get; set; }

    [Option("STRINGS.UI.FRONTEND.CGSM.WARP_OPT", "STRINGS.UI.FRONTEND.CGSM.WARP_OPT")]
    [JsonProperty]
    public WarpPlanetoids warpPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MINIMETALLICSWAMPY.NAME", "STRINGS.WORLDS.MINIMETALLICSWAMPY.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool metallicSwampyPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MINIBADLANDS.NAME", "STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool desolandsPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MINIFORESTFROZEN.NAME", "STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool frozenForestPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MINIFLIPPED.NAME", "STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool flippedPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME", "STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool radioactiveOceanPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.TUNDRAMOONLET.NAME", "STRINGS.WORLDS.TUNDRAMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool tundraPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MARSHYMOONLET.NAME", "STRINGS.WORLDS.MARSHYMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool marshyPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.MOOMOONLET.NAME", "STRINGS.WORLDS.MOOMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool mooPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.WATERMOONLET.NAME", "STRINGS.WORLDS.WATERMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool waterPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.NIOBIUMMOONLET.NAME", "STRINGS.WORLDS.NIOBIUMMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool firePlanetoid { get; set; }

    [Option("STRINGS.WORLDS.REGOLITHMOONLET.NAME", "STRINGS.WORLDS.REGOLITHMOONLET.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool regolithPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.IDEALLANDINGSITE.NAME", "STRINGS.WORLDS.IDEALLANDINGSITE.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool irradiatedForestPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.SWAMPYLANDINGSITE.NAME", "STRINGS.WORLDS.SWAMPYLANDINGSITE.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool irradiatedSwampPlanetoid { get; set; }

    [Option("STRINGS.WORLDS.METALHEAVYLANDINGSITE.NAME", "STRINGS.WORLDS.METALHEAVYLANDINGSITE.DESCRIPTION", "STRINGS.UI.FRONTEND.CGSM.ADDITIONAL_PLANET_CAT")]
    [JsonProperty]
    public bool irradiatedMarshPlanetoid { get; set; }

    [Option("STRINGS.UI.FRONTEND.CGSM.NUM_ARTIFACT_OPT", "STRINGS.UI.FRONTEND.CGSM.NUM_ARTIFACT_OPT_DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [Limit(0, 9)]
    [JsonProperty]
    public int numArtifactPOIs { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.CARBONASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.CARBONASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool carbonAsteroid { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.CHLORINECLOUD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.CHLORINECLOUD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool chlorineCloud { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.FORESTYOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.FORESTYOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool forestyOreField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.FROZENOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.FROZENOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool frozenOreField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GASGIANTCLOUD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GASGIANTCLOUD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool gasGiantCloud { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GILDEDASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GILDEDASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool gildedAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GLIMMERINGASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.GLIMMERINGASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool glimmeringAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.HELIUMCLOUD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.HELIUMCLOUD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool heliumCloud { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ICEASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ICEASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool iceAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.INTERSTELLARICEFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.INTERSTELLARICEFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool interstellarIceField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.INTERSTELLAROCEAN.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.INTERSTELLAROCEAN.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool interstellarOcean { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.METALLICASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.METALLICASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool metallicAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OILYASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OILYASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool oilyAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ORGANICMASSFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ORGANICMASSFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool organicMassField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OXIDIZEDASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OXIDIZEDASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool oxidizedAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OXYGENRICHASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.OXYGENRICHASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool oxygenRichAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.RADIOACTIVEASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.RADIOACTIVEASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool radioactiveAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.RADIOACTIVEGASCLOUD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.RADIOACTIVEGASCLOUD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool radioactiveGasCloud { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ROCKYASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.ROCKYASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool rockyAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SALTYASTEROIDFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SALTYASTEROIDFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool saltyAsteroidField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SANDYOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SANDYOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool sandyOreField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SATELLITEFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SATELLITEFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool satelliteField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SWAMPYOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.SWAMPYOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [JsonProperty]
    public bool swampyOreField { get; set; }

    public Options() {
        // defaults
        starmapRadius = 17;

        startPlanetoid = StartPlanetoids.RadioactiveOcean;
        warpPlanetoid = WarpPlanetoids.Desolands;

        metallicSwampyPlanetoid = false;
        desolandsPlanetoid = false;
        frozenForestPlanetoid = true;
        flippedPlanetoid = false;
        radioactiveOceanPlanetoid = false;
        tundraPlanetoid = true;
        marshyPlanetoid = true;
        mooPlanetoid = false;
        waterPlanetoid = true;
        firePlanetoid = true;
        regolithPlanetoid = false;
        irradiatedForestPlanetoid = false;
        irradiatedSwampPlanetoid = false;
        irradiatedMarshPlanetoid = false;

        numArtifactPOIs = 4;
        carbonAsteroid = true;
        chlorineCloud = false;
        forestyOreField = true;
        frozenOreField = true;
        gasGiantCloud = true;
        gildedAsteroidField = true;
        glimmeringAsteroidField = true;
        heliumCloud = false;
        iceAsteroidField = false;
        interstellarIceField = true;
        interstellarOcean = false;
        metallicAsteroidField = true;
        oilyAsteroidField = true;
        organicMassField = true;
        oxidizedAsteroidField = true;
        oxygenRichAsteroidField = true;
        radioactiveAsteroidField = true;
        radioactiveGasCloud = true;
        rockyAsteroidField = true;
        saltyAsteroidField = true;
        sandyOreField = true;
        satelliteField = true;
        swampyOreField = true;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Options[ ");

        foreach(PropertyInfo prop in typeof(Options).GetProperties()) {
            sb.Append(string.Format("{0}={1} ", prop.Name, prop.GetValue(this, null)));
        }
        sb.Append("]");

        return sb.ToString();
    }
}
