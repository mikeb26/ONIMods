// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using Klei.AI;
using System;

namespace QuickStart;

public static class Hooks
{
    [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
    public static class SaveGame_OnPrefabInit_Patch {
        public static void Postfix(ref Game __instance) {
            Util.LogDbg("SaveGame.OnPrefabInit");

            Mod.Instance.gameState.LoadSavedStateAndOpts(ref __instance);
        }
    }

    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_OnSpawn_Patch {
        public static void Postfix() {
            Util.LogDbg("Game.OnSpawn");

            Mod.Instance.gameState.LogNewGame();
        }
    }

    // Game.OnSpawn() is too early; hook UserMenuScreen.OnPrefabInit() instead
    [HarmonyPatch(typeof(UserMenuScreen), "OnPrefabInit")]
    public static class UserMenuScreen_OnPrefabInit_Patch {
        internal static void Postfix() {
            Util.LogDbg("UserMenuScreen.OnPrefabInit");

            Mod.Instance.gameState.ApplyStartOptions();
        }
    }

    [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
    public static class MinionIdentity_OnSpawn_Patch {
        internal static void Postfix(ref MinionIdentity __instance) {
            Util.LogDbg("MinionIdentity.OnSpawn");

            Mod.Instance.gameState.MaybeUpgradeDupe(ref __instance);
        }
    }
}
