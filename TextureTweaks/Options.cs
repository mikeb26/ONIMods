// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.Text;
using System.Reflection;
using System;

namespace TextureTweaks;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/TextureTweaks")]
public sealed class Options /* : IOptions */ {
    [Option("STRINGS.UI.FRONTEND.TEXTURETWEAKS.MAGMA_TEXTURE", "STRINGS.UI.FRONTEND.TEXTURETWEAKS.MAGMA_TEXTURE_DESC")]
    [RestartRequired]
    [JsonProperty]
    public LiquidTexture MagmaTexture { get; set; }

    [Option("STRINGS.UI.FRONTEND.TEXTURETWEAKS.MOLTEN_METAL_TEXTURE", "STRINGS.UI.FRONTEND.TEXTURETWEAKS.MOLTEN_METAL_TEXTURE_DESC")]
    [RestartRequired]
    [JsonProperty]
    public LiquidTexture MoltenMetalTexture { get; set; }

    public Options() {
        // defaults
        MagmaTexture = LiquidTexture.Polluted;
        MoltenMetalTexture = LiquidTexture.Polluted;
    }

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
