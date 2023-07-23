// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

namespace BeyondExtraHard;

public static class Strings {
    public static class UI {
        public static class FRONTEND {
            public static class BEH {
                public static LocString POWER = "Power";
                public static LocString POWER_DESC = "Adjusts the difficulty level of power generation.";

                public static LocString POWER_NORMAL = "Normal";
                public static LocString POWER_NORMAL_DESC = "Power generators produce their normal amount of power.";

                public static LocString POWER_BROWNOUT = "Brownout";
                public static LocString POWER_BROWNOUT_DESC = "Power generators produce 25% less power vs. Normal.";

                public static LocString POWER_BLACKOUT = "Blackout";
                public static LocString POWER_BLACKOUT_DESC = "Power generators produce 50% less power vs. Normal.";

                public static LocString OXYGEN = "Oxygen";
                public static LocString OXYGEN_DESC = "Adjusts the amount of oxygen duplicants consume.";

                public static LocString OXYGEN_NORMAL = "Normal";
                public static LocString OXYGEN_NORMAL_DESC = "Duplicants consume the normal amount of oxygen (100 g/s).";

                public static LocString OXYGEN_GASPING = "Gasping";
                public static LocString OXYGEN_GASPING_DESC = "Duplicants consume 50 g/s more oxygen vs. Normal.";

                public static LocString OXYGEN_HYPERVENTILATING = "Hyperventilating";
                public static LocString OXYGEN_HYPERVENTILATING_DESC = "Duplicants consume 100 g/s more oxygen vs. Normal.";

                public static LocString HEALTH = "Health";
                public static LocString HEALTH_DESC = "Adjusts the maximum amount of health duplicants can have.";

                public static LocString HEALTH_NORMAL = "Normal";
                public static LocString HEALTH_NORMAL_DESC = "Duplicants start with the normal amount of maximum health (100 hit points).";

                public static LocString HEALTH_DELICATE = "Delicate";
                public static LocString HEALTH_DELICATE_DESC = "Duplicants start with 25% less maximum health vs. Normal (75 hit points).";

                public static LocString HEALTH_FRAGILE = "Fragile";
                public static LocString HEALTH_FRAGILE_DESC = "Duplicants start with half of the maximum health vs. Normal (50 hit points).";

                public static LocString TECH = "Technology";
                public static LocString TECH_DESC = "Adjusts which buildings are available to be researched & built.";

                public static LocString TECH_NORMAL = "Normal";
                public static LocString TECH_NORMAL_DESC = "All buildings are available.";

                public static LocString TECH_NOSPOM = "SPOM Implosion";
                public static LocString TECH_NOSPOM_DESC = "Some powerful and easy to setup buildings are removed. These are:\n\t<smallcaps>Electrolyzer:</smallcaps> Electrolyzer based SPOMs are so cheap and easy to setup that they have become an ubiquitous inclusion in almost every base. There are many alternative, if less convenient, pathways to sustainable oxygen.\n\n\t<smallcaps>Solar Panel:</smallcaps> Spamming solar panels in space at the cost of a few kg of plastic is so cheap and easy that it usually renders all other sources of power essentially redundant.";

                public static LocString TECH_MANUALSIEVE = "Sieve it Yourself";
                public static LocString TECH_MANUALSIEVE_DESC = "In addition to the \"SPOM Implosion\" removals, the following buildings are removed:\n\n\t<smallcaps>Water Sieve and Desalinator:</smallcaps> Players can instead produce clean water by boiling polluted water, salt water, or brine in a steam room. Care must be taken in the early game to ensure that enough water is available to research steel. It may be a good idea to defer upgrading that Latrine to a Washroom.";

                public static LocString TECH_NOTURBINE = "Heat != Power";
                public static LocString TECH_NOTURBINE_DESC = "In addition to the \"Sieve it Yourself\" removals, the following building is removed:\n\n\t<smallcaps>Steam Turbine:</smallcaps> This is a game defining limitation and will push even the most advanced and experienced ONI players to their limit. All of the usual design patterns for heat deletion are completely upended.";
            }
        }
    }
}
