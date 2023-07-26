// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System.Diagnostics;
using Database;

namespace ShowUndiscovered;

public static class Hooks
{
    // [HarmonyPatch(typeof(DiscoveredResources), "Discover")]
    // public static class DiscoveredResources_Discover_Patch  {
    //     // keep prefix; some other mod is breaking postfix
    //     public static void Prefix(Tag tag, Tag categoryTag) {
    //         Mod.Instance.gameState.logMissing(tag, categoryTag);
    //     }
    // }

    // new game hook
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_Spawn_Patch {
        public static void Postfix(Game __instance) {
            Util.LogDbg("Game OnSpawn");

            Mod.Instance.gameState.discoverAll();
        }
    }
}
