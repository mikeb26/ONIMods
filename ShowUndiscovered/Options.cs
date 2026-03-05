// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.Text;
using System.Reflection;
using System;

namespace ShowUndiscovered;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/ShowUndiscovered")]
public sealed class Options /* : IOptions */ {
    [Option("STRINGS.UI.FRONTEND.SU.LOG_DISC", "STRINGS.UI.FRONTEND.SU.LOG_DISC_DESC")]
    [JsonProperty]
    public bool logDiscovery { get; set; }

    public Options() {
        // defaults
	this.logDiscovery = false;
    }

    // @todo add option to store currently non-storable items
    //    e.g. microchip, micronutrient fertilizer
    //    as "Industrial Product"
    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Options[ ");

        foreach(PropertyInfo prop in typeof(Options).GetProperties()) {
            sb.Append(string.Format("{0}={1} ", prop.Name, prop.GetValue(this, null)));
        }
        sb.Append("]");

        return sb.ToString();
    }
}
