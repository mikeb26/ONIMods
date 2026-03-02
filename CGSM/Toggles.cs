// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using Klei.CustomSettings;
using UnityEngine;              
using System.Collections.Generic;
using System;
using ProcGen;

namespace CGSM;

public class Toggles {
    public Toggles() {
    }

    public void reset(CustomGameSettings cgs, string clusterKey) {
        Util.LogDbg("Resetting toggles for {0}...", clusterKey);

        var presentWorlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var clusterLayout = cgs.GetCurrentClusterLayout();

        foreach (var wp in clusterLayout.worldPlacements) {
            presentWorlds.Add(wp.world);
        }

        foreach (var kvp in cgs.MixingSettings) {
            if (kvp.Value is not WorldMixingSettingConfig wm) {
                continue;
            }

            var haveWorldPath = WorldMixing.tryGetWorldPath(wm, out var worldPath);
            var isCGSMSetting = WorldMixing.isCGSMWorldMixingSetting(wm);

            string desired = WorldMixingSettingConfig.DisabledLevelId;
            if (isCGSMSetting && haveWorldPath && presentWorlds.Contains(worldPath)) {
                desired = WorldMixingSettingConfig.TryMixingLevelId;
            }

            // we set non-CGSM settings here as well to workaround Klei UI glitch where
	    // non-default Ceres/prehistory and biome mixing settings from a prior
	    // New Game invocation are displayed but not honored
            Util.LogDbg("Resetting {0}.{1} toggle to {2}", clusterKey, worldPath, desired);

            cgs.SetMixingSetting(wm, desired);
        }
    }
};
