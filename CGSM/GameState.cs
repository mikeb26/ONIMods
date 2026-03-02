// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using Klei.CustomSettings;
using UnityEngine;              
using System;
using System.Collections.Generic;
using System.Threading;
using PeterHan.PLib.Options;
using ProcGen;

namespace CGSM;

public class GameState
{
    internal Options opts = null;
    public CustomGameSettings cgs {get; set;}
    public string selectedClusterKey;
    public Cluster cgsmCluster;
    private Toggles toggles;
    public WorldMixing worldMixing;

    // One-shot flag set by LaunchClicked (or any other "start new game" action)
    // and consumed by the real (non-debug) worldgen mixing call.
    private int applyLateWorldgenMutationsRequested = 0;

    public GameState()
    {
        this.opts = POptions.ReadSettings<Options>();
        if (opts == null)
            this.opts = new Options();
        this.toggles = new Toggles();
	this.worldMixing = new WorldMixing();

        ClusterUtils.loadClusterFromOptionsAndEmit(true);
        Util.Log("Opts: {0}", opts);
    }

    public void selectNewCluster(string clusterKey) {
        Util.LogDbg("new cluster selected:{0}", clusterKey);
	this.selectedClusterKey = clusterKey;
        this.toggles.reset(this.cgs, clusterKey);
	this.worldMixing.reset();
    }

    public string getCurrentCluster() {
        SettingLevel currentQualitySetting = this.cgs.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
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

    public void Launch() {
        Util.LogDbg("Launch() start");

        if (this.selectedClusterKey == "") {
            maybeSelectNewCluster();
        }

        // The destination-screen worldgen preview can call into the worldgen pipeline too.
        // We want our destructive clusterLayout mutations to happen only for the *real*
        // generation, and as late as possible.
        RequestLateWorldgenMutations();

        Util.LogDbg("Launch() end");
    }

    public void RequestLateWorldgenMutations() {
        Interlocked.Exchange(ref applyLateWorldgenMutationsRequested, 1);
    }

    public bool ConsumeLateWorldgenMutationsRequest() {
        return Interlocked.Exchange(ref applyLateWorldgenMutationsRequested, 0) == 1;
    }
}
