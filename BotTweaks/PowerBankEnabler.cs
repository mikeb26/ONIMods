// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BotTweaks;

/// <summary>
/// Adds Flydo-like Power Bank UI/behavior to non-Flydo robots after their internal battery expires.
///
/// This intentionally reuses the game's stock components:
/// - Storage (with storageID ChargedPortableBattery)
/// - TreeFilterable + ManualDeliveryKG (for the filter UI and delivery requests)
/// - RobotElectroBankMonitor (manages battery meter + NoElectroBank behaviour tag)
/// - RobotElectroBankDeadStates (plays "dead_battery" anim + revives when a bank is delivered)
///
/// Key design detail:
/// We remove the DeathMonitor DeadBattery cause so the robot is not tagged Dead, and instead we
/// clamp its internal battery amount to a small epsilon so the base RobotBatteryMonitor doesn't
/// re-kill it.
/// </summary>
internal static class PowerBankEnabler {
    // Keep internal battery slightly above 0 so RobotBatteryMonitor doesn't kill.
    private const float INTERNAL_BATT_EPSILON = 1f;

    // For ManualDeliveryKG settings. Flydo uses 21kg.
    private const float POWERBANK_STORAGE_CAPACITY_KG = 21f;

    internal static void EnablePowerBanksIfEligible(TrackedRobot tracked) {
        if (!tracked.CanEnablePowerbank()) {
            return;
        }

        EnablePowerBanks(tracked.gameObject);
    }

    internal static bool IsPowerBankEnabled(GameObject go) {
        return go.GetComponent<TreeFilterable>() != null
            && FindElectrobankStorage(go) != null
            && HasElectroBankMonitor(go);
    }

    internal static void EnablePowerBanks(GameObject go) {
        if (go == null) {
            return;
        }

        // If already enabled, no-op.
        if (IsPowerBankEnabled(go)) {
            Util.LogDbg("EnablePowerBanks: already enabled for '{0}'", go.name);
            return;
        }

        // IMPORTANT:
        // RobotElectroBankMonitor depends on the InternalElectroBank amount existing.
        // Flydo prefabs have it via Modifiers.initialAmounts.
        EnsureInternalElectroBankAmount(go);

        var batteryStorage = EnsureElectrobankStorage(go);
        EnsureWakeOnElectrobankDelivery(go, batteryStorage);
        EnsureFilterAndDelivery(go, batteryStorage);

        var rebmDef = EnsureElectroBankMonitor(go);
        EnsureElectroBankDeadBehaviourChore(go);

        PreventInternalBatteryDeathAndRevive(go);
        StartStateMachines(go, rebmDef);
        RefreshUI(go);

        Util.LogDbg("EnablePowerBanks: enabled power bank system for '{0}'", go.name);
    }

    private static Storage EnsureElectrobankStorage(GameObject go) {
        // Ensure we have the standard Flydo-style storage for ChargedPortableBattery
        return FindOrCreateElectrobankStorage(go);
    }

    private static void EnsureFilterAndDelivery(GameObject go, Storage batteryStorage) {
        if (go == null || batteryStorage == null) {
            return;
        }

        // Filter UI + manual delivery request
        var filterable = go.AddOrGet<TreeFilterable>();
        filterable.storageToFilterTag = batteryStorage.storageID;
        filterable.dropIncorrectOnFilterChange = false;
        filterable.tintOnNoFiltersSet = false;

        var manualDelivery = go.AddOrGet<ManualDeliveryKG>();
        manualDelivery.SetStorage(batteryStorage);
        manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
        manualDelivery.capacity = POWERBANK_STORAGE_CAPACITY_KG;
        manualDelivery.refillMass = POWERBANK_STORAGE_CAPACITY_KG;
        manualDelivery.MinimumMass = 1f;
        // Match Flydo's chore type.
        manualDelivery.choreTypeIDHash = Db.Get().ChoreTypes.RepairFetch.IdHash;
        manualDelivery.allowPause = true;
    }

