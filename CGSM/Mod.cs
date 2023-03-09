// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using PeterHan.PLib.Database;

namespace CGSM;

public class Mod : UserMod2
{
    private Options opts = null;
    private Cluster cluster = null;

    public override void OnLoad(Harmony harmony) {
        Util.Log("Loading v{0}", Util.Version());

        PUtil.InitLibrary(false);
        LocString.CreateLocStringKeys(typeof(Strings.UI));
        LocString.CreateLocStringKeys(typeof(Strings.CLUSTER_NAMES));
        LocString.CreateLocStringKeys(typeof(Strings.WORLDS));
        new PLocalization().Register();
        new POptions().RegisterOptions(this, typeof(Options));
        this.opts = POptions.ReadSettings<Options>();
        if (opts == null) {
            this.opts = new Options();
        }

        this.cluster = new Cluster(this.opts);
        var clusterYamlPath = System.IO.Path.Combine(mod.ContentPath, "worldgen", "clusters",
                                                     "CGSM.yaml");
        var emitter = new ClusterYamlEmitter(cluster, clusterYamlPath);
	emitter.emit();

        base.OnLoad(harmony);
    }
}