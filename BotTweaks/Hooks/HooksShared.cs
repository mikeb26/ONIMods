// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;

namespace BotTweaks.Hooks;

internal static class HooksShared {
    internal static bool SafePrefix(Func<bool> body, bool fallback = true) {
        try {
            return body();
        } catch {
            return fallback;
        }
    }

    /// <summary>
    /// Helper for Harmony Prefixes that need to optionally override a bool return value.
    /// Return null from <paramref name="body"/> to proceed to the original method.
    /// Return true/false to set __result and suppress the original.
    /// </summary>
    internal static bool SafePrefixOverrideBool(Func<bool?> body, ref bool __result, bool fallback = true) {
        try {
            var overrideResult = body();
            if (overrideResult.HasValue) {
                __result = overrideResult.Value;
                return false;
            }
            return true;
        } catch {
            return fallback;
        }
    }
}
