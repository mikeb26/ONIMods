// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace IdleNotificationTweaks;

[JsonObject(MemberSerialization.OptIn)]
[ModInfo("https://github.com/mikeb26/ONIMods/IdleNotificationTweaks")]
public sealed class IdleOptions {
    [Option("Pause on Idle", "Pause the game when a duplicant becomes idle.")]
    [JsonProperty]
    public bool PauseOnIdle { get; set; }

    public IdleOptions() {
        PauseOnIdle = false; // default
    }

    public override string ToString() {
        return string.Format("IdleOptions[PauseOnIdle={0}]", PauseOnIdle);
    }
}
