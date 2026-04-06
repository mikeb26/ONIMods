// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BotTweaks;

internal static class AtomicPowerBanks {
    private static readonly Tag AtomicPowerBankTag = new("SelfChargingElectrobank");

    [ThreadStatic]
    private static bool reentryElectroBankStorageChange;

    // In the base game, the "Atomic" power bank is the Self-Charging Electrobank.
    // (It is a charged portable battery with effectively unlimited lifetime.)
    private static bool IsAtomicPowerBank(GameObject go) {
        if (go == null) {
            return false;
        }

        // SelfChargingElectrobank derives from Electrobank.
        return go.GetComponent<SelfChargingElectrobank>() != null;
    }

    private static void ClearSweepAndClearChoreMarks(GameObject go) {
        if (go == null) {
            return;
        }

        // "Sweep" in ONI is implemented via Movable/MarkedForMove.
        // If we drop an excess atomic bank, ensure it's not immediately re-swept/re-stored.
        try {
            go.GetComponent<Movable>()?.ClearMove();
        } catch {
            // ignore
        }

        // Also clear any "clear" (move to storage) marking.
        try {
            go.GetComponent<Clearable>()?.CancelClearing();
        } catch {
            // ignore
        }
    }

    /// <summary>
    /// Enforce atomic-power-bank-specific rules on a robot's electrobank storage:
    /// - At most 1 atomic power bank may be stored.
    /// - If an atomic power bank is present alongside non-atomic banks, ensure it is first in the list
    ///   so RobotElectroBankMonitor will consume from it.
    ///
    /// Returns true if any modifications were made.
    /// </summary>
    internal static bool EnforceRules(Storage electroBankStorage) {
        if (electroBankStorage == null || electroBankStorage.items == null || electroBankStorage.items.Count == 0) {
            return false;
        }

        // Find atomic banks.
        int firstAtomicIdx = -1;
        var extraAtomics = new List<GameObject>();

        for (int i = 0; i < electroBankStorage.items.Count; i++) {
            var item = electroBankStorage.items[i];
            if (item == null || !item.HasTag(GameTags.ChargedPortableBattery)) {
                continue;
            }

            if (!IsAtomicPowerBank(item)) {
                continue;
            }

            if (firstAtomicIdx < 0) {
                firstAtomicIdx = i;
            } else {
                extraAtomics.Add(item);
            }
        }

        bool changed = false;

        // Drop any extra atomic banks beyond the first.
        if (extraAtomics.Count > 0) {
            changed = true;
            foreach (var go in extraAtomics) {
                try {
                    // Ensure the dropped bank won't be immediately targeted by sweep/move chores.
                    ClearSweepAndClearChoreMarks(go);
                    electroBankStorage.Drop(go);
                } catch (Exception e) {
                    Util.Log("AtomicPowerBanks: failed to drop extra atomic power bank: {0}", e);
                }
            }
        }

        // Ensure atomic bank is in slot 0 so the base game's monitor consumes it.
        // RobotElectroBankMonitor.Instance.ElectroBankStorageChange() uses items[0] as the active bank.
        if (firstAtomicIdx > 0) {
            changed = true;
            var atomic = electroBankStorage.items[firstAtomicIdx];
            electroBankStorage.items.RemoveAt(firstAtomicIdx);
            electroBankStorage.items.Insert(0, atomic);
        }

        return changed;
    }

    private static bool FilterAllowsAtomicPowerBank(RobotElectroBankMonitor.Instance instance,
        System.Collections.Generic.HashSet<Tag> allowedTags = null) {
        if (allowedTags != null) {
            return allowedTags.Contains(AtomicPowerBankTag);
        }

        if (instance == null) {
            return false;
        }

        var filterable = instance.GetComponent<TreeFilterable>();
        return filterable != null && filterable.ContainsTag(AtomicPowerBankTag);
    }

    private static void AddForbiddenTag(ManualDeliveryKG manualDelivery, Tag tag) {
        if (manualDelivery == null) {
            return;
        }

        var current = manualDelivery.ForbiddenTags;
        if (current == null) {
            manualDelivery.ForbiddenTags = new[] { tag };
            return;
        }

        for (int i = 0; i < current.Length; i++) {
            if (current[i] == tag) {
                return;
            }
        }

        var next = new Tag[current.Length + 1];
        for (int i = 0; i < current.Length; i++) {
            next[i] = current[i];
        }
        next[current.Length] = tag;
        manualDelivery.ForbiddenTags = next;
    }

    private static void RemoveForbiddenTag(ManualDeliveryKG manualDelivery, Tag tag) {
        if (manualDelivery == null) {
            return;
        }

        var current = manualDelivery.ForbiddenTags;
        if (current == null || current.Length == 0) {
            return;
        }

        int idx = -1;
        for (int i = 0; i < current.Length; i++) {
            if (current[i] == tag) {
                idx = i;
                break;
            }
        }
        if (idx < 0) {
            return;
        }

        if (current.Length == 1) {
            manualDelivery.ForbiddenTags = null;
            return;
        }

        var next = new Tag[current.Length - 1];
        int j = 0;
        for (int i = 0; i < current.Length; i++) {
            if (i == idx) {
                continue;
            }
            next[j++] = current[i];
        }
        manualDelivery.ForbiddenTags = next;
    }

