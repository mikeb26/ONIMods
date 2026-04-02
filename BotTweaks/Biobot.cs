// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;

namespace BotTweaks;

internal sealed class Biobot : TrackedRobot {
    internal static readonly Tag PrefabTag = new Tag("MorbRover");

    internal override RobotType RobotType => RobotType.Biobot;

    internal override bool CanEnablePowerbank() {
        var opts = Mod.Instance?.gameState?.opts;
        if (opts == null || opts.biobotExpireDisposition != ExpiredDisposition.EnablePowerBanks) {
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
        ApplyExpirationDisposition(this.gameObject);
    }

    internal override void ApplyExistingDeadRobotDisposition() {
        base.ApplyExistingDeadRobotDisposition();
        ApplyExpirationDisposition(gameObject);
    }

    private static void ApplyExpirationDisposition(GameObject go) {
        ExpirationDispositionApplier.Apply(
            go,
            Mod.Instance.gameState.opts.biobotExpireDisposition,
            IsBatteryExpired,
            cancelDeconstructWhenDoNothing: true
        );
    }

    // For existing saves, DeathMonitor may not be fully initialized at the time we scan,
    // so we also fall back to checking the relevant battery amount.
    //
    internal static bool IsBatteryExpired(GameObject go) {
        if (ExpiredRobotBehavior.IsDeadBatteryDeath(go)) {
            return true;
        }

        var amt = Db.Get().Amounts.InternalBioBattery.Lookup(go);
        return amt != null && amt.value <= 0f;
    }

    internal static bool HandleDeconstructOnDeathEvent(GameObject go) {
        if (go == null) {
            return false;
        }

        if (go.GetComponent<Biobot>() == null) {
            return true;
        }

        if (!ExpiredRobotBehavior.IsDeadBatteryDeath(go)) {
            // Not an "expiration"; preserve base-game behavior.
            return true;
        }

        ExpirationDispositionApplier.Apply(
            go,
            Mod.Instance.gameState.opts.biobotExpireDisposition,
            // This handler is only for the base-game "expiration" event.
            ExpiredRobotBehavior.IsDeadBatteryDeath,
            cancelDeconstructWhenDoNothing: true
        );

        // Suppress the base game handler for "expiration" only.
        return false;
    }
}
