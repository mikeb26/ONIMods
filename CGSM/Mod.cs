// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace CGSM;

public class Mod : UserMod2
{
    public static LocString CLUSTER_NAME;
    public static LocString CLUSTER_DESCRIPTION;

    private Options opts = null;

    static Mod() {
        Mod.CLUSTER_NAME = "Cluster Generation Settings Manager";
        Mod.CLUSTER_DESCRIPTION = "CGSM generated cluster. To change planetoids please go back to the Mods menu and click CGSM's Options button.";
    }
    public Mod() {
    }

    public override void OnLoad(Harmony harmony) {
        Util.Log("Loading v{0}", Util.Version());

        PUtil.InitLibrary(false);

        Strings.Add(new string[] { "STRINGS.CLUSTER_NAMES.CGSM_CLUSTER.NAME", Mod.CLUSTER_NAME });
        Strings.Add(new string[] { "STRINGS.CLUSTER_NAMES.CGSM_CLUSTER.DESCRIPTION", Mod.CLUSTER_DESCRIPTION });

        // @todo ModUtil.RegisterForTranslation(typeof(Mod));

        new POptions().RegisterOptions(this, typeof(Options));
        this.opts = POptions.ReadSettings<Options>();
        if (opts == null) {
            this.opts = new Options();
        }

        Emitter.generateYAML(this.opts, mod.ContentPath);

        base.OnLoad(harmony);
    }
}