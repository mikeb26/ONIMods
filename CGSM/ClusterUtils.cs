// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;
using Klei;
using System.Collections.Generic;

namespace CGSM;

public static class ClusterUtils {
    public static void loadClusterFromOptionsAndEmit(bool onStart) {
        var opts = POptions.ReadSettings<Options>();
        if (opts == null) {
            opts = new Options();
        }

        var cluster = new Cluster(opts);
        var clusterYamlPath = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                     "clusters", "CGSM.yaml");
        var clusterYamlPathVanilla = System.IO.Path.Combine(Mod.Instance.contentPath, "worldgen",
                                                            "clusters", "CGSMVanilla.yaml");
        var emitter = new ClusterYamlEmitter(cluster, 1, clusterYamlPath);
        var needSettingsCacheReload1 = emitter.emit();
        emitter = new ClusterYamlEmitter(cluster, 2, clusterYamlPathVanilla);
        var needSettingsCacheReload2 = emitter.emit();

        if (!onStart && (needSettingsCacheReload1 || needSettingsCacheReload2)) {
            Util.LogDbg("Reloading settings cache");

            ProcGen.SettingsCache.Clear();
            List<YamlIO.Error> errors = new List<YamlIO.Error>();
            ProcGen.SettingsCache.LoadFiles(errors);
        } else {
            Util.LogDbg("No CGSM changes; skipping settings cache reload");
        }
    }
}
