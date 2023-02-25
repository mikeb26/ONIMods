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
        internal static void Prefix(MinionIdentity __instance) {
            globalGameState.RemoveDupe(ref __instance);
        }
    }

    // posted notification hook
    [HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
    public static class NotificationScreen_AddNotification_Patch {
        internal static bool Prefix(Notification notification) {
            return globalGameState.AddNotification(ref notification);
        }
    }

    // removed notification hook
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    public static class NotificationScreen_RemoveNotification_Patch {
        internal static void Prefix(Notification notification) {
            globalGameState.RemoveNotification(ref notification);
        }
    }
}