    private static RobotElectroBankMonitor.Def EnsureElectroBankMonitor(GameObject go) {
        if (go == null) {
            return null;
        }

        // Add the electrobank monitor state machine so the robot consumes banks and toggles NoElectroBank
        // NOTE: RobotElectroBankMonitor is a "regular" SMI attached to the robot (via StateMachineController).
        // NOTE2: The monitor tries to override a battery meter symbol in the robot's anim build.
        // Rovers/biobots do not have Flydo's battery meter symbols, so we install a Harmony guard
        // (see HooksPowerBanks) to skip the override.
        var rebmDef = go.AddOrGetDef<RobotElectroBankMonitor.Def>();
        rebmDef.lowBatteryWarningPercent = 0.2f;
        return rebmDef;
    }

    private static void StartStateMachines(GameObject go, RobotElectroBankMonitor.Def rebmDef) {
        if (go == null) {
            return;
        }

        // Start newly added SMIs immediately.
        var smc = go.GetComponent<StateMachineController>();
        EnsureStateMachineStarted<RobotElectroBankMonitor.Instance>(smc, rebmDef);
        smc?.StartSMIS();
    }

    private static void RefreshUI(GameObject go) {
        // Ensure the user menu refreshes so the storage/filter UI shows up.
        var robotAi = go != null ? go.GetSMI<RobotAi.Instance>() : null;
        robotAi?.RefreshUserMenu();
    }

    private static void EnsureStateMachineStarted<TInstance>(StateMachineController smc, StateMachine.BaseDef def)
        where TInstance : StateMachine.Instance {
        if (smc == null || def == null) {
            return;
        }

        // If it already exists, nothing to do.
        if (smc.GetSMI<TInstance>() != null) {
            return;
        }

        var smi = def.CreateSMI(smc);
        smi?.StartSM();
    }

    private static Storage FindElectrobankStorage(GameObject go) {
        var storages = go.GetComponents<Storage>();
        if (storages == null) {
            return null;
        }

        foreach (var s in storages) {
            if (s != null && s.storageID == GameTags.ChargedPortableBattery) {
                return s;
            }
        }

        return null;
    }

