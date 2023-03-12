// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Klei.CustomSettings;
using UnityEngine;              
using System.Collections.Generic;
using System;

namespace CGSM;

public class Toggles {
    private Dictionary<PlanetoidType, ToggleSettingConfig> toggleMap;
    public bool anyToggleSettingsChanged { get; set;}
    // new cluster select and toggle changes are both triggered from the same
    // CustomGameSettings.SetQualitySetting hook; as toggle.reset() itself needs to
    // invoke CustomGameSettings.SetQualitySetting() this creates a recursive call
    // use resetInProgress to detect & guard this
    public bool resetInProgress;

    public Toggles() {
        this.toggleMap = new Dictionary<PlanetoidType, ToggleSettingConfig>();
        this.anyToggleSettingsChanged = false;
        this.resetInProgress = false;
    }

    // reference: CustomGameSettingConfigs()
    // @todo put Enable/Disabled & tooltips into Strings.cs
    private void addOneToggleSetting(ref CustomGameSettings cgs, PlanetoidType pType,
                                     string displayName, string displayDesc, bool changeToCustom) {
        var toggle = new ToggleSettingConfig(string.Format("{0}.{1}", "CGSM", pType.ToString()),
                                            displayName, displayDesc,
                                            new SettingLevel("Disabled", "Disabled",
                                                             "Will not be included", (long)0, null),
                                            new SettingLevel("Enabled", "Enabled", "Will be included",
                                                             (long)0, null),
                                            "Disabled", "Disabled", (long)-1, (long)-1, false,
                                            changeToCustom, "", "");
        toggleMap.Add(pType, toggle);
        cgs.AddQualitySettingConfig(toggle);
    }

    // @todo would be nice to unify this w/ Options && PlanetoidInfos to avoid repetition
    public void addAllToggles(ref CustomGameSettings cgs) {
        Util.LogDbg("Adding planetoid toggles to custom game settings");

        addOneToggleSetting(ref cgs, PlanetoidType.MetallicSwampy,
                            STRINGS.WORLDS.MINIMETALLICSWAMPY.NAME,
                            STRINGS.WORLDS.MINIMETALLICSWAMPY.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Desolands,
                            STRINGS.WORLDS.MINIBADLANDS.NAME,
                            STRINGS.WORLDS.MINIBADLANDS.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.FrozenForest,
                            STRINGS.WORLDS.MINIFORESTFROZEN.NAME,
                            STRINGS.WORLDS.MINIFORESTFROZEN.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Flipped,
                            STRINGS.WORLDS.MINIFLIPPED.NAME,
                            STRINGS.WORLDS.MINIFLIPPED.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.RadioactiveOcean,
                            STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.NAME,
                            STRINGS.WORLDS.MINIRADIOACTIVEOCEAN.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Tundra
                            , STRINGS.WORLDS.TUNDRAMOONLET.NAME,
                            STRINGS.WORLDS.TUNDRAMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Marshy,
                            STRINGS.WORLDS.MARSHYMOONLET.NAME,
                            STRINGS.WORLDS.MARSHYMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Moo, STRINGS.WORLDS.MOOMOONLET.NAME,
                            STRINGS.WORLDS.MOOMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Water,
                            STRINGS.WORLDS.WATERMOONLET.NAME,
                            STRINGS.WORLDS.WATERMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Superconductive,
                            STRINGS.WORLDS.NIOBIUMMOONLET.NAME,
                            STRINGS.WORLDS.NIOBIUMMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.Regolith,
                            STRINGS.WORLDS.REGOLITHMOONLET.NAME,
                            STRINGS.WORLDS.REGOLITHMOONLET.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.IrradiatedForest,
                            STRINGS.WORLDS.IDEALLANDINGSITE.NAME,
                            STRINGS.WORLDS.IDEALLANDINGSITE.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.IrradiatedSwamp,
                            STRINGS.WORLDS.SWAMPYLANDINGSITE.NAME,
                            STRINGS.WORLDS.SWAMPYLANDINGSITE.DESCRIPTION, false);
        addOneToggleSetting(ref cgs, PlanetoidType.IrradiatedMarsh,
                            STRINGS.WORLDS.METALHEAVYLANDINGSITE.NAME,
                            STRINGS.WORLDS.METALHEAVYLANDINGSITE.DESCRIPTION, false);

    }

