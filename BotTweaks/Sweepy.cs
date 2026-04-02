// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

namespace BotTweaks;

internal sealed class Sweepy : TrackedRobot {
    internal static readonly Tag PrefabTag = new Tag("SweepBot");

    internal override RobotType RobotType => RobotType.Sweepy;
}