    private static void SyncAtomicFetchBan(RobotElectroBankMonitor.Instance instance,
        System.Collections.Generic.HashSet<Tag> allowedTags = null) {
        if (instance == null) {
            return;
        }

        var storage = instance.electroBankStorage;
        var manualDelivery = instance.fetchBatteryChore;
        if (storage == null || manualDelivery == null) {
            return;
        }

        // Only interfere if the filter allows atomic banks. If the user doesn't allow them,
        // don't touch ForbiddenTags; the base game + other BotTweaks logic may be dropping them.
        if (!FilterAllowsAtomicPowerBank(instance, allowedTags)) {
            return;
        }

        bool hasAtomic = HasAtomicPowerBank(storage);
        if (hasAtomic) {
            // Prevent infinite deliver/drop loops by disallowing *additional* atomic deliveries.
            AddForbiddenTag(manualDelivery, AtomicPowerBankTag);
        } else {
            // If no atomic is installed, ensure we don't keep forbidding atomic fetches forever.
            RemoveForbiddenTag(manualDelivery, AtomicPowerBankTag);
        }
    }

    /// <summary>
    /// Harmony postfix handler for RobotElectroBankMonitor.Instance.OnFilterChanged.
    /// </summary>
    internal static void OnFilterChanged(RobotElectroBankMonitor.Instance instance,
        System.Collections.Generic.HashSet<Tag> allowedTags) {
        try {
            SyncAtomicFetchBan(instance, allowedTags);
        } catch (Exception e) {
            Util.Log("AtomicPowerBanks: OnFilterChanged error: {0}", e);
        }
    }

    /// <summary>
    /// Harmony postfix handler for RobotElectroBankMonitor.Instance.ElectroBankStorageChange.
    /// Enforces limit/order and keeps fetch bans in sync.
    /// </summary>
    internal static void OnElectroBankStorageChange(RobotElectroBankMonitor.Instance instance) {
        try {
            if (instance == null) {
                return;
            }

            if (reentryElectroBankStorageChange) {
                return;
            }

            var storage = instance.electroBankStorage;
            if (storage == null) {
                return;
            }

            bool changed = EnforceRules(storage);

            // Whether or not the filter changed, ensure the fetch ban is consistent with the
            // current storage contents.
            SyncAtomicFetchBan(instance);

            // If we reordered/dropped, ensure the monitor points at the active bank.
            // (Its implementation is index-0 based.)
            if (changed) {
                reentryElectroBankStorageChange = true;
                instance.ElectroBankStorageChange();
                reentryElectroBankStorageChange = false;
            }
        } catch (Exception e) {
            Util.Log("AtomicPowerBanks: ElectroBankStorageChange error: {0}", e);
        }
    }

    /// <summary>
    /// Harmony prefix handler for RobotElectroBankMonitor.ConsumePower.
    /// Returns true to run the base game method, false to skip it.
    /// </summary>
    internal static bool ConsumePowerPrefix(RobotElectroBankMonitor.Instance smi, float dt) {
        if (smi == null || smi.electroBankStorage == null) {
            return true;
        }

        var storage = smi.electroBankStorage;
        var atomic = FindAtomicPowerBank(storage);
        if (atomic == null) {
            return true; // no atomic bank present, let base game run
        }

        // Ensure the atomic bank is treated as the active bank.
        if (storage.items != null && storage.items.Count > 0) {
            if (storage.items[0] != null) {
                var eb0 = storage.items[0].GetComponent<Electrobank>();
                if (eb0 != atomic) {
                    // Put atomic first.
                    EnforceRules(storage);
                }
            }
        }

        // Consume from the atomic bank.
        float joules = Mathf.Min(dt * Mathf.Abs(smi.bankAmount.GetDelta()), atomic.Charge);
        atomic.RemovePower(joules, dropWhenEmpty: true);

        // Keep the amount in sync with the active bank.
        smi.electrobank = atomic;
        smi.bankAmount.value = atomic.Charge;

        return false;
    }

    internal static bool HasAtomicPowerBank(Storage electroBankStorage) {
        if (electroBankStorage == null || electroBankStorage.items == null) {
            return false;
        }

        foreach (var item in electroBankStorage.items) {
            if (item == null || !item.HasTag(GameTags.ChargedPortableBattery)) {
                continue;
            }
            if (IsAtomicPowerBank(item)) {
                return true;
            }
        }

        return false;
    }

    internal static Electrobank FindAtomicPowerBank(Storage electroBankStorage) {
        if (electroBankStorage == null || electroBankStorage.items == null) {
            return null;
        }

        foreach (var item in electroBankStorage.items) {
            if (item == null || !item.HasTag(GameTags.ChargedPortableBattery)) {
                continue;
            }

            if (IsAtomicPowerBank(item)) {
                return item.GetComponent<Electrobank>();
            }
        }

        return null;
    }
}
