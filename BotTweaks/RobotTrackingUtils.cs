// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

namespace BotTweaks;

public static class RobotTrackingUtils {
    private static bool PassesSearchFilter(string title, string search) {
        if (string.IsNullOrEmpty(search)) {
            return true;
        }
        if (title == null) {
            return false;
        }
        // Avoid allocations and locale-sensitive casing.
        return title.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    internal static bool PassesSearchFilterForRow(string title, string search) => PassesSearchFilter(title, search);

    internal static Tag GetRobotModelTag(RobotType type) {
        return type switch {
            // Use prefab tags for UI sprites; some robots (e.g. Remote Worker) do not have a model tag.
            RobotType.Flydo => Flydo.PrefabTag,
            RobotType.Sweepy => Sweepy.PrefabTag,
            RobotType.Biobot => Biobot.PrefabTag,
            RobotType.Rover => Rover.PrefabTag,
            RobotType.RemoteWorker => RemoteWorker.PrefabTag,
            _ => Tag.Invalid,
        };
    }

    internal static string GetRobotDisplayName(RobotType type) {
        // Prefer the tag's localized proper name when available. This keeps naming
        // consistent with other resource list rows.
        var tag = GetRobotModelTag(type);
        if (tag.IsValid) {
            return tag.ProperNameStripLink();
        }
        return type.ToString();
    }
}
