// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using KSerialization;
using PeterHan.PLib.Options;
using System.Collections.Generic;
using System.Linq;
using System;
namespace IdleNotificationTweaks;

public class GameState
{
    public List<Notification> trackedNotifications;

    private IdleOptions opts = null;
    private System.DateTime lastPause;

    public GameState() {
        this.opts = POptions.ReadSettings<IdleOptions>();
        if (opts == null) {
            this.opts = new IdleOptions();
        }

        if (trackedNotifications == null) {
            trackedNotifications = new List<Notification>();
        }
        lastPause = System.DateTime.UtcNow;

        Util.Log("Game started; tracking idle dupes. Opts: {0}", opts);
    }

    public void AddDupe(ref MinionIdentity minion) {
        Util.LogDbg("adddupe: tracking dupeId:{0}", minion.GetInstanceID());

        minion.gameObject.AddComponent<SuppressIdleButton>();
    }

    public void RemoveDupe(ref MinionIdentity minion) {
        Util.LogDbg("removedupe: ceased tracking dupeId:{0}", minion.GetInstanceID());
    }

    /* Process a new notification that is about to be displayed to the screen.
     * returns true if the notification should be displayed; false otherwise.
     * If we return false we are responsible for tracking the notification ourselves
     * (potentially reposting it if the user unsuppresses)
     */
    public bool AddNotification(ref Notification n) {
        if (!isIdleNotification(ref n)) {
            Util.LogDbg("addnote: ignore type:{0} text:{1}", n.Type, n.titleText);

            return true;
        }

        if (n.clickFocus.TryGetComponent(out MinionIdentity minion) == false) {
            Util.Log("addnote: could not find associated dupe");

            return true;
        }
        if (minion.TryGetComponent(out SuppressIdleButton button) == false) {
            Util.Log("addnote: could not find dupeId:{0} idleSuppressed",
                     minion.GetInstanceID());

            return true;
        }

        Util.LogDbg("addnote: dupeId:{0} idleSuppressed:{1}", minion.GetInstanceID(),
                    button.IsIdleSuppressed);

        trackedNotifications.Add(n);

        if (button.IsIdleSuppressed || isInBusyRocket(ref minion)) {
            return false;
        }

        // pause processing is deferred until NotificationDisplayed()

        return true;
    }

    /* Post-process a notification has been displayed to the screen */
    public void NotificationDisplayed(ref Notification n) {
        if (!isIdleNotification(ref n)) {
            Util.LogDbg("dispnote: ignore type:{0} text:{1}", n.Type, n.titleText);

            return;
        }

        if (n.clickFocus.TryGetComponent(out MinionIdentity minion) == false) {
            Util.Log("dispnote: could not find associated dupe");

            return;
        }
        if (minion.TryGetComponent(out SuppressIdleButton button) == false) {
            Util.Log("dispnote: could not find dupeId:{0} idleSuppressed", minion.GetInstanceID());

            return;
        }

        if (!button.IsIdleSuppressed && !isInBusyRocket(ref minion) && opts.PauseOnIdle &&
            !SpeedControlScreen.Instance.IsPaused) {

            var now = System.DateTime.UtcNow;
            TimeSpan diff = now.Subtract(lastPause);
            if (diff.TotalSeconds >= opts.PauseCooldown) {
                Util.LogDbg("dispnote: pausing for dupeId:{0}", minion.GetInstanceID());
                SpeedControlScreen.Instance.Pause();
                lastPause = now;
            } else {
                Util.LogDbg("dispnote: skipping pause for dupeId:{0} due to cooldown", minion.GetInstanceID());
            }
        }
    }

    public void RemoveNotification(ref Notification n) {
        if (!isIdleNotification(ref n)) {
            Util.LogDbg("removenote: ignore type:{0} text:{1}", n.Type, n.titleText);

            return;
        }

        if (n.clickFocus.TryGetComponent(out MinionIdentity minion) == false) {
            Util.Log("removenote: could not find associated dupe");

            return;
        }

        Util.LogDbg("removenote: dupeId:{0}", minion.GetInstanceID());

        trackedNotifications.Remove(n);
    }

    /* Recalculate which tracked notifications should or should not be displayed;
     * this is invoked whenever the player clicks the dupe's idle suppress button. This
     * recalculation is done by implicitly triggering O(n) RemoveNotification() and
     * O(n) AddNotification() calls allowing us to reuse the same logic to squelch
     * notifications when they are first created.
     */
    public void RefreshNotifications() {
        Util.LogDbg("refreshnote");

        List<Notification> tmpList = trackedNotifications.ToList();
        trackedNotifications.Clear();

        foreach (var n in tmpList) {
            Notifier notifier = n.Notifier;
            n.Clear();
            notifier.Add(n);
        }
    }

    private static bool isIdleNotification(ref Notification n) {
        if (n.Type != NotificationType.BadMinor) {
            return false;
        }

        if (n.titleText == STRINGS.DUPLICANTS.STATUSITEMS.IDLE.NOTIFICATION_NAME) {
            return true;
        }

        return false;
    }

    /* @todo do we need to hook Clustercraft.SetCraftStatus() and/or
     * ResourceHarvestModule.{Add,Remove}HarvestStatusItems() in order to trigger a
     * RefreshNotifications()?
     */
    private bool isInBusyRocket(ref MinionIdentity m) {
        if (DlcManager.IsPureVanilla()) {
            return false;
        }
        if (!DlcManager.IsExpansion1Active()) {
            return false;
        }
        if (!opts.SuppressIdleInRockets) {
            return false;
        }
        var world = m.GetMyWorld();
        if (world == null) {
            Util.Log("isbusyrocket: could not find world for dupeId:{0}", m.GetInstanceID());
            return false;
        }
        if (!world.IsModuleInterior) {
            // dupe is not in a rocket
            return false;
        }
        if (world.TryGetComponent(out Clustercraft ship) == false) {
            Util.Log("isbusyrocket: could not find ship for dupeId:{0} worldId:{1}",
                     m.GetInstanceID(), world.id);
            return false;
        }

        Util.LogDbg("isbusyrocket: dupeId:{0} worldId:{1} shipStatus:{2} fueled:{3} mining:{4}",
                    m.GetInstanceID(), world.id, ship.Status, ship.IsTravellingAndFueled(),
                    ship.HasTag(GameTags.POIHarvesting));

        if (ship.Status == Clustercraft.CraftStatus.Grounded) {
            return false;
        } else if (ship.Status != Clustercraft.CraftStatus.InFlight) {
            // rocket is launching or landing
            return true;
        } else if (ship.IsTravellingAndFueled()) {
            // rocket is moving
            return true;
        } else if (ship.HasTag(GameTags.POIHarvesting)) {
            // rocket is mining
            return true;
        } // else rocket is idle in space

        return false;
    }
}
