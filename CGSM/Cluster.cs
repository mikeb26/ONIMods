// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;

namespace CGSM;

public class Cluster {
    public Planetoid start { get; }
    public Planetoid warp { get; }
    public Planetoid[] others { get; }
    public Options opts { get; }

    public Cluster(Options optsIn) {
        this.opts = optsIn;

        if (this.opts.warpPlanetoid == (WarpPlanetoidType) this.opts.startPlanetoid) {
            Util.Log("Warning: starting planetoid and warp planetoid are the same; this is untested");
        }

        this.start = new Planetoid(this.opts.startPlanetoid);
        this.warp = new Planetoid(this.opts.warpPlanetoid);
        this.others = addOtherPlanets();

        Util.LogDbg("Opts.others len: {0} filtered len: {1}", this.opts.getOtherPlanetoidCount(),
                    this.others.Length);

        logCluster();
    }

    public void logCluster() {
        Util.LogDbg("Created new cluster:");
        Util.LogDbg("    start:    {0}", this.start);
        Util.LogDbg("    warp:     {0}", this.warp);

        for (int ii = 0; ii<this.others.Length; ii++) {
            Util.LogDbg("    other[{0}]: {1}", ii, this.others[ii]);
        }
    }

    private Planetoid[] addOtherPlanets() {
        var tmpOthers = new Planetoid[this.opts.getOtherPlanetoidCount()];
        int idx = 0;

        addOtherPlanetIfSet(this.opts.metallicSwampyPlanetoid, PlanetoidType.MetallicSwampy,
                            ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.desolandsPlanetoid, PlanetoidType.Desolands, ref idx,
                            tmpOthers);
        addOtherPlanetIfSet(this.opts.frozenForestPlanetoid, PlanetoidType.FrozenForest, ref idx,
                            tmpOthers);
        addOtherPlanetIfSet(this.opts.flippedPlanetoid, PlanetoidType.Flipped, ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.radioactiveOceanPlanetoid, PlanetoidType.RadioactiveOcean,
                            ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.tundraPlanetoid, PlanetoidType.Tundra, ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.marshyPlanetoid, PlanetoidType.Marshy, ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.mooPlanetoid, PlanetoidType.Moo, ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.waterPlanetoid, PlanetoidType.Water, ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.firePlanetoid, PlanetoidType.Superconductive, ref idx,
                            tmpOthers);
        addOtherPlanetIfSet(this.opts.regolithPlanetoid, PlanetoidType.Regolith, ref idx,
                            tmpOthers);
        addOtherPlanetIfSet(this.opts.irradiatedForestPlanetoid, PlanetoidType.IrradiatedForest,
                            ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.irradiatedSwampPlanetoid, PlanetoidType.IrradiatedSwamp,
                            ref idx, tmpOthers);
        addOtherPlanetIfSet(this.opts.irradiatedMarshPlanetoid, PlanetoidType.IrradiatedMarsh,
                            ref idx, tmpOthers);

        // addOtherPlanetIfSet() may have filtered some out
        Array.Resize(ref tmpOthers, idx);

        return tmpOthers;
    }

    private void addOtherPlanetIfSet(bool optVal, PlanetoidType planetoidType, ref int idx,
                                     Planetoid[] tmpOthers) {
        if (!optVal) {
            return;
        }

        // @todo add conflict checking
        if (planetoidType == this.start.Type()) {
            Util.Log("Skipping other planetoid {0} since it is already the starting one",
                     planetoidType);
            return;
        }
        if (planetoidType == this.warp.Type()) {
            Util.Log("Skipping other planetoid {0} since it is already the warp one",
                     planetoidType);
            return;
        }

        tmpOthers[idx] = new Planetoid(planetoidType, PlanetoidCategory.Other);
        idx++;
    }
}
