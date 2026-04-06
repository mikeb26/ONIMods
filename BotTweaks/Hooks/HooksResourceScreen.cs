// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;

namespace BotTweaks.Hooks;

internal static class HooksResourceScreen {
    [HarmonyPatch(typeof(AllResourcesScreen), "OnPrefabInit")]
    public static class AllResourcesScreen_OnPrefabInit_Patch {
        public static void Postfix(AllResourcesScreen __instance) {
            Util.LogDbg("AllResourcesScreen.OnPrefabInit: adding RobotCategoryRows");
            __instance.gameObject.AddOrGet<ResourceScreen.RobotCategoryRows>();
        }
    }

    [HarmonyPatch(typeof(AllResourcesScreen), nameof(AllResourcesScreen.RefreshRows))]
    public static class AllResourcesScreen_RefreshRows_Patch {
        public static void Postfix(AllResourcesScreen __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.RobotCategoryRows rr)) {
                Util.LogDbg("AllResourcesScreen.RefreshRows: updating robot rows");
                rr.UpdateContents();
            }
        }
    }

    [HarmonyPatch(typeof(AllResourcesScreen), "SearchFilter")]
    public static class AllResourcesScreen_SearchFilter_Patch {
        public static void Prefix(AllResourcesScreen __instance, string search) {
            if (__instance.TryGetComponent(out ResourceScreen.RobotCategoryRows rr)) {
                rr.SearchFilter(search);
            }
        }
    }

    [HarmonyPatch(typeof(AllResourcesScreen), "SetRowsActive")]
    public static class AllResourcesScreen_SetRowsActive_Patch {
        public static void Postfix(AllResourcesScreen __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.RobotCategoryRows rr)) {
                rr.SetRowsActive();
            }
        }
    }

    [HarmonyPatch(typeof(AllResourcesScreen), "SpawnRows")]
    public static class AllResourcesScreen_SpawnRows_Patch {
        public static void Postfix(AllResourcesScreen __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.RobotCategoryRows rr)) {
                Util.LogDbg("AllResourcesScreen.SpawnRows: spawning robot rows");
                rr.SpawnRows();
            }
        }
    }

    [HarmonyPatch(typeof(PinnedResourcesPanel), "OnSpawn")]
    public static class PinnedResourcesPanel_OnSpawn_Patch {
        public static void Prefix(PinnedResourcesPanel __instance) {
            Util.LogDbg("PinnedResourcesPanel.OnSpawn: adding PinnedRobotManager");
            __instance.gameObject.AddOrGet<ResourceScreen.PinnedRobotManager>();
        }
    }

    [HarmonyPatch(typeof(PinnedResourcesPanel), nameof(PinnedResourcesPanel.Refresh))]
    public static class PinnedResourcesPanel_Refresh_Patch {
        public static void Postfix(PinnedResourcesPanel __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.PinnedRobotManager rm)) {
                Util.LogDbg("PinnedResourcesPanel.Refresh: updating robot pinned entries");
                rm.UpdateContents();
            }
        }
    }

    [HarmonyPatch(typeof(PinnedResourcesPanel), "SortRows")]
    public static class PinnedResourcesPanel_SortRows_Patch {
        public static void Postfix(PinnedResourcesPanel __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.PinnedRobotManager rm)) {
                Util.LogDbg("PinnedResourcesPanel.SortRows: populating pinned robot rows");
                rm.PopulatePinnedRows();
            }
        }
    }

    [HarmonyPatch(typeof(PinnedResourcesPanel), "SyncRows")]
    public static class PinnedResourcesPanel_SyncRows_Patch {
        public static void Postfix(PinnedResourcesPanel __instance) {
            if (__instance.TryGetComponent(out ResourceScreen.PinnedRobotManager rm) && rm.IsDirty) {
                Util.LogDbg("PinnedResourcesPanel.SyncRows: dirty -> repopulating");
                __instance.Populate();
            }
        }
    }
}
