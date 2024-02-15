// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;

namespace QuickStart;

public class GameState
{
    private QuickStartOptions opts;
    private bool baseGameOnly;
    private Items items;
    private DupeBufs dupeBufs;
    private TechBufs techBufs;
    private SavedState savedState;
    private bool needDeferredDupeUpgrade;
    private float firstDeferredDupeUpgradeTime;

    public GameState() {
        this.opts = null;
        if (DlcManager.IsPureVanilla() || !DlcManager.IsExpansion1Active()) {
            this.baseGameOnly = true;
        } else {
            this.baseGameOnly = false;
        }
        this.items = null;
        this.techBufs = new TechBufs(this.baseGameOnly);
        this.dupeBufs = null;
        this.savedState = null;
        this.needDeferredDupeUpgrade = false;
        this.firstDeferredDupeUpgradeTime = 0;
    }

    public void LoadSavedStateAndOpts(ref Game game) {
        this.savedState = game.gameObject.AddOrGet<SavedState>();
        this.opts = POptions.ReadSettings<QuickStartOptions>();
        if (this.opts == null) {
            this.opts = new QuickStartOptions();
        }
    }

    public void LogNewGame() {
        this.needDeferredDupeUpgrade = false;
        this.firstDeferredDupeUpgradeTime = 0;
        if (this.savedState.GameWasModified) {
            Util.Log("Loaded game already modified by Quick Start {0} {1}",
                     this.savedState.version, this.savedState.opts);
        } else {
            this.savedState.version = Util.Version();
            this.savedState.opts = this.opts;

            Util.Log("Loaded game unmodified by Quick Start with {0}", this.opts);
        }
    }

    public void ApplyStartOptions() {
        if (this.savedState.GameWasModified ||
            (!isNewlyCreatedGame() && this.opts.scope == Scope.NewGameOnly) ||
            this.opts.startLevel == StartLevel.Normal) {
            return;
        }

        // can't init Items or DupeBufs within GameState() constructor because their
        // dependencies aren't yet initialized at GameState() time.
        if (this.items == null) {
            this.items = new Items(this.baseGameOnly);
        }
        if (this.dupeBufs == null) {
            this.dupeBufs = new DupeBufs(this.baseGameOnly);
        }

        this.items.Spawn(this.opts);
        this.techBufs.UnlockResearch(this.opts);
        if (this.dupeBufs.UpgradeAll(this.opts) == 0) {
            Util.Log("Deferring dupe upgrades");

            this.needDeferredDupeUpgrade = true;
        }

        this.savedState.GameWasModified = true;
    }

    public void MaybeUpgradeDupe(ref MinionIdentity minion) {
        if (this.needDeferredDupeUpgrade == false ||
            minion.addToIdentityList == false) {
            return;
        }

        // only upgrade the initial set of dupes
        if (this.firstDeferredDupeUpgradeTime == 0) {
            this.firstDeferredDupeUpgradeTime = GameClock.Instance.GetTimeInCycles();
        }
        if (this.firstDeferredDupeUpgradeTime != GameClock.Instance.GetTimeInCycles()) {
            this.needDeferredDupeUpgrade = false;
            Util.Log("Not upgrading new dupe(id:{0}) and shutting off future dupe upgrades",
                     minion.GetInstanceID());
            return;
        }

        this.dupeBufs.UpgradeOne(this.opts, minion);
    }

    private bool isNewlyCreatedGame() {
        if (GameClock.Instance.GetTimeInCycles() > 1.01) {
            return false;
        }

        return true;
    }
}
