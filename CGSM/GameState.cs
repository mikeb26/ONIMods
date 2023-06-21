// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Klei.CustomSettings;
using UnityEngine;              

namespace CGSM;

public class GameState {
    public NewGameSettingsPanel ngsp {get; set;}
    public DestinationSelectPanel dsp {get; set;}
    public CustomGameSettings cgs {get; set;}
    public ColonyDestinationSelectScreen cdss {get; set;}
    public string selectedClusterKey;
    public Cluster selectedCluster;
    public Cluster cgsmCluster;
    private Toggles toggles;

    // we add 2 new clusters under worldgen/clusters/CGSM{Vanilla}.yaml; 1 for "vanilla" clusters
    // and 1 for spaced out clusters. once the settings cache is populated, this results in our
    // 2 clusters being added to the cluster cache. when the player is navigating through the
    // colony destination selection screen, only 1 or the other is visible as a selectable starting
    // asteroid. the one that is *not* visible we are marking here as "masked". we use this masked
    // entry in the cluster cache to store a copy of the player's selected starting asteroid to
    // use as a basis for CGSM customization. this is done to de-risk unanticipated consequences
    // in case we missed something during code inspection. we only write this masked copy
    // iff the player has customized their cluster at Launch() time; otherwise it is unused
    public string maskedCluster {get; set;}

    public GameState() {
        this.ngsp = null;
        this.dsp = null;
        this.cgs = null;
        this.cdss = null;
        this.selectedCluster = null;
        this.selectedClusterKey = "";
        this.cgsmCluster = null;
        this.toggles = new Toggles();

        ClusterUtils.loadClusterFromOptionsAndEmit(true);
    }

    public void selectNewCluster(string clusterKey) {
        Util.LogDbg("new cluster selected:{0}", clusterKey);

        bool found = false;
        foreach (var cl in ProcGen.SettingsCache.clusterLayouts.clusterCache) {
            if (clusterKey == cl.Key) {
                found = true;
                this.selectedClusterKey = cl.Key;
                if (cl.Key == "clusters/CGSM" || cl.Key == "clusters/CGSMVanilla") {
                    this.selectedCluster = this.cgsmCluster;
                } else {
                     // in case prior setting changed; @todo should make a copy instead
                    BuiltinClusters.reinit();
                    this.selectedCluster = BuiltinClusters.lookup(cl.Key);
                }
                if (this.selectedCluster == null) {
                    Util.Log("CGSM does not yet support cluster {0}; will not apply CGSM settings to this cluster", this.selectedClusterKey);
                }
                break;
            }
        }

        if (!found) {
            Util.Log("BUG: could not find {0} in cluster cache");
            return;
        }

        this.toggles.reset(this.selectedClusterKey, this.selectedCluster);
    }

    public string getCurrentCluster() {
        SettingLevel currentQualitySetting = cgs.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
             if (currentQualitySetting == null) {
              return "";
        }

        return currentQualitySetting.id;

    }

    public void maybeSelectNewCluster() {
        string curCluster = getCurrentCluster();
        if (curCluster == "" || curCluster == this.selectedClusterKey) {
            return;
        }

        selectNewCluster(curCluster);
    }

    // makes sure we start clean when ngsp is created
    public void resetTogglesAndSelectedCluster() {
        this.selectNewCluster(this.selectedClusterKey);
    }

    public void addToggleSettings(ref CustomGameSettings cgs) {
        this.toggles.addAllToggles(ref cgs);
    }
    public void toggleSetting(ref CustomGameSettings cgs, SettingConfig config, string value) {
        this.toggles.toggleSetting(this.selectedCluster, ref cgs, config, value);
    }

    public void Launch() {
        if (this.selectedCluster == null) {
            maybeSelectNewCluster();
        }
        string curCluster = getCurrentCluster();
        if (curCluster != selectedClusterKey) {
            selectNewCluster(curCluster);
        }
        if (!this.toggles.anyToggleSettingsChanged) {
            return;
        }

        if (this.ngsp == null) {
            Util.Log("BUG: missing new game settings panel; not applying CGSM customizations");
            return;
        }

        Util.Log("Lauching customized {0}", this.selectedCluster.ToString());
        ClusterUtils.emitCluster(this.selectedCluster);

        this.ngsp.SetSetting(CustomGameSettingConfigs.ClusterLayout, this.maskedCluster);
    }
}
