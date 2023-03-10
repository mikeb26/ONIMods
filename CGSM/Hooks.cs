// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;

namespace CGSM;
public static class Hooks
{
    [HarmonyPatch(typeof(ClusterCategorySelectionScreen))]
    [HarmonyPatch("OnClickSpacedOut")]
    public class ClusterCat_OnClickSpacedOut_Patch {
        public static void Postfix() {
            ClusterUtils.loadClusterFromOptionsAndEmit(false);
        }
    }

    [HarmonyPatch(typeof(ClusterCategorySelectionScreen))]
    [HarmonyPatch("OnClickVanilla")]
    public class ClusterCat_OnClickVanilla_Patch {
        public static void Postfix() {
            ClusterUtils.loadClusterFromOptionsAndEmit(false);
        }
    }

    // new game hook
    [HarmonyPatch(typeof(ProcGenGame.Cluster), "BeginGeneration")]
    public static class Game_OnPrefabInit_Patch {
         public static void Postfix(ref ProcGenGame.Cluster __instance) {
             WorldGen.ApplyGeyserPreferences(ref __instance);
         }
    }
}
