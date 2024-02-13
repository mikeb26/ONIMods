// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;

namespace QuickStart;

public enum Scope {
    [Option("STRINGS.UI.FRONTEND.QUICKSTART.NEWGAMEONLY", "STRINGS.UI.FRONTEND.QUICKSTART.NEWGAMEONLY_DESC")]
    NewGameOnly = 0,
    [Option("STRINGS.UI.FRONTEND.QUICKSTART.EXISTING", "STRINGS.UI.FRONTEND.QUICKSTART.EXISTING_DESC")]
    NewAndExistingGames,
};
