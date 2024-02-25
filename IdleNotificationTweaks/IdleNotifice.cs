// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

namespace IdleNotificationTweaks;

public class IdleNotice {
    public Notification N;
    public System.DateTime IdleStart;

    public IdleNotice(ref Notification n) {
        this.N = n;
        this.IdleStart = System.DateTime.UtcNow;
    }
}
