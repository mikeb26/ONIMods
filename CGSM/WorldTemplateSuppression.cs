// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;

namespace CGSM;

internal static class WorldTemplateSuppression
{
    // Worldgen robustness: suppress specific guaranteed POI templates that are
    // known to occasionally be unplaceable on some seeds.
    //
    // Example: NiobiumMoonlet sometimes fails with:
    //   "Guaranteed placement failure ... Could not place ... poi_mining_room"
    //
    // Rather than hard-failing worldgen, we selectively remove that template
    // from the rule's candidate list at spawn time.

    private static readonly Dictionary<string, HashSet<string>> SuppressGuaranteedTemplatesByWorld =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "expansion1::worlds/NiobiumMoonlet",
                new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "expansion1::poi/genericGravitas/poi_mining_room",
                    "poi/genericGravitas/poi_mining_room",
                }
            }
        };

    private static readonly HashSet<string> LoggedSuppressedTemplates =
        new(StringComparer.OrdinalIgnoreCase);

    internal static void MaybeSuppressGuaranteedTemplates(WorldGenSettings settings, ProcGen.World.TemplateSpawnRules rule)
    {
        try
        {
            if (settings?.world == null || rule?.names == null || rule.names.Count == 0)
                return;

            // Only adjust rules on specific worlds.
            var worldPath = settings.world.filePath;
            if (string.IsNullOrWhiteSpace(worldPath))
                return;

            if (!SuppressGuaranteedTemplatesByWorld.TryGetValue(worldPath, out var suppressList)
                || suppressList == null
                || suppressList.Count == 0)
                return;

            // Only suppress for guarantee rules (these are the ones that hard-fail worldgen).
            if (!rule.IsGuaranteeRule())
                return;

            for (int i = rule.names.Count - 1; i >= 0; i--)
            {
                var name = rule.names[i];
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                if (!suppressList.Contains(name))
                    continue;

                rule.names.RemoveAt(i);

                var logKey = $"{worldPath}|{name}";
                if (LoggedSuppressedTemplates.Add(logKey))
                    Util.Log("TemplateSpawning: suppressing guaranteed template '{0}' for world '{1}'", name, worldPath);
            }

            // No further action needed; if this was the only name in a GuaranteeAll rule,
            // the rule becomes a no-op and worldgen will proceed.
        }
        catch (Exception e)
        {
            Util.Log("TemplateSpawning: suppression hook failed; continuing with stock behavior: {0}", e);
        }
    }
}
