// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System;
using Klei.CustomSettings;
using Klei.AI;
using PeterHan.PLib.Options;

namespace ShowUndiscovered;

public class GameState
{
    private Critters critters;
    private Seeds seeds;
    private Foods foods;
    private Medicines medicines;
    private Clothes clothes;
    private Elements elements;
    private Extras extras;
    private List<Tag> allTags;
    private Dictionary<Tag, Tag> allTagCategories;
    private bool isStarted;
    private bool isOurDiscover;
    internal Options opts = null;

    public GameState() {
        this.opts = POptions.ReadSettings<Options>();
        if (opts == null) {
            this.opts = new Options();
        }
        this.critters = new Critters();
        this.seeds = new Seeds();
        this.medicines = new Medicines();
        this.clothes = new Clothes();
        this.elements = new Elements();
        this.extras = new Extras();
        this.foods = new Foods();
        this.allTags = new List<Tag>();
        this.allTagCategories = new Dictionary<Tag, Tag>();
        this.isStarted = false;
        this.isOurDiscover = false;
    }

    public void discoverAll() {
        // discoverBuildings();
        this.allTags.AddRange(this.foods.discoverAll());
        this.allTags.AddRange(this.critters.discoverAll());
        this.allTags.AddRange(this.seeds.discoverAll());
        this.allTags.AddRange(this.medicines.discoverAll());
        this.allTags.AddRange(this.clothes.discoverAll());
        this.allTags.AddRange(this.elements.discoverAll());
        this.allTags.AddRange(this.extras.discoverAll());
        this.isStarted = true;
    }

    // @todo
    //   Notes: BuildingMenuBuildingScreen.RefreshToggle():Overlay_NeedTech
    //          PlanBuildinghToggle.RefreshFG():Overlay_NeedTech
    // private void discoverBuildings() {
    // }

    public void logDiscover(Tag tag, Tag categoryTag) {
        if (!this.opts.logDiscovery) {
            return;
        }
        if (!this.isOurDiscover) {
            return;
        }
        if (!this.isStarted) {
            Util.Log("Pre-start tag: {0}, {1}", tag, categoryTag);
            return;
        }

        if (!this.allTags.Contains(tag)) {
            Util.Log("Missing tag: {0}, {1}", tag, categoryTag);
        } else {
            // If the game discovers a resource under a different category than the one we
            // pre-discovered during init, log it (helps catch DLC/API changes).
            if (this.isStarted && this.allTagCategories.TryGetValue(tag, out Tag originalCategoryTag)) {
                if (originalCategoryTag != categoryTag) {
                    Util.Log("Category mismatch tag={0}: original={1}, now={2}", tag, originalCategoryTag, categoryTag);
                }
            }
        }
    }

    public void Discover(Tag tag, Tag catTag) {
        // Track the category we originally used when pre-discovering this tag during init.
        // (Don't overwrite later; we want the initial mapping for mismatch detection.)
        if (!this.isStarted && !this.allTagCategories.ContainsKey(tag)) {
            this.allTagCategories.Add(tag, catTag);
        }

        if (this.opts.showUndiscovered) {
            this.isOurDiscover = true;
            DiscoveredResources.Instance.Discover(tag, catTag);
            this.isOurDiscover = false;
        }
    }
}
