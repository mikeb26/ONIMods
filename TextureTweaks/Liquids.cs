// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace TextureTweaks;

public static class Liquids {
    private static readonly FieldInfo SubstanceTextureField = AccessTools.Field(typeof(Substance), "texture");

    public static void ApplyTextureOverrides(List<Element> elements) {
        if (SubstanceTextureField == null) {
            Util.Log("Unable to apply liquid texture overrides: Substance.texture field was not found");
            return;
        }

        Options opts = Mod.Instance?.gameState?.opts ?? new Options();
        int changed = 0;

        foreach(Element element in elements) {
            if (element == null || element.substance == null || !element.IsLiquid) {
                continue;
            }

            LiquidTextureChoice choice = LiquidTextureChoice.Default;
            if (element.id == SimHashes.Magma) {
                choice = opts.MagmaTexture;
            } else if (element.IsMoltenMetal) {
                choice = opts.MoltenMetalTexture;
            }

            if (choice == LiquidTextureChoice.Default) {
                continue;
            }

            Substance.SubstanceTexture texture = ToSubstanceTexture(choice);
            SubstanceTextureField.SetValue(element.substance, texture);
            changed++;
            Util.LogDbg("Set {0} liquid texture to {1}", element.id, texture);
        }

        if (changed > 0) {
            Util.Log("Applied {0} liquid texture override(s): Magma={1}, MoltenMetal={2}",
                     changed, opts.MagmaTexture, opts.MoltenMetalTexture);
        }
    }

    private static Substance.SubstanceTexture ToSubstanceTexture(LiquidTextureChoice choice) {
        if (choice == LiquidTextureChoice.Default) {
            return Substance.SubstanceTexture.None;
        }

        return (Substance.SubstanceTexture)System.Enum.Parse(typeof(Substance.SubstanceTexture), choice.ToString());
    }
}
