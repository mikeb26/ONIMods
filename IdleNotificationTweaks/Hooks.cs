// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;

namespace IdleNotificationTweaks;

public static class Hooks
{
    internal static GameState globalGameState = null;

    // new game hook
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public static class Game_OnPrefabInit_Patch {
         public static void Postfix(Game __instance) {
             globalGameState = new GameState();
         }
    }

    // exit game hook
    [HarmonyPatch(typeof(Game), "DestroyInstances")]
    public static class Game_DestroyInstances_Patch {
         public static void Prefix(Game __instance) {
             globalGameState = null;

             Util.Log("Game exited; ceased tracking idle dupes.");
         }
    }

    // new dupe hook
    [HarmonyPatch(typeof(MinionIdentity), "OnPrefabInit")]
    public static class MinionIdentity_OnPrefabInit_Patch {
        internal static void Postfix(ref MinionIdentity __instance) {
            globalGameState.AddDupe(ref __instance);
        }
    }

    // dupe exit hook
    [HarmonyPatch(typeof(MinionIdentity), "OnCleanUp")]
    public static class MinionIdentity_OnCleanUp_Patch {
        internal static void Prefix(ref MinionIdentity __instance) {
            globalGameState.RemoveDupe(ref __instance);
        }
    }

    // posted notification hook (pre-display)
    [HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
    public static class NotificationScreen_AddNotification_PrePatch {
        internal static bool Prefix(ref Notification notification) {
            return globalGameState.AddNotification(ref notification);
        }
    }

    // removed notification hook
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    public static class NotificationScreen_RemoveNotification_Patch {
        internal static void Prefix(ref Notification notification) {
            globalGameState.RemoveNotification(ref notification);
        }
    }

    // The "Packed Snacks" update removes idle notifications altogether, so re-add them here
    [HarmonyPatch(typeof(Database.DuplicantStatusItems), "CreateStatusItems")]
    public static class DuplicantStatusItems_CreateStatusItems_Patch {
        internal static void Postfix(ref Database.DuplicantStatusItems __instance) {
            __instance.Idle.AddNotification(null, null, null);
            __instance.IdleInRockets.AddNotification(null, null, null);
            __instance.IdleInRockets.shouldNotify = true;
        }
    }

    // hook chore tick updates to process any delayed pauses or hidden notices
    [HarmonyPatch(typeof(GlobalChoreProvider), "Render200ms")]
    public static class GlobalChoreProvider_Render200ms_Patch {
        internal static void Postfix() {
            globalGameState.ProcessDeferredIdleNotices();
        }
    }

    // just for reference if we need to hook chores in the future
    //[HarmonyPatch(typeof(ChoreDriver.StatesInstance), "BeginChore")]
    //public static class ChoreDriver_BeginChore_Patch {
    //    internal static void Postfix(ref ChoreDriver.StatesInstance __instance) {
    //        globalGameState.BeginChore(ref __instance);
    //    }
    //}
}
