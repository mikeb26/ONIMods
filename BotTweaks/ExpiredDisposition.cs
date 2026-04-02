// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;

namespace BotTweaks;

// Stock game behavior:
//   Sweepy/Remote Worker: doesnt expire; has a dedicated charging dock
//   Flydo: doesnt expire; uses power banks from jump
//   Rover: expires after 10 cycles; not automatically marked for deconstruct
//   Biobot: expires after 10 cycles; automatically marked for deconstruct

public enum ExpiredDisposition {
    [Option("STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_NOTHING", "STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_NOTHING_DESC")]
    DoNothing,
    [Option("STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_DECONS", "STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_DECONS_DESC")]
    MarkForDeconstruct,
    [Option("STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_ENABLE_POWERBANKS", "STRINGS.UI.FRONTEND.BOTTWEAKS.EXPIRE_ENABLE_POWERBANKS_DESC")]
    EnablePowerBanks,
}
