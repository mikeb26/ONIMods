// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace IdleNotificationTweaks;

public class IdleNotificationTweaksMod : UserMod2
{
    public override void OnLoad(Harmony harmony) {
        base.OnLoad(harmony);

        Util.Log("Loading v{0}", Util.Version());

        PUtil.InitLibrary(false);

        new POptions().RegisterOptions(this, typeof(IdleOptions));
    }
}
