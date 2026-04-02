// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;

namespace BotTweaks;

internal sealed class RemoteWorker : TrackedRobot {
    internal static readonly Tag PrefabTag = new Tag("RemoteWorker");

    internal override RobotType RobotType => RobotType.RemoteWorker;
}