    private static Storage FindOrCreateElectrobankStorage(GameObject go) {
        var existing = FindElectrobankStorage(go);
        if (existing != null) {
            // Ensure it's configured similarly to Flydo.
            existing.showInUI = true;
            existing.showDescriptor = true;
            if (existing.storageFilters == null || existing.storageFilters.Count == 0) {
                existing.storageFilters = new List<Tag> { GameTags.ChargedPortableBattery };
            }
            existing.storageID = GameTags.ChargedPortableBattery;
            return existing;
        }

        // Create a new storage component to hold power banks.
        var storage = go.AddComponent<Storage>();
        storage.storageID = GameTags.ChargedPortableBattery;
        storage.showInUI = true;
        // Improves hover card to show storage contents (matches player expectation for Flydo-style battery storage).
        storage.showDescriptor = true;
        storage.storageFilters = new List<Tag> { GameTags.ChargedPortableBattery };
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Insulate,
        });
        return storage;
    }

    private static bool HasElectroBankMonitor(GameObject go) {
        var smc = go.GetComponent<StateMachineController>();
        if (smc == null) {
            return false;
        }

        // If the def exists, the SMI will exist (or be startable).
        return smc.GetDef<RobotElectroBankMonitor.Def>() != null;
    }

    private static void PreventInternalBatteryDeathAndRevive(GameObject go) {
        // RobotBatteryMonitor's deadBattery state has no transitions out, so if the rover/biobot
        // already reached it, we must explicitly move it back to a draining state.
        //
        // Additionally, when a robot is converted to electrobank power, its internal battery becomes
        // irrelevant. If we let RobotBatteryMonitor continue to evaluate the internal battery level,
        // it may keep the robot in low-battery / needs-recharge states and keep the red status item
        // visible even while the robot is running on a power bank.
        var rbm = go.GetSMI<RobotBatteryMonitor.Instance>();
        if (rbm != null) {
            // Ensure non-zero before we bounce out.
            if (rbm.amountInstance != null && rbm.amountInstance.value <= 0f) {
                rbm.amountInstance.value = INTERNAL_BATT_EPSILON;
            }

            // Always park the battery monitor in the "highBattery" state for converted robots.
            // (Harmony patches in HooksPowerBanks ensure it stays there.)
            if (!rbm.IsInsideState(rbm.sm.drainingStates.highBattery)) {
                rbm.GoTo(rbm.sm.drainingStates.highBattery);
            }
        }

        // Clamp (and pause) internal battery above 0 so RobotBatteryMonitor doesn't immediately re-enter deadBattery.
        // We only support Rover/Biobot conversions.
        AmountInstance amt = Db.Get().Amounts.InternalChemicalBattery.Lookup(go)
            ?? Db.Get().Amounts.InternalBioBattery.Lookup(go);
        if (amt != null) {
            if (amt.value <= 0f) {
                amt.value = INTERNAL_BATT_EPSILON;
            }
            // Stop any further depletion from the internal battery now that we're converting.
            // (Otherwise it can tick back down to 0 and re-trigger death.)
            amt.paused = true;
        }

        // If the robot already entered DeathMonitor's dead state, simply clearing tags/cause isn't enough:
        // DeathMonitor has no transitions out of dead states. We must explicitly reset it back to alive.
        var dm = go.GetSMI<DeathMonitor.Instance>();
        if (dm != null) {
            var deadBattery = Db.Get().Deaths.DeadBattery;
            if (deadBattery != null && dm.sm.death.Get(dm) == deadBattery) {
                dm.sm.death.Set(null, dm);
            }

            // "Revive" if it is currently dead (or was dead-battery killed).
            // NOTE: DeathMonitor.IsDead() only checks the duplicant "dead" state; non-dupe
            // creatures (including robots) use dead_creature.
            if (dm.IsDead() || dm.IsInsideState(dm.sm.dead_creature) || go.HasTag(GameTags.Dead)) {
                dm.GoTo(dm.sm.alive);
            }
        }

        // Resume brain just in case it was suspended due to death/behaviour transitions.
        go.GetComponent<Brain>()?.Resume("BotTweaks: EnablePowerBanks");

        // Just in case anything already applied the Dead tag, remove it.
        var kpid = go.GetComponent<KPrefabID>();
        if (kpid != null && go.HasTag(GameTags.Dead)) {
            kpid.RemoveTag(GameTags.Dead);
        }
        if (go.HasTag(GameTags.Corpse)) {
            go.RemoveTag(GameTags.Corpse);
        }

        // Clear common death/disable tags.
        if (go.HasTag(GameTags.Dying)) {
            go.RemoveTag(GameTags.Dying);
        }
        if (go.HasTag(GameTags.Creatures.Die)) {
            go.RemoveTag(GameTags.Creatures.Die);
        }

        // RobotAi does not automatically transition from dead -> alive when the Dead tag is removed.
        // If the robot has ever entered RobotAi.dead, its brain will remain toggled off until we
        // explicitly return it to an alive state.
        var ai = go.GetSMI<RobotAi.Instance>();
        if (ai != null && ai.IsInsideState(ai.sm.dead)) {
            ai.GoTo(ai.sm.alive.normal);
            ai.RefreshUserMenu();
        }
    }

    private static void EnsureWakeOnElectrobankDelivery(GameObject go, Storage batteryStorage) {
        if (go == null || batteryStorage == null) {
            return;
        }

        // Avoid double-hooking.
        var existing = go.GetComponent<ElectrobankWakeOnDelivery>();
        if (existing != null) {
            existing.SetStorage(batteryStorage);
            return;
        }

        var hook = go.AddComponent<ElectrobankWakeOnDelivery>();
        hook.SetStorage(batteryStorage);
    }

    /// <summary>
    /// Runtime-only helper that listens for electrobank deliveries and forces the rover/biobot out
    /// of any dead/shutdown SM states.
    ///
    /// We do this because the base-game "internal battery" death path pushes the robot through
    /// RobotBatteryMonitor.deadBattery -> DeathMonitor.dead_creature -> RobotAi.dead.
    /// Once in those states, simply adding the Flydo power-bank systems isn't sufficient; we must
    /// explicitly bounce the SMIs back to alive states.
    /// </summary>
    private sealed class ElectrobankWakeOnDelivery : KMonoBehaviour {
        private Storage storage;
        private bool hooked;

        internal void SetStorage(Storage s) {
            storage = s;
            Hook();
        }

        protected override void OnSpawn() {
            base.OnSpawn();
            Hook();
        }

        private void Hook() {
            if (hooked || storage == null) {
                return;
            }
            hooked = true;

            // Storage invokes this on store/transfer without needing a state machine / brain.
            storage.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage.OnStorageChange, new Action<GameObject>(OnStorageChanged));

            // Also run once in case the robot already has a bank (e.g. load, debug, etc.).
            TryWake();
        }

        private void OnStorageChanged(GameObject _) {
            TryWake();
        }

        private void TryWake() {
            if (storage == null || gameObject == null) {
                return;
            }

            // Only wake once a bank is actually present.
            if (!storage.Has(GameTags.ChargedPortableBattery)) {
                return;
            }

            // Ensure NoElectroBank is cleared so the behaviour chore can complete.
            gameObject.GetComponent<KPrefabID>()?.RemoveTag(GameTags.Robots.Behaviours.NoElectroBank);

            // Re-run our revive logic now that a bank is present.
            PreventInternalBatteryDeathAndRevive(gameObject);

            // If the electrobank monitor exists, force it to re-evaluate now.
            var rebm = gameObject.GetSMI<RobotElectroBankMonitor.Instance>();
            rebm?.ElectroBankStorageChange();
        }
    }

    /// <summary>
    /// RobotElectroBankMonitor expects the InternalElectroBank amount (and its attributes) to exist.
    /// Flydo prefabs include it via Modifiers.initialAmounts; rovers/biobots do not.
    /// </summary>
    private static void EnsureInternalElectroBankAmount(GameObject go) {
        var amount = Db.Get().Amounts.InternalElectroBank;
        if (amount == null) {
            return;
        }

        if (amount.Lookup(go) != null) {
            return;
        }

        var modifiers = go.GetComponent<Modifiers>();
        if (modifiers == null || modifiers.amounts == null) {
            return;
        }

        Util.LogDbg("EnablePowerBanks: adding missing InternalElectroBank amount to '{0}'", go.name);

        // Create and activate the amount instance.
        var inst = new AmountInstance(amount, go);
        modifiers.amounts.Add(inst);

        // Match the Flydo base trait values.
        // (120000J capacity, -50J/s depletion)
        var maxMod = new AttributeModifier(amount.maxAttribute.Id, 120000f, "BotTweaks: Power Bank");
        var deltaMod = new AttributeModifier(amount.deltaAttribute.Id, -50f, "BotTweaks: Power Bank");

        go.GetAttributes()?.Add(maxMod);
        go.GetAttributes()?.Add(deltaMod);

        // Start at 0; RobotElectroBankMonitor will sync to the installed electrobank charge.
        inst.value = 0f;
    }

    /// <summary>
    /// Ensure the Flydo-style NoElectroBank behaviour chore exists on this robot.
    /// This is what plays the shutdown animation and, critically, resumes the brain on power-up.
    /// </summary>
    private static void EnsureElectroBankDeadBehaviourChore(GameObject go) {
        if (go == null) {
            return;
        }

        var kpid = go.GetComponent<KPrefabID>();
        var provider = go.GetComponent<ChoreProvider>();
        if (kpid == null || provider == null) {
            return;
        }

        // Avoid duplicates.
        foreach (var kvp in provider.choreWorldMap) {
            var chores = kvp.Value;
            if (chores == null) {
                continue;
            }
            foreach (var chore in chores) {
                if (chore is ChoreTable.ChoreTableChore<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance>) {
                    return;
                }
            }
        }

        Util.LogDbg("EnablePowerBanks: adding RobotElectroBankDeadStates chore to '{0}'", go.name);

        // Create the behaviour chore using the stock implementation (normally created via ChoreTable.Builder).
        // We reuse the stock Die chore type to inherit sensible interrupt priority settings.
        _ = new ChoreTable.ChoreTableChore<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance>(
            new RobotElectroBankDeadStates.Def(),
            Db.Get().ChoreTypes.Die,
            kpid
        );
    }
}
