// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

namespace BotTweaks;

using UnityEngine;

internal sealed class Rover : TrackedRobot {
    internal static readonly Tag PrefabTag = new Tag("ScoutRover");

    internal override RobotType RobotType => RobotType.Rover;

    internal override bool CanEnablePowerbank() {
        var opts = Mod.Instance?.gameState?.opts;
        if (opts == null || opts.roverExpireDisposition != ExpiredDisposition.EnablePowerBanks) {
            return false;
        }

        return IsBatteryExpired(gameObject);
    }

    internal override bool IsPowerbankEnabled() => PowerBankEnabler.IsPowerBankEnabled(gameObject);

    internal override void EnablePowerbank() {
        PowerBankEnabler.EnablePowerBanksIfEligible(this);
    }

    internal override void OnDeathAnimComplete(StateMachine.Instance smi) {
        base.OnDeathAnimComplete(smi);
        ApplyExpirationDisposition(gameObject);
    }

    internal override void ApplyExistingDeadRobotDisposition() {
        base.ApplyExistingDeadRobotDisposition();
        ApplyExpirationDisposition(gameObject);
    }

    private static void ApplyExpirationDisposition(GameObject go) {
        ExpirationDispositionApplier.Apply(
            go,
            Mod.Instance.gameState.opts.roverExpireDisposition,
            IsBatteryExpired,
            cancelDeconstructWhenDoNothing: false
        );
    }

    // For existing saves, DeathMonitor may not be fully initialized at the time we scan, so
    // we also fall back to checking the relevant battery amount.
    internal static bool IsBatteryExpired(GameObject go) {
        if (ExpiredRobotBehavior.IsDeadBatteryDeath(go)) {
            return true;
        }

        var amt = Db.Get().Amounts.InternalChemicalBattery.Lookup(go);
        return amt != null && amt.value <= 0f;
    }
}
