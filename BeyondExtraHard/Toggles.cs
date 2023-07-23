// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Klei.CustomSettings;
using System.Collections.Generic;

namespace BeyondExtraHard;

public class Toggles {
    public Toggles() {
    }

    public void addAllToggles(ref CustomGameSettings cgs) {
        Util.LogDbg("Adding {0} toggles to custom game settings", Constants.ModPrefix);

        var powerSettings = new List<SettingLevel>();
        powerSettings.Add(new SettingLevel(PowerSetting.Normal.ToString(),
                                           Strings.UI.FRONTEND.BEH.POWER_NORMAL,
                                           Strings.UI.FRONTEND.BEH.POWER_NORMAL_DESC));
        powerSettings.Add(new SettingLevel(PowerSetting.Brownout.ToString(),
                                           Strings.UI.FRONTEND.BEH.POWER_BROWNOUT,
                                           Strings.UI.FRONTEND.BEH.POWER_BROWNOUT_DESC));
        powerSettings.Add(new SettingLevel(PowerSetting.Blackout.ToString(),
                                           Strings.UI.FRONTEND.BEH.POWER_BLACKOUT,
                                           Strings.UI.FRONTEND.BEH.POWER_BLACKOUT_DESC));
        var powerSettingConfig = new ListSettingConfig(string.Format("{0}{1}", Constants.ModPrefix,
                                                                     BEHSetting.Power.ToString()),
                                                       Strings.UI.FRONTEND.BEH.POWER,
                                                       Strings.UI.FRONTEND.BEH.POWER_DESC,
                                                       powerSettings,
                                                       PowerSetting.Normal.ToString(),
                                                       PowerSetting.Normal.ToString());
        cgs.AddQualitySettingConfig(powerSettingConfig);

        var oxygenSettings = new List<SettingLevel>();
        oxygenSettings.Add(new SettingLevel(OxygenSetting.Normal.ToString(),
                                           Strings.UI.FRONTEND.BEH.OXYGEN_NORMAL,
                                           Strings.UI.FRONTEND.BEH.OXYGEN_NORMAL_DESC));
        oxygenSettings.Add(new SettingLevel(OxygenSetting.Gasping.ToString(),
                                           Strings.UI.FRONTEND.BEH.OXYGEN_GASPING,
                                           Strings.UI.FRONTEND.BEH.OXYGEN_GASPING_DESC));
        oxygenSettings.Add(new SettingLevel(OxygenSetting.Hyperventilating.ToString(),
                                           Strings.UI.FRONTEND.BEH.OXYGEN_HYPERVENTILATING,
                                           Strings.UI.FRONTEND.BEH.OXYGEN_HYPERVENTILATING_DESC));
        var oxygenSettingConfig = new ListSettingConfig(string.Format("{0}{1}", Constants.ModPrefix,
                                                                      BEHSetting.Oxygen.ToString()),
                                                       Strings.UI.FRONTEND.BEH.OXYGEN,
                                                       Strings.UI.FRONTEND.BEH.OXYGEN_DESC,
                                                       oxygenSettings,
                                                       OxygenSetting.Normal.ToString(),
                                                       OxygenSetting.Normal.ToString());
        cgs.AddQualitySettingConfig(oxygenSettingConfig);

        var healthSettings = new List<SettingLevel>();
        healthSettings.Add(new SettingLevel(HealthSetting.Normal.ToString(),
                                           Strings.UI.FRONTEND.BEH.HEALTH_NORMAL,
                                           Strings.UI.FRONTEND.BEH.HEALTH_NORMAL_DESC));
        healthSettings.Add(new SettingLevel(HealthSetting.Delicate.ToString(),
                                           Strings.UI.FRONTEND.BEH.HEALTH_DELICATE,
                                           Strings.UI.FRONTEND.BEH.HEALTH_DELICATE_DESC));
        healthSettings.Add(new SettingLevel(HealthSetting.Fragile.ToString(),
                                           Strings.UI.FRONTEND.BEH.HEALTH_FRAGILE,
                                           Strings.UI.FRONTEND.BEH.HEALTH_FRAGILE_DESC));
        var healthSettingConfig = new ListSettingConfig(string.Format("{0}{1}", Constants.ModPrefix,
                                                                      BEHSetting.Health.ToString()),
                                                       Strings.UI.FRONTEND.BEH.HEALTH,
                                                       Strings.UI.FRONTEND.BEH.HEALTH_DESC,
                                                       healthSettings,
                                                       HealthSetting.Normal.ToString(),
                                                       HealthSetting.Normal.ToString());
        cgs.AddQualitySettingConfig(healthSettingConfig);

        var techSettings = new List<SettingLevel>();
        techSettings.Add(new SettingLevel(TechSetting.Normal.ToString(),
                                           Strings.UI.FRONTEND.BEH.TECH_NORMAL,
                                           Strings.UI.FRONTEND.BEH.TECH_NORMAL_DESC));
        techSettings.Add(new SettingLevel(TechSetting.NoSpom.ToString(),
                                          Strings.UI.FRONTEND.BEH.TECH_NOSPOM,
                                          Strings.UI.FRONTEND.BEH.TECH_NOSPOM_DESC));
        techSettings.Add(new SettingLevel(TechSetting.ManualSieve.ToString(),
                                          Strings.UI.FRONTEND.BEH.TECH_MANUALSIEVE,
                                          Strings.UI.FRONTEND.BEH.TECH_MANUALSIEVE_DESC));
        techSettings.Add(new SettingLevel(TechSetting.NoTurbine.ToString(),
                                          Strings.UI.FRONTEND.BEH.TECH_NOTURBINE,
                                          Strings.UI.FRONTEND.BEH.TECH_NOTURBINE_DESC));
        var techSettingConfig = new ListSettingConfig(string.Format("{0}{1}", Constants.ModPrefix,
                                                                      BEHSetting.Tech.ToString()),
                                                       Strings.UI.FRONTEND.BEH.TECH,
                                                       Strings.UI.FRONTEND.BEH.TECH_DESC,
                                                       techSettings,
                                                       TechSetting.Normal.ToString(),
                                                       TechSetting.Normal.ToString());
        cgs.AddQualitySettingConfig(techSettingConfig);
    }

    public void toggleSetting(ref CustomGameSettings cgs, SettingConfig config,
                              string value) {

        Util.Log("toggle:{0}, value:{1}", config.id, value);
    }
};
