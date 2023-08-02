// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

namespace BeyondExtraHard;

public enum BEHSetting {
    Power, 
    Oxygen,
    Health,
    Tech,
    Mass
};

public enum PowerSetting {
    Normal,
    Brownout,
    Blackout
};

public enum GeneratorType {
    Manual,
    Coal,
    NatGas,
    Hydrogen,
    Petrol,
    Wood,
    Solar,
    Turbine,
    PlugSlug,
}
 
public enum OxygenSetting {
    Normal,
    Gasping,
    Hyperventilating
};

public enum HealthSetting {
    Normal,
    Delicate,
    Fragile
};

public enum TechSetting {
    Normal,
    NoSpom,
    ManualSieve,
    NoTurbine
};

public enum MassSetting {
    Normal,
    LightTax,
    HeavyTax,
};
