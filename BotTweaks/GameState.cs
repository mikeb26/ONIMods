// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using PeterHan.PLib.Options;

namespace BotTweaks;

public class GameState
{
    internal Options opts = null;

    public GameState() {
        this.opts = POptions.ReadSettings<Options>();
        if (this.opts == null) {
            this.opts = new Options();
        }
    }
}