    // unfortunately we can't make the set of toggled planetoids dynamic because the
    // caller is iterating over cgs.QualitySettings preventing us from invoking
    // cgs.QualitySettings.Remove(<old_planetoid_toggle>);
    //   (InvalidOperationException: Collection was modified; enumeration operation may not
    //    execute.)
    //
    // but we can at least reset the defaults to the cluster the player has selected
    //
    // *note cluster may be null in this context
    public void reset(string clusterKey, Cluster cluster) {
        Util.LogDbg("Resetting toggles for {0}...", clusterKey);

        var cgs = CustomGameSettings.Instance;
        this.resetInProgress = true;
        if (cluster == null) {
            // at least display reasonable defaults
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.MetallicSwampy], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Desolands], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.FrozenForest], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Flipped], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.RadioactiveOcean], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Tundra], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Marshy], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Moo], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Water], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Superconductive], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.Regolith], "Enabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.IrradiatedForest], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.IrradiatedSwamp], "Disabled");
            cgs.SetQualitySetting(this.toggleMap[PlanetoidType.IrradiatedMarsh], "Disabled");
            this.anyToggleSettingsChanged = false;
            this.resetInProgress = false;
            return;
        }

        var pTypes = new List<PlanetoidType>(){
            PlanetoidType.MetallicSwampy,
            PlanetoidType.Desolands,
            PlanetoidType.FrozenForest,
            PlanetoidType.Flipped,
            PlanetoidType.RadioactiveOcean,
            PlanetoidType.Tundra,
            PlanetoidType.Marshy,
            PlanetoidType.Moo,
            PlanetoidType.Water,
            PlanetoidType.Superconductive,
            PlanetoidType.Regolith,
            PlanetoidType.IrradiatedForest,
            PlanetoidType.IrradiatedSwamp,
            PlanetoidType.IrradiatedMarsh,
        };

        foreach (var pType in pTypes) {
            if (cluster.hasOtherPlanetoid(pType)) {
                cgs.SetQualitySetting(this.toggleMap[pType], "Enabled");
            } else {
                cgs.SetQualitySetting(this.toggleMap[pType], "Disabled");
            }
        }

        this.anyToggleSettingsChanged = false;
        this.resetInProgress = false;

        Util.LogDbg("All toggles reset");
    }

    // *note cluster may be null in this context
    public void toggleSetting(Cluster cluster, ref CustomGameSettings cgs, SettingConfig config,
                              string value) {

        Util.LogDbg("toggle:{0}, value:{1} resetInProgress:{2}", config.id, value,
                    this.resetInProgress);

        if (cluster == null || this.resetInProgress) {
            return;
        }

        var pTypeStr = config.id.Substring(5); // "CGSM."
        PlanetoidType pType;
        if (!Enum.TryParse<PlanetoidType>(pTypeStr, out pType)) {
            Util.Log("BUG: toggleSetting failed to convert {0}", pTypeStr);
            return;
        }
        if (value == "Enabled" && !cluster.hasAnyPlanetoid(pType)) {
            cluster.others.Add(new PlanetoidPlacement(pType,
                                                      PlanetoidCategory.Other,
                                                      4, 6, cluster.radius - 2, false));
            this.anyToggleSettingsChanged = true;
            Util.LogDbg("cluster:{0} added planet:{1}", cluster.name, pType);
        } else if (value == "Disabled" && cluster.hasOtherPlanetoid(pType)) {
            cluster.remove(pType);
            this.anyToggleSettingsChanged = true;
            Util.LogDbg("cluster:{0} removed planet:{1}", cluster.name, pType);
        }

        // no combinations of these seem to give the desired behavior; emit at Launch() time
        // for now instead. an emitCluster() here followed by ngsp.SetSetting() gets close; it
        // correctly displays the customized clusters *except* that the dsp still shows the
        // original starting planetoid sprite
        //
        // ClusterUtils.emitCluster(this.selectedCluster);
        // ngsp.SetSetting(CustomGameSettingConfigs.ClusterLayout, "clusters/CGSM");
        // dsp.UpdateDisplayedClusters();
        // cdss.RefreshRowsAndDescriptions();
        // dsp.RePlaceAsteroids();
        // dsp.SelectCluster("clusters/CGSM", 788418303);
    }
};
