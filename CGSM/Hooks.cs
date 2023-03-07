// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;

namespace CGSM;

public static class Hooks
{
    // new game hook
    [HarmonyPatch(typeof(ProcGenGame.Cluster), "BeginGeneration")]
    public static class Game_OnPrefabInit_Patch {
         public static void Postfix(ref ProcGenGame.Cluster __instance) {
             WorldGen.ApplyGeyserPreferences(ref __instance);
         }
    }
}
