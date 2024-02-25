// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.Text;
using System.Reflection;
using System;

namespace QuickStart;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/QuickStart")]
public sealed class QuickStartOptions {
    [Option("STRINGS.UI.FRONTEND.QUICKSTART.STARTLEVEL_OPT", "STRINGS.UI.FRONTEND.QUICKSTART.STARTLEVEL_OPT_DESC")]
    [JsonProperty]
    public StartLevel startLevel { get; set; }

    [Option("STRINGS.UI.FRONTEND.QUICKSTART.SCOPE_OPT", "STRINGS.UI.FRONTEND.QUICKSTART.SCOPE_OPT_DESC")]
    [JsonProperty]
    public Scope scope { get; set; }

    [Option("STRINGS.UI.FRONTEND.QUICKSTART.CRITTERS_OPT", "STRINGS.UI.FRONTEND.QUICKSTART.CRITTERS_DESC")]
    [JsonProperty]
    public bool includeCritters { get; set; }

    public QuickStartOptions() {
        this.startLevel = StartLevel.AdvancedEarly;
        this.scope = Scope.NewGameOnly;
        this.includeCritters = true;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Options[ ");

        foreach(PropertyInfo prop in typeof(QuickStartOptions).GetProperties()) {
            sb.Append(string.Format("{0}={1} ", prop.Name, prop.GetValue(this, null)));
        }
        sb.Append("]");

        return sb.ToString();
    }
}
