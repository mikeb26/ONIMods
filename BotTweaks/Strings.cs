// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

namespace BotTweaks;

public static class Strings {
    public static class UI {
        public static class FRONTEND {
            public static class BOTTWEAKS {
                public static LocString ROVER_EXPIRE_OPT = "Rover Expire Behavior";
                public static LocString ROVER_EXPIRE_OPT_DESC = "Control the behavior of an expired Rover.";

                public static LocString BIOBOT_EXPIRE_OPT = "Biobot Expire Behavior";
                public static LocString BIOBOT_EXPIRE_OPT_DESC = "Control the behavior of an expired Biobot.";
                                                                                                              public static LocString EXPIRE_NOTHING = "Do Nothing";
                public static LocString EXPIRE_NOTHING_DESC = "The robot is left alone after it expires.";

                                                                                                              public static LocString EXPIRE_DECONS = "Deconstruct";
                public static LocString EXPIRE_DECONS_DESC = "The robot is marked for deconstruction after it expires.";

                public static LocString EXPIRE_ENABLE_POWERBANKS = "Enable Power Banks";
                public static LocString EXPIRE_ENABLE_POWERBANKS_DESC = "When the robot's internal battery expires, allow it to use Power Banks (like a Flydo).";
            }
        }
    }
}
