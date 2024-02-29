// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

namespace IdleNotificationTweaks;

public static class Util {
    public static void Log(string fmt, params object[] args) {
        Debug.LogFormat(string.Format("[IdleNT] " + fmt, args));
    }

    public static void LogDbg(string fmt, params object[] args) {
#if DEBUG
        Util.Log(fmt, args);
#endif
    }

    public static string Version() {
        return "%VERSION%";
    }

    public static bool IsModEnabled(string staticID) {
        foreach(var mod in Global.Instance.modManager.mods) {
            if (mod.staticID == staticID && mod.IsActive()) {
                return true;
            }
        }

        return false;
    }

    public static void LogStackTrace() {
        Util.LogDbg("{0}", Environment.StackTrace);
    }
}
