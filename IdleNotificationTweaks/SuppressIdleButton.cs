// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using KSerialization;
using System;
using UnityEngine;

// references:
//   https://github.com/AzeTheGreat/ONI-Mods/blob/master/src/SuppressNotifications/UI/SuppressionButton.cs
//   https://github.com/Berkays/OniMods/blob/main/HysteresisStorage/UI/HysteresisStorageToggleButton.cs

namespace IdleNotificationTweaks;

public class SuppressIdleButton : KMonoBehaviour, ISaveLoadable
{
    [Serialize]
    public bool IsIdleSuppressed;

    /* when our button object is instantiated, register a callback for when the user menu
     * is refreshed. this gives us the opportunity to paint the correct state of our idle
     * suppression button
     */
    protected override void OnPrefabInit() {
        Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu());
    }

    protected override void OnCleanUp() {
        Unsubscribe((int)GameHashes.RefreshUserMenu);
        base.OnCleanUp();
    }

    private void OnRefreshUserMenu() {
        if (gameObject.HasTag(GameTags.Dead)) {
            return;
        }

        // For vanilla and SO, must explicitly set the right action since this enum is different.
        Enum.TryParse(nameof(Action.NumActions), out Action action);

        if (!IsIdleSuppressed) {
            Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", "Suppress Idle", new System.Action(OnSuppressClick), action, tooltipText: GetSuppressString()));
        } else {
            Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", "Clear Suppressed", new System.Action(OnClearClick), action, tooltipText: GetUnsuppressedString()));
        }
    }

    internal string GetSuppressString() {
        return "Suppress Idle notifications from this duplicant";
    }

    internal string GetUnsuppressedString() {
        return "Stop suppressing Idle notifications from this duplicant";
    }

    internal void OnSuppressClick() {
        IsIdleSuppressed = true;

        /* @todo is there a cleaner way to derive gameState? */
        Hooks.globalGameState.RefreshNotifications();
    }

    internal void OnClearClick() {
        IsIdleSuppressed = false;

        /* @todo is there a cleaner way to derive gameState? */
        Hooks.globalGameState.RefreshNotifications();
    }
}
