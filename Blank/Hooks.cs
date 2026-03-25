// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System;
using System.Reflection;
using System.Diagnostics;
using Database;

namespace Blank;

public static class Hooks
{
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_Spawn_Patch {
        public static void Postfix(Game __instance) {
            Util.LogDbg("Game OnSpawn");
        }
    }
}
