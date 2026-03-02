// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.OptionsFilt;
using System.Text;
using System.Reflection;
using System;

namespace CGSM;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://steamcommunity.com/sharedfiles/filedetails/?id=2945098028")]
public sealed class Options /* : IOptions */ {
    [Option("STRINGS.UI.FRONTEND.CGSM.STARMAP_OPT", "STRINGS.UI.FRONTEND.CGSM.STARMAP_OPT_DESC")]
    [Limit(14, 20)]
    [JsonProperty]
    public int starmapRadius { get; set; }

    [Option("STRINGS.UI.FRONTEND.CGSM.START_OPT", "STRINGS.UI.FRONTEND.CGSM.START_OPT")]
    [JsonProperty]
    public StartPlanetoidType startPlanetoid { get; set; }

    [Option("STRINGS.UI.FRONTEND.CGSM.WARP_OPT", "STRINGS.UI.FRONTEND.CGSM.WARP_OPT")]
    [JsonProperty]
    public WarpPlanetoidType warpPlanetoid { get; set; }

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

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.DLC2CERESOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.DLC2CERESOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [RequireDLC(DlcManager.DLC2_ID)]
    [JsonProperty]
    public bool dlc2CeresOreField { get; set; }

    [Option("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.DLC4PREHISTORICOREFIELD.NAME", "STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.DLC4PREHISTORICOREFIELD.DESC", "STRINGS.UI.FRONTEND.CGSM.SPACE_POIS_CAT")]
    [RequireDLC(DlcManager.DLC4_ID)]
    [JsonProperty]
    public bool dlc4PrehistoricOreField { get; set; }

    public Options() {
        // defaults
        starmapRadius = 17;

        startPlanetoid = StartPlanetoidType.Marshy;
        warpPlanetoid = WarpPlanetoidType.Desolands;

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
        // DLC-specific harvestables default enabled only when the DLC is owned/active.
        dlc2CeresOreField = DlcManager.IsContentSubscribed(DlcManager.DLC2_ID);
        dlc4PrehistoricOreField = DlcManager.IsContentSubscribed(DlcManager.DLC4_ID);
    }

    public int getHarvestPoiCount() {
        int count = 0;

        count += Convert.ToInt32(carbonAsteroid);
        count += Convert.ToInt32(chlorineCloud);
        count += Convert.ToInt32(forestyOreField);
        count += Convert.ToInt32(frozenOreField);
        count += Convert.ToInt32(gasGiantCloud);
        count += Convert.ToInt32(gildedAsteroidField);
        count += Convert.ToInt32(glimmeringAsteroidField);
        count += Convert.ToInt32(heliumCloud);
        count += Convert.ToInt32(iceAsteroidField);
        count += Convert.ToInt32(interstellarIceField);
        count += Convert.ToInt32(interstellarOcean);
        count += Convert.ToInt32(metallicAsteroidField);
        count += Convert.ToInt32(oilyAsteroidField);
        count += Convert.ToInt32(organicMassField);
        count += Convert.ToInt32(oxidizedAsteroidField);
        count += Convert.ToInt32(oxygenRichAsteroidField);
        count += Convert.ToInt32(radioactiveAsteroidField);
        count += Convert.ToInt32(radioactiveGasCloud);
        count += Convert.ToInt32(rockyAsteroidField);
        count += Convert.ToInt32(saltyAsteroidField);
        count += Convert.ToInt32(sandyOreField);
        count += Convert.ToInt32(satelliteField);
        count += Convert.ToInt32(swampyOreField);
        if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID)) {
            count += Convert.ToInt32(dlc2CeresOreField);
        }
        if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID)) {
            count += Convert.ToInt32(dlc4PrehistoricOreField);
        }

        return count;
    }

    // if a player has loaded a mod, selected that mod's specific planetoids, disabled the mod,
    // and restarted the game, we can wind up in a state where the persistent configuration
    // returned by POptions.ReadSettings() still contains references to the disabled mod's
    // planetoids. so, scrub them here if we detect this situation
    public void scrubUnloadedModOptions() {
        if (!Util.IsModEnabled("Baator_BumminsMod")) {
            if (this.startPlanetoid == StartPlanetoidType.Baator ||
                this.startPlanetoid == StartPlanetoidType.BaatorMoonlet) {
                this.startPlanetoid = StartPlanetoidType.Marshy;
                Util.Log("Baator mod is not enabled; resetting start planetoid to {0}", this.startPlanetoid);
            }
        }
        if (!Util.IsModEnabled("AllBiomesWorld")) {
            if (this.startPlanetoid == StartPlanetoidType.Fuleria) {
                this.startPlanetoid = StartPlanetoidType.Marshy;
                Util.Log("Fuleria mod is not enabled; resetting start planetoid to {0}", this.startPlanetoid);
            }
        }
        if (!Util.IsModEnabled("test447.RollerSnake")) {
            if (this.startPlanetoid == StartPlanetoidType.Tetrament ||
                this.startPlanetoid == StartPlanetoidType.VanillaTetrament) {
                this.startPlanetoid = StartPlanetoidType.Marshy;
                Util.Log("Roller Snakes mod is not enabled; resetting start planetoid to {0}", this.startPlanetoid);
            }
            if (this.warpPlanetoid == WarpPlanetoidType.DryRadioactiveForest) {
                this.warpPlanetoid = WarpPlanetoidType.Desolands;
                Util.Log("Roller Snakes mod is not enabled; resetting warp planetoid to {0}", this.warpPlanetoid);
            }
        }
    }

    // public IEnumerable<IOptionsEntry> CreateOptions() {
    //     Util.Log("CreateOptions() called");
    //     var extraOpts = new List<IOptionsEntry>();
    //     return extraOpts;
    // }
    // public void OnOptionsChanged() {
    //     Util.Log("On Options Changed testcheckbox:{0}", this.testCheckbox.Value);
    // }

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
