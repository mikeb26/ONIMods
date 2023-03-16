// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using Klei;
using UnityEngine;

namespace CGSM;

public static class Util {
    public static void Log(string fmt, params object[] args) {
        Debug.LogFormat(string.Format("[CGSM] " + fmt, args));
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

    public static void DumpGO(GameObject go) {
        var goComponents = new Dictionary<int, Component>();

        Util.LogDbg("GameObject: name:{0} id:{1} type:{2}", go.name, go.GetInstanceID(),
                    go.GetType());
        Util.LogDbg("  layer:{0}", go.layer);
        Util.LogDbg("  scene:{0}", go.scene);
        Util.LogDbg("  tag:{0}", go.tag);
        Util.LogDbg("  components:");
        foreach(var component in go.GetComponents(typeof(Component))) {
            DumpComponent("    ", component);
            goComponents[component.GetInstanceID()] = component;
        }
        Util.LogDbg("  parent Components:");
        foreach(var component in go.GetComponentsInParent(typeof(Component))) {
            if (goComponents.ContainsKey(component.GetInstanceID())) {
                continue;
            }
            DumpComponent("    ", component);
        }
        Util.LogDbg("  child Components:");
        foreach(var component in go.GetComponentsInChildren(typeof(Component))) {
            if (goComponents.ContainsKey(component.GetInstanceID())) {
                continue;
            }
            DumpComponent("    ", component);
        }
    }

    public static void DumpComponent(string prefix, Component c) {
        Util.LogDbg("{0}name:{1} id:{2} type:{3}", prefix, c.name, c.GetInstanceID(), c.GetType());
        switch (c) {
        case KButton b:
            Util.LogDbg("{0}  button:{1} fgImage:{2} bgImage:", prefix, b.ToString(), b.fgImage, b.bgImage);
            break;
        case LocText t:
            Util.LogDbg("{0}  text.key:{1} text.text:{2}", prefix, t.key, t.text);
            break;
        case UnityEngine.UI.Image i:
            Util.LogDbg("{0}  sprite:{1} overrideSprite:{2} type:{3}", prefix, i.sprite,
                        i.overrideSprite, i.type);
            break;
        }
    }
}
