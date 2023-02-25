// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

namespace IdleNotificationTweaks;

public static class Util {
    public static void Log(string fmt, params object[] args) {
        Debug.LogFormat(string.Format("[IdleNotificationTweaks] " + fmt, args));
    }

    public static void LogDbg(string fmt, params object[] args) {
#if DEBUG
        Util.Log(fmt, args);
#endif
    }
}
