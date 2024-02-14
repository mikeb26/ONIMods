// Copyright © 2024 Mike Brown; see LICENSE at the root of this package

using Klei.AI;
using System.Collections.Generic;

namespace QuickStart;

public class DupeBufs {
    private bool baseGameOnly;
    private const int AttrBufIncr = 5;
    private const int SkillBufIncr = 5;
    private List<string> generalistAttrs;

    public DupeBufs(bool baseGameOnlyIn) {
        this.baseGameOnly = baseGameOnlyIn;
        this.generalistAttrs = new List<string>();

        // we only upgrade the general attributes & skill groups for all dupes,
        // not specialist skills (e.g. decor, ranching, etc)
        this.generalistAttrs.Add(Db.Get().Attributes.Athletics.Id);
        this.generalistAttrs.Add(Db.Get().Attributes.Construction.Id);
        this.generalistAttrs.Add(Db.Get().Attributes.Digging.Id);
        this.generalistAttrs.Add(Db.Get().Attributes.Machinery.Id);
        this.generalistAttrs.Add(Db.Get().Attributes.Strength.Id);
        this.generalistAttrs.Add(Db.Get().Attributes.Learning.Id);
    }

    public int UpgradeAll(QuickStartOptions opts) {
        int count = 0;

        foreach (MinionIdentity minion in Components.LiveMinionIdentities.Items) {
            this.UpgradeOne(opts, minion);
            count++;
        }

        return count;
    }

    public void UpgradeOne(QuickStartOptions opts, MinionIdentity minion) {
        Util.Log("Upgrading dupe(id:{0}) for a {1} start.", minion.GetInstanceID(),
                 opts.startLevel);

        this.upgradeAttributeLevels(opts, minion);
        this.upgradeSkillpoints(opts, minion);
    }

    private void upgradeAttributeLevels(QuickStartOptions opts, MinionIdentity minion) {
        if (minion.gameObject.TryGetComponent(out AttributeLevels attrLvls) == false) {
            Util.Log("BUG: could not find attr levels for dupe(id:{0})",
                     minion.GetInstanceID());
            return;
        }

        var targetLvl = (int)opts.startLevel * AttrBufIncr;

        foreach(var attrId in this.generalistAttrs) {
            var attrLvl = attrLvls.GetAttributeLevel(attrId);
            if (attrLvl == null) {
                Util.Log("Could not find attribute {0} for dupe(id:{1})", attrId,
                         minion.GetInstanceID());
                continue;
            }

            while (attrLvl.GetLevel() < targetLvl) {
                // need multiple calls here because AttributeLevel.LevelUp() resets experience
                // each time
                attrLvl.AddExperience(attrLvls, attrLvl.GetExperienceForNextLevel());
            }
        }
    }

    private void upgradeSkillpoints(QuickStartOptions opts, MinionIdentity minion) {
        if (minion.gameObject.TryGetComponent(out MinionResume minionResume) == false) {
            Util.Log("BUG: could not find resume for dupe");
            return;
        }

        var targetLvl = (int)opts.startLevel * SkillBufIncr;

        while (minionResume.TotalSkillPointsGained < targetLvl) {
            // need multiple calls here because ForceSetSkillPoints() doesn't invoke
            // triggers
            minionResume.ForceAddSkillPoint();
        }
    }
}
