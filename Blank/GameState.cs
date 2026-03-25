// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System;
using Klei.CustomSettings;
using Klei.AI;
using PeterHan.PLib.Options;

namespace Blank;

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
