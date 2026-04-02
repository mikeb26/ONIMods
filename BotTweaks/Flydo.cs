// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using UnityEngine;

namespace BotTweaks;

internal sealed class Flydo : TrackedRobot
{
    internal static readonly Tag PrefabTag = new Tag("FetchDrone");
    internal override RobotType RobotType => RobotType.Flydo;

    internal static bool IsFlydo(GameObject go)
    {
        if (go == null) {
            return false;
        }
        var kpid = go.GetComponent<KPrefabID>();
        return kpid != null && kpid.PrefabTag == PrefabTag;
    }

    // Flydo: if the player disables a Power Bank in the Flydo's element filter,
    // the currently-installed bank should be dropped immediately.
    public static void MaybeRemoveBattery(RobotElectroBankMonitor.Instance rebm,
        HashSet<Tag> allowed_tags)
    {
        if (rebm == null) {
            return;
        }

        int dropped = DropPowerBanks(rebm.electroBankStorage, allowed_tags,
            "Flydo filter changed: dropping {0} disallowed electrobank(s)");
        if (dropped > 0) {
            rebm.ElectroBankStorageChange();
        }
    }

    private static int DropPowerBanks(Storage storage, HashSet<Tag> allowed_tags, string log_fmt)
    {
        if (storage == null || storage.items == null || storage.items.Count == 0) {
            return 0;
        }

        var toDrop = new List<GameObject>();
        foreach (var item in storage.items) {
            if (item == null || !item.HasTag(GameTags.ChargedPortableBattery)) {
                continue;
            }

            // When allowed_tags is provided, only drop banks which are no longer allowed.
            if (allowed_tags != null) {
                var prefabId = item.GetComponent<KPrefabID>();
                if (prefabId != null && allowed_tags.Contains(prefabId.PrefabTag)) {
                    continue;
                }
            }

            toDrop.Add(item);
        }

        if (toDrop.Count == 0) {
            return 0;
        }

        Util.LogDbg(log_fmt, toDrop.Count);
        foreach (var item in toDrop) {
            storage.Drop(item);
        }

        return toDrop.Count;
    }

    internal static void DropAllPowerBanks(GameObject flydo)
    {
        if (flydo == null) {
            return;
        }

        var storageComponents = flydo.GetComponents<Storage>();
        if (storageComponents == null || storageComponents.Length == 0) {
            return;
        }

        foreach (var storage in storageComponents) {
            if (storage == null || storage.storageID != GameTags.ChargedPortableBattery) {
                continue;
            }

            DropPowerBanks(storage, null, "Flydo deconstruct: dropping {0} power bank(s)");
        }
    }
}
