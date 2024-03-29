// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.IO;
using System.Text;

namespace CGSM;

public class ClusterYamlEmitter {
    private string outputFile;
    private Cluster cluster;
    private int category;

    // @todo does c# have the equivalent of go:embed ?
    private static string ClusterYamlHeaderTextFmt = @"name: STRINGS.CLUSTER_NAMES.CGSM_CLUSTER.NAME
description: STRINGS.CLUSTER_NAMES.CGSM_CLUSTER.DESCRIPTION
requiredDlcId: EXPANSION1_ID
coordinatePrefix: CGSM-C
menuOrder: {4}
clusterCategory: {0}
{1}difficulty: {2}

startWorldIndex: {3}
";

    public ClusterYamlEmitter(Cluster clusterIn, int categoryIn, string outputFileIn) {
        this.cluster = clusterIn;
        this.outputFile = outputFileIn;
        this.category = categoryIn;
    }

    /* returns whether or not the generated yaml has changed */
    public bool emit() {
        StringBuilder yamlContent = new StringBuilder();

        emitHeaders(ref yamlContent);
        if (this.cluster.placePriority == Cluster.PlacePriority.Start) {
            emitStartPlanet(ref yamlContent);
            emitWarpPlanet(ref yamlContent);
            emitOtherPlanets(ref yamlContent, false);
        } else if (this.cluster.placePriority == Cluster.PlacePriority.Warp) {
            emitWarpPlanet(ref yamlContent);
            emitStartPlanet(ref yamlContent);
            emitOtherPlanets(ref yamlContent, false);
        } else {
            // Cluster.PlacePriority.FirstOther
            emitFirstOtherPlanet(ref yamlContent);
            emitStartPlanet(ref yamlContent);
            emitWarpPlanet(ref yamlContent);
            emitOtherPlanets(ref yamlContent, true);
        }
        emitSpacePOIs(ref yamlContent);

        if (File.Exists(this.outputFile)) {
            var existingyamlContent = File.ReadAllText(this.outputFile);
            if (existingyamlContent == yamlContent.ToString()) {
                // Util.LogDbg("Skipping write of {0}", this.outputFile);
                return false;
            }
        }

        // Util.LogDbg("Generating {0} from {1}", this.outputFile, this.cluster);
        // Util.LogDbg("Generated yaml file content is: {0}", yamlContent.ToString());

        File.WriteAllText(this.outputFile, yamlContent.ToString());

        return true;
    }

    private void emitHeaders(ref StringBuilder yamlContent) {
        var storyTraitsStr = "disableStoryTraits: true\n";
        if (this.cluster.storyTraits) {
            storyTraitsStr = "";
        }
        int startWorldIndex = 0;
        if (this.cluster.placePriority != Cluster.PlacePriority.Start) {
            startWorldIndex = 1;
        }
        yamlContent.Append(string.Format("# AUTOGENERATED by CGSM v{0}\n", Util.Version()));
        yamlContent.Append(string.Format(ClusterYamlHeaderTextFmt, this.category, storyTraitsStr,
                                             this.cluster.difficulty, startWorldIndex, -16));
        yamlContent.Append(string.Format("numRings: {0}\nworldPlacements:\n",
                                         this.cluster.radius));
    }

    private void emitStartPlanet(ref StringBuilder yamlContent) {
        yamlContent.Append(this.cluster.start.ToYamlString());
    }

    private void emitWarpPlanet(ref StringBuilder yamlContent) {
        yamlContent.Append(this.cluster.warp.ToYamlString());
    }

    private void emitOtherPlanets(ref StringBuilder yamlContent, bool skipFirst) {
        bool didSkip = false;
        foreach (var placement in this.cluster.others) {
            if (skipFirst && !didSkip) {
                didSkip = true;
                continue;
            }
            yamlContent.Append(placement.ToYamlString());
        }
    }

    private void emitFirstOtherPlanet(ref StringBuilder yamlContent) {
        foreach (var placement in this.cluster.others) {
            yamlContent.Append(placement.ToYamlString());
            break;
        }
    }

    private void emitSpacePOIs(ref StringBuilder yamlContent) {
        if (this.cluster.poiGroups.Count == 0) {
            return;
        }

        yamlContent.Append("\npoiPlacements:\n");

        foreach (var poiGroup in this.cluster.poiGroups) {
            yamlContent.Append(poiGroup.ToYamlString());
        }
    }
}
