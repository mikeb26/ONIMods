// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.OptionsFilt;
using Klei;
using System.Collections.Generic;

namespace CGSM;

public static class ClusterUtils {
    public static Cluster loadClusterFromOptionsAndEmit(bool onStart) {
        var opts = POptions.ReadSettings<Options>();
        if (opts == null) {
            opts = new Options();
        }
        opts.scrubUnloadedModOptions();

        foreach(var mod in Global.Instance.modManager.mods) {
            Util.LogDbg("mod label:{0} staticID:{1}", mod.label, mod.staticID);
        }
        var cluster = new Cluster("CGSM", opts);
        var clusterYamlPath = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                     "clusters", "CGSM.yaml");
        var clusterYamlPathVanilla = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                            "clusters", "CGSMVanilla.yaml");
        var clusterYamlPathLab = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                        "clusters", "CGSMLab.yaml");

        var emitter = new ClusterYamlEmitter(cluster, 2, clusterYamlPath);
        var needSettingsCacheReload1 = emitter.emit();
        cluster.name = "CGSMVanilla";
        emitter = new ClusterYamlEmitter(cluster, 1, clusterYamlPathVanilla);
        var needSettingsCacheReload2 = emitter.emit();
        cluster.name = "CGSMLab";
        emitter = new ClusterYamlEmitter(cluster, 3, clusterYamlPathLab);
        var needSettingsCacheReload3 = emitter.emit();

        if (!onStart && (needSettingsCacheReload1 || needSettingsCacheReload2 ||
                         needSettingsCacheReload3)) {
            Util.LogDbg("Reloading settings cache");

            // @todo should be able to reduce this to just a clustercache load of only our
            // clusters if we can ensure our custom start worlds are already loaded
            //ProcGen.SettingsCache.clusterLayouts.LoadFiles(worldGenPath, "", errors);
            ProcGen.SettingsCache.Clear();
            List<YamlIO.Error> errors = new List<YamlIO.Error>();
            ProcGen.SettingsCache.LoadFiles(errors);
        } else {
            // Util.LogDbg("No CGSM changes; skipping settings cache reload");
        }

        cluster.name = "CGSM";
        return cluster;
    }

    public static void emitCluster(Cluster cluster) {
        var clusterYamlPath = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                     "clusters", "CGSM.yaml");
        var clusterYamlPathVanilla = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                            "clusters", "CGSMVanilla.yaml");
        var clusterYamlPathLab = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                        "clusters", "CGSMLab.yaml");

        var emitter = new ClusterYamlEmitter(cluster, 2, clusterYamlPath);
        var needSettingsCacheReload1 = emitter.emit();
        emitter = new ClusterYamlEmitter(cluster, 1, clusterYamlPathVanilla);
        var needSettingsCacheReload2 = emitter.emit();
        emitter = new ClusterYamlEmitter(cluster, 3, clusterYamlPathLab);
        var needSettingsCacheReload3 = emitter.emit();

        if (needSettingsCacheReload1 || needSettingsCacheReload2 || needSettingsCacheReload3) {
            Util.LogDbg("Reloading cluster cache");

            var worldGenPath = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen");
            List<YamlIO.Error> errors = new List<YamlIO.Error>();
            ProcGen.SettingsCache.clusterLayouts.LoadFiles(worldGenPath, "", errors);
        } else {
            // Util.LogDbg("No CGSM changes; skipping cluster cache reload");
        }
    }
}
