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
    private bool isStarted;
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
        this.isStarted = false;
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
        if (!this.isStarted) {
            Util.Log("Pre-start tag: {0}, {1}", tag, categoryTag);
        }

        if (!this.allTags.Contains(tag)) {
            Util.Log("Missing tag: {0}, {1}", tag, categoryTag);
        } else {
            // @todo check if category tag is the same as what we discovered with during init
        }
    }

    public void Discover(Tag tag, Tag catTag) {
        if (this.opts.showUndiscovered) {
            DiscoveredResources.Instance.Discover(tag, catTag);
        }
    }
}
