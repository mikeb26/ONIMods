// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using KSerialization;
using PeterHan.PLib.Options;
using System.Collections.Generic;
using System.Linq;
namespace IdleNotificationTweaks;

public class GameState
{
    public List<Notification> trackedNotifications;

    private IdleOptions opts = null;

    public GameState() {
        this.opts = POptions.ReadSettings<IdleOptions>();
        if (opts == null) {
            this.opts = new IdleOptions();
        }

        if (trackedNotifications == null) {
            trackedNotifications = new List<Notification>();
        }

        Util.Log("Game started; tracking idle dupes. Opts: {0}", opts);
    }

    public void AddDupe(ref MinionIdentity minion) {
        Util.LogDbg("Tracking dupe name:{0} id:{1}", minion.GetProperName(),
                    minion.GetInstanceID());

        minion.gameObject.AddComponent<SuppressIdleButton>();
    }

    public void RemoveDupe(ref MinionIdentity minion) {
        Util.LogDbg("Ceased tracking dupe id:{0}", minion.GetInstanceID());
    }

    /* Process a new notification that is about to be posted to the screen.
     * returns true if the notification should be displayed; false otherwise.
     * If we return false we are responsible for tracking the notification ourselves
     * (potentially reposting it if the user unsuppresses)
     */
    public bool AddNotification(ref Notification n) {
        if (!isIdleNotification(ref n)) {
            Util.LogDbg("add: Ignoring notification type:{0} text:{1}", n.Type, n.titleText);

            return true;
        }

        if (n.clickFocus.TryGetComponent(out MinionIdentity minion) == false) {
            Util.Log("add: Could not find dupe associated with Idle notification");

            return true;
        }
        if (minion.TryGetComponent(out SuppressIdleButton button) == false) {
            Util.Log("add: Could not find dupe id:{0} idle suppression button",
                     minion.GetInstanceID());

            return true;
        }

        Util.LogDbg("add: tracking for dupe id:{0} suppression button state:{1}",
                    minion.GetInstanceID(), button.IsIdleSuppressed);

        trackedNotifications.Add(n);

        /* @todo add check for inside rocket */
        if (button.IsIdleSuppressed) {
            return false;
        }

        if (opts.PauseOnIdle && !SpeedControlScreen.Instance.IsPaused) {
            SpeedControlScreen.Instance.Pause();
        }

        return true;
    }

    public void RemoveNotification(ref Notification n) {
        if (!isIdleNotification(ref n)) {
            Util.LogDbg("remove: Ignoring notification type:{0} text:{1}", n.Type, n.titleText);

            return;
        }

        if (n.clickFocus.TryGetComponent(out MinionIdentity minion) == false) {
            Util.Log("remove: Could not find dupe associated with Idle notification");

            return;
        }
        if (minion.TryGetComponent(out SuppressIdleButton button) == false) {
            Util.Log("remove: Could not find dupe id:{0} idle suppression button",
                     minion.GetInstanceID());

            return;
        }

        Util.LogDbg("remove: tracking for dupe id:{0} suppression button state:{1}",
                    minion.GetInstanceID(), button.IsIdleSuppressed);

        trackedNotifications.Remove(n);
    }

    /* Recalculate which tracked notifications should or should not be displayed;
     * this is invoked whenever the player clicks the dupe's idle suppress button. This
     * recalculation is done by implicitly triggering O(n) RemoveNotification() and
     * O(n) AddNotification() calls allowing us to reuse the same logic to squelch
     * notifications when they are first created.
     */
    public void RefreshNotifications() {
        Util.LogDbg("refreshing notifications");

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
}
