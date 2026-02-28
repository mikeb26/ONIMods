// Copyright © 2023,2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Elements {
    public Elements() {
    }

    // Don't discover special "non-material" or unused elements.
    private static readonly HashSet<SimHashes> IgnoredElements = new HashSet<SimHashes>() {
        SimHashes.Vacuum,
        SimHashes.Void,
        SimHashes.Bitumen,
        SimHashes.Propane,
        SimHashes.LiquidPropane,
        SimHashes.SolidPropane,
        SimHashes.Unobtanium,
        SimHashes.COMPOSITION,
    };

    // Only allow known resource screen categories; unknown/empty categories fall back to Other.
    private static readonly HashSet<Tag> AllowedCategoryTags = new HashSet<Tag>() {
        GameTags.Agriculture,
        GameTags.Breathable,
        GameTags.BuildableProcessed,
        GameTags.BuildableRaw,
        GameTags.ConsumableOre,
        GameTags.Farmable,
        GameTags.Filter,
        GameTags.Liquid,
        GameTags.Liquifiable,
        GameTags.ManufacturedMaterial,
        GameTags.Metal,
        GameTags.Organics,
        GameTags.Other,
        GameTags.RareMaterials,
        GameTags.RefinedMetal,
        GameTags.Solid,
        GameTags.Sublimating,
        GameTags.Unbreathable,
    };

    // Avoid log spam: only log category fallbacks once per element per session.
    private static readonly HashSet<SimHashes> LoggedCategoryFallback = new HashSet<SimHashes>();

    private static Tag GetCategoryTag(Element element) {
        // Use materialCategory directly; it's a Tag in current ONI builds.
        Tag categoryTag = element.materialCategory;

        if (categoryTag == Tag.Invalid) {
            if (LoggedCategoryFallback.Add(element.id)) {
                Util.Log("Elements: {0} has empty/invalid materialCategory; using category '{1}'", element.tag, GameTags.Other);
            }
            return GameTags.Other;
        }

        if (!AllowedCategoryTags.Contains(categoryTag)) {
            if (LoggedCategoryFallback.Add(element.id)) {
                Util.Log("Elements: {0} has materialCategory '{1}' not in AllowedCategoryTags; using category '{2}'",
                    element.tag, categoryTag, GameTags.Other);
            }
            return GameTags.Other;
        }

        return categoryTag;
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        // Iterate the live element table so DLCs (e.g. Bionic: GearOil/Gunk) and game updates
        // automatically work without needing to keep a hardcoded list up to date.
        foreach (var element in ElementLoader.elements) {
            if (element == null) {
                continue;
            }

            if (IgnoredElements.Contains(element.id)) {
                continue;
            }

            var elementTag = element.tag;

            var categoryTag = GetCategoryTag(element);
            tags.Add(elementTag);
            DiscoveredResources.Instance.Discover(elementTag, categoryTag);
        }

        return tags;
    }
}
