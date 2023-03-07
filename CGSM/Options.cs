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
    [Option("Starmap radius", "Number of hexes from the center to an outermost hex on the starmap")]
    [Limit(14, 20)]
    [JsonProperty]
    public int starmapRadius { get; set; }

    [Option("Start Planetoid", "Select your starting planetoid", "Planetoids")]
    [JsonProperty]
    public StartPlanetoids startPlanetoid { get; set; }

    [Option("Warp Planetoid", "Select your teleport planetoid", "Planetoids")]
    [JsonProperty]
    public WarpPlanetoids warpPlanetoid { get; set; }

    [Option("Include Metallic Swampy Planetoid?", "(uncheck if already selected as start or warp planetoid)", "Planetoids")]
    [JsonProperty]
    public bool metallicSwampyPlanetoid { get; set; }

    [Option("Include Desolands Planetoid?", "(uncheck if already selected as start or warp planetoid)", "Planetoids")]
    [JsonProperty]
    public bool desolandsPlanetoid { get; set; }

    [Option("Include Frozen Forest Planetoid?", "(uncheck if already selected as start or warp planetoid)", "Planetoids")]
    [JsonProperty]
    public bool frozenForestPlanetoid { get; set; }

    [Option("Include Flipped Planetoid?", "(uncheck if already selected as start or warp planetoid)", "Planetoids")]
    [JsonProperty]
    public bool flippedPlanetoid { get; set; }

    [Option("Include Radioactive Ocean Planetoid?", "(uncheck if already selected as start or warp planetoid)", "Planetoids")]
    [JsonProperty]
    public bool radioactiveOceanPlanetoid { get; set; }

    [Option("Include Tundra Planetoid?", "Must be included to open the Temporal Tear", "Planetoids")]
    [JsonProperty]
    public bool tundraPlanetoid { get; set; }

    [Option("Include Marshy Planetoid?", "Must be included to produce Insulation or Visco-gel", "Planetoids")]
    [JsonProperty]
    public bool marshyPlanetoid { get; set; }

    [Option("Include Moo Planetoid?", "Must be included to tame Gassy Moos or farm Gas Grass", "Planetoids")]
    [JsonProperty]
    public bool mooPlanetoid { get; set; }

    [Option("Include Water Planetoid?", "Should be included for lots of natural Fossil tiles", "Planetoids")]
    [JsonProperty]
    public bool waterPlanetoid { get; set; }

    [Option("Include Fire Planetoid?", "Must be included to produce Thermium", "Planetoids")]
    [JsonProperty]
    public bool firePlanetoid { get; set; }

    [Option("Include Regolith Planetoid?", "Must be included for regular meteor showers", "Planetoids")]
    [JsonProperty]
    public bool regolithPlanetoid { get; set; }

    [Option("Number of Artifact Only POIs", "(russell's teapot, gravitas space station, etc.)", "Space POIs")]
    [Limit(0, 9)]
    [JsonProperty]
    public int numArtifactPOIs { get; set; }

    [Option("Include Carbon Asteroid?", "(refined carbon, coal, diamon)", "Space POIs")]
    [JsonProperty]
    public bool carbonAsteroid { get; set; }

    [Option("Include ChlorineCloud?", "(bleach stone, liquid chlorine)", "Space POIs")]
    [JsonProperty]
    public bool chlorineCloud { get; set; }

    [Option("Include Forested Ore Field?", "(igneous rock, carbone dioxide, aluminum ore)", "Space POIs")]
    [JsonProperty]
    public bool forestyOreField { get; set; }

    [Option("Include Frozen Ore Asteroid Field?", "(ice, polluted ice, aluminum ore, snow)", "Space POIs")]
    [JsonProperty]
    public bool frozenOreField { get; set; }

    [Option("Include Exploded Gas Giant Cloud?", "(hydrogen, natural gas, methane, sold methane)", "Space POIs")]
    [JsonProperty]
    public bool gasGiantCloud { get; set; }

    [Option("Include Gilded Asteroid Field?", "(sedimentary rock, gold, fullerene, refined carbon, regolith)", "Space POIs")]
    [JsonProperty]
    public bool gildedAsteroidField { get; set; }

    [Option("Include Glimmering Asteroid Field?", "(wolframite, liquid tungsten, coal, carbon dioxide)", "Space POIs")]
    [JsonProperty]
    public bool glimmeringAsteroidField { get; set; }

    [Option("Include Helium Cloud?", "(water, hydrogen)", "Space POIs")]
    [JsonProperty]
    public bool heliumCloud { get; set; }

    [Option("Include Ice Asteroid Field?", "(solid carbon dioxide, ice, solid oxygen)", "Space POIs")]
    [JsonProperty]
    public bool iceAsteroidField { get; set; }

    [Option("Include Exploded Ice Giant?", "(ice, solid carbon dioxide, oxygen, solid methane)", "Space POIs")]
    [JsonProperty]
    public bool interstellarIceField { get; set; }

    [Option("Include Interstellar Ocean?", "(salt water, brine, salt, ice)", "Space POIs")]
    [JsonProperty]
    public bool interstellarOcean { get; set; }

    [Option("Include Metallic Asteroid Field?", "(obsidian, copper ore, liquid iron)", "Space POIs")]
    [JsonProperty]
    public bool metallicAsteroidField { get; set; }

    [Option("Include Oily Asteroid Field?", "(solid carbon dioxide, solid methane, crude oil)", "Space POIs")]
    [JsonProperty]
    public bool oilyAsteroidField { get; set; }

    [Option("Include Organic Mass Field?", "(slime, algae, dirt, polluted oxygen)", "Space POIs")]
    [JsonProperty]
    public bool organicMassField { get; set; }

    [Option("Include Oxidized Asteroid Field?", "(rust, solid carbon dioxide)", "Space POIs")]
    [JsonProperty]
    public bool oxidizedAsteroidField { get; set; }

    [Option("Include Oxygen Rich Asteroid Field?", "(water, ice, polluted oxygen)", "Space POIs")]
    [JsonProperty]
    public bool oxygenRichAsteroidField { get; set; }

    [Option("Include Radioactive Asteroid Field?", "(rust, sulfur, uranium ore, bleach stone)", "Space POIs")]
    [JsonProperty]
    public bool radioactiveAsteroidField { get; set; }

    [Option("Include Radioactive Gas Cloud?", "(carbon dioxide, uranium ore, liquid chlorine)", "Space POIs")]
    [JsonProperty]
    public bool radioactiveGasCloud { get; set; }

    [Option("Include Rocky Asteroid Field?", "(sedimentary rock, igneous rock, copper ore)", "Space POIs")]
    [JsonProperty]
    public bool rockyAsteroidField { get; set; }

    [Option("Include Salty Asteroid Field?", "(walt water, brine, solid carbon dioxide)", "Space POIs")]
    [JsonProperty]
    public bool saltyAsteroidField { get; set; }

    [Option("Include Sandy Ore Field?", "(sandstone, sand, algae, copper ore)", "Space POIs")]
    [JsonProperty]
    public bool sandyOreField { get; set; }

    [Option("Include Space Debris?", "(sand, iron ore, liquid copper, glass)", "Space POIs")]
    [JsonProperty]
    public bool satelliteField { get; set; }

    [Option("Include Swampy Ore Field?", "(polluted dirt, mud, cobalt, ore)", "Space POIs")]
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
