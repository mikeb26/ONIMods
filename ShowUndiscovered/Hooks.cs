// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System;
using System.Reflection;
using System.Diagnostics;
using Database;

namespace ShowUndiscovered;

public static class Hooks
{
    // DiscoveredResources.Discover has multiple overloads in some game versions.
    // Explicitly target the (Tag tag, Tag categoryTag) overload to avoid Harmony
    // ambiguous match crashes at mod load.
    [HarmonyPatch]
    public static class DiscoveredResources_Discover_Patch  {
        public static MethodBase TargetMethod() => AccessTools.Method(
            typeof(DiscoveredResources),
            "Discover",
            new[] { typeof(Tag), typeof(Tag) }
        );

        // keep prefix; some other mod is breaking postfix
        public static void Prefix(Tag tag, Tag categoryTag) {
            Mod.Instance.gameState.logDiscover(tag, categoryTag);
        }
    }

    // new game hook
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_Spawn_Patch {
        public static void Postfix(Game __instance) {
            Util.LogDbg("Game OnSpawn");

            Mod.Instance.gameState.discoverAll();
        }
    }
}
