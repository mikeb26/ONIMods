// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BotTweaks;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/BotTweaks")]
public sealed class Options : IOptions {
    [Option("STRINGS.UI.FRONTEND.BOTTWEAKS.ROVER_EXPIRE_OPT", "STRINGS.UI.FRONTEND.BOTTWEAKS.ROVER_EXPIRE_DESC")]
    [JsonProperty]
    public ExpiredDisposition roverExpireDisposition { get; set; }

    [Option("STRINGS.UI.FRONTEND.BOTTWEAKS.BIOBOT_EXPIRE_OPT", "STRINGS.UI.FRONTEND.BOTTWEAKS.BIOBOT_EXPIRE_DESC")]
    [JsonProperty]
    public ExpiredDisposition biobotExpireDisposition { get; set; }

    public Options() {
        // defaults (match stock game behavior)
        this.roverExpireDisposition = ExpiredDisposition.DoNothing;
        this.biobotExpireDisposition = ExpiredDisposition.MarkForDeconstruct;
    }

    public IEnumerable<IOptionsEntry> CreateOptions() {
        // Use PLib's attribute-driven options.
        return null;
    }

    public void OnOptionsChanged() {
        if (Mod.Instance?.gameState != null) {
            Mod.Instance.gameState.opts = this;
        }
        // If the player changed expiration behavior, re-apply to already-dead robots
        // (important for existing saves that already have expired rovers/biobots).
        ExpiredRobotBehavior.ApplyToExistingDeadRobots();
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
