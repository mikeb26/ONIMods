// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using PeterHan.PLib.Database;

namespace ShowUndiscovered;

public static class Constants {
    public const string ModPrefix = "SU.";
    public const string ModName = "ShowUndiscovered";
}

public class Mod : UserMod2
{
    public static Mod Instance;
    public string contentPath;
    public GameState gameState;

    public override void OnLoad(Harmony harmony) {
        Util.Log("Loading v{0}", Util.Version());

        PUtil.InitLibrary(false);
        LocString.CreateLocStringKeys(typeof(Strings.UI));
        new PLocalization().Register();
        new POptions().RegisterOptions(this, typeof(SUOptions));

        Instance = this;
        contentPath = mod.ContentPath;
        gameState = new GameState();

        base.OnLoad(harmony);
    }
}