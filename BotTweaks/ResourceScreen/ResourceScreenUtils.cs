/*
 * Copyright © 2026 Mike Brown; see LICENSE at the root of this package
 */

using UnityEngine;
using UnityEngine.UI;

namespace BotTweaks.ResourceScreen;

internal static class ResourceScreenUtils {
    internal static bool TryGetActiveInventory(out RobotInventory inv) {
        inv = null;

        var cm = ClusterManager.Instance;
        if (cm == null || cm.activeWorld == null) {
            return false;
        }

        return cm.activeWorld.TryGetComponent(out inv) && inv != null;
    }

    internal static void ApplyRobotIcon(GameObject row, Image icon, RobotType robotType) {
        if (icon == null) {
            return;
        }

        if (robotType == RobotType.RemoteWorker) {
            // HACK: Def.GetUISprite() doesnt work for remote worker
            BotTweaks.RemoteWorker.ApplyIcon(row, icon);
            return;
        }

        var ui = Def.GetUISprite(RobotTrackingUtils.GetRobotModelTag(robotType));
        icon.sprite = ui.first;
        icon.color = ui.second;
        icon.enabled = true;
        icon.preserveAspect = true;
    }
}
