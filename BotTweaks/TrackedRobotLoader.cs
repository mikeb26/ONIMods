// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;

namespace BotTweaks;

/// <summary>
/// One-time scan for existing-save robots.
///
/// We patch concrete robot config CreatePrefab() to attach TrackedRobot for new spawns,
/// but robots already present in a .sav will not have the component. This scan runs once after
/// load and attaches it to known robot prefab IDs.
/// </summary>
internal static class TrackedRobotLoader {
    private static bool ran;

    private static readonly System.Collections.Generic.Dictionary<Tag, System.Action<GameObject>> AttachByPrefabTag =
        new System.Collections.Generic.Dictionary<Tag, System.Action<GameObject>>() {
            { Flydo.PrefabTag, go => go.AddOrGet<Flydo>() },
            { Sweepy.PrefabTag, go => go.AddOrGet<Sweepy>() },
            { Biobot.PrefabTag, go => go.AddOrGet<Biobot>() },
            { Rover.PrefabTag, go => go.AddOrGet<Rover>() },
            { RemoteWorker.PrefabTag, go => go.AddOrGet<RemoteWorker>() },
        };

    internal static void ScanAndAttachForExistingSaves() {
        if (ran) {
            return;
        }
        ran = true;

        var all = Object.FindObjectsByType<KPrefabID>(FindObjectsSortMode.None);
        if (all == null || all.Length == 0) {
            return;
        }

        int attached = 0;
        for (int i = 0; i < all.Length; i++) {
            var kpid = all[i];
            if (kpid == null) {
                continue;
            }
            var tag = kpid.PrefabTag;
            if (!AttachByPrefabTag.TryGetValue(tag, out var attach) || attach == null) {
                continue;
            }

            var go = kpid.gameObject;
            if (go == null) {
                continue;
            }

            // Ensure it's actually a robot entity.
            if (!go.HasTag(GameTags.Robot)) {
                continue;
            }

            // Only attach if not already present.
            if (go.GetComponent<TrackedRobot>() == null) {
                attach(go);
                attached++;
            }
        }

        Util.LogDbg("TrackedRobotLoader: attached tracker to {0} existing robot(s)", attached);

        // Ensure resource screen picks up newly discovered robot types.
        var rs = AllResourcesScreen.Instance;
        if (rs != null) {
            rs.Populate();
        }
        var pr = PinnedResourcesPanel.Instance;
        if (pr != null) {
            pr.Refresh();
        }
    }
}
