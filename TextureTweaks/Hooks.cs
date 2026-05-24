// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System.Collections.Generic;

namespace TextureTweaks;

public static class Hooks
{
    [HarmonyPatch(typeof(SimMessages), nameof(SimMessages.CreateSimElementsTable))]
    public static class SimMessages_CreateSimElementsTable_Patch {
        public static void Prefix(List<Element> elements) {
            Liquids.ApplyTextureOverrides(elements);
        }
    }
}
