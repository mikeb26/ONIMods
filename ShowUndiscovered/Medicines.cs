// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System;
using System.Collections.Generic;

namespace ShowUndiscovered;

public class Medicines {
    private class MedicineInfo {
        public string id;
        public MedicineInfo(string idIn) {
            this.id = idIn;
        }
    }
    private bool baseGameOnly;

    private List<MedicineInfo> medicines;

    public Medicines(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.medicines = new List<MedicineInfo>();

        // vitamin chews
        this.medicines.Add(new MedicineInfo(BasicBoosterConfig.ID));
        // Immuno Booster
        this.medicines.Add(new MedicineInfo(IntermediateBoosterConfig.ID));
        // curative tablet
        this.medicines.Add(new MedicineInfo(BasicCureConfig.ID));
        // rad pill
        this.medicines.Add(new MedicineInfo(BasicRadPillConfig.ID));
        // medical pack
        this.medicines.Add(new MedicineInfo(IntermediateCureConfig.ID));
        // serum vial
        this.medicines.Add(new MedicineInfo(AdvancedCureConfig.ID));
        // Allergy Medication
        this.medicines.Add(new MedicineInfo(AntihistamineConfig.ID));
    }

    public List<Tag> discoverAll() {
        List<Tag> tags = new List<Tag>();

        foreach (MedicineInfo medicineInfo in this.medicines) {
            var medicineTag = TagManager.Create(medicineInfo.id);
            tags.Add(medicineTag);
            DiscoveredResources.Instance.Discover(medicineTag, GameTags.Medicine);
        }

        return tags;
    }
}
