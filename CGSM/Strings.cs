// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

namespace CGSM;

public static class Strings {
    public static class UI {
        public static class FRONTEND {
            public static class CGSM {
                public static LocString STARMAP_OPT = "Starmap radius";
                public static LocString STARMAP_OPT_DESC = "Number of hexes from the center to an outermost hex on the starmap";

                public static LocString START_OPT = "Start Planetoid";
                public static LocString START_OPT_DESC = "Select your starting planetoid";

                public static LocString WARP_OPT = "Warp Planetoid";
                public static LocString WARP_OPT_DESC = "Select your teleport planetoid";

                public static LocString ADDITIONAL_PLANET_CAT = "Additional Planetoids";
                public static LocString SPACE_POIS_CAT = "Space POIs";

                public static LocString NUM_ARTIFACT_OPT = "Number of Artifact Only POIs";
                public static LocString NUM_ARTIFACT_OPT_DESC = "(Russell's Teapot, Gravitas Space Station, etc.)";
            }
        }
    }

    public static class CLUSTER_NAMES {
        public static class CGSM_CLUSTER {
            public static LocString NAME = "Cluster Generation Settings Manager";
            public static LocString DESCRIPTION = "CGSM generated cluster. To change planetoids please go back to the Mods menu and click CGSM's Options button.";
        }
    }

    public static class WORLDS {
        public static class CGSM {
            public static LocString MARSHYMOONLET_NAME = "Marshy Asteroid(cgsm)";
            public static LocString MARSHYMOONLET_DESC = "A small location with an abundance of marshland.\n\n<smallcaps>While Marshy Asteroids offer an abundance of organic resources like Slime Mold and Algae, their air quality poses a significant disease risk for Duplicants.</smallcaps>. (customized by CGSM)";

            public static LocString TUNDRAMOONLET_NAME = "Tundra Asteroid(cgsm)";
            public static LocString TUNDRAMOONLET_DESC = "A small frozen planetoid with sub-zero temperatures.\n\n<smallcaps>Duplicants that travel to Tundra Asteroids will need to wear protective gear due to its harsh environment.</smallcaps>. (customized by CGSM)";
        }
    }
}
