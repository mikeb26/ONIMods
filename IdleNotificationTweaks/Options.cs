// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace IdleNotificationTweaks;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/IdleNotificationTweaks")]
public sealed class IdleOptions {
    [Option("Suppress Idle in Rockets", "Suppress Idle notifications from duplicants inside of moving or mining rockets.")]
    [JsonProperty]
    public bool SuppressIdleInRockets { get; set; }

    [Option("Pause on Idle", "Pause the game when a duplicant becomes idle (and was not suppressed).")]
    [JsonProperty]
    public bool PauseOnIdle { get; set; }

    public IdleOptions() {
        // defaults
        SuppressIdleInRockets = true;
        PauseOnIdle = false;
    }

    public override string ToString() {
        return string.Format("IdleOptions[SuppressIdleInRockets={0} PauseOnIdle={1}]",
                             SuppressIdleInRockets, PauseOnIdle);
    }
}
