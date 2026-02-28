// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System;
using System.Reflection;
using Klei.CustomSettings;
using Klei.AI;

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

    public GameState() {
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

    public void logMissing(Tag tag, Tag categoryTag) {
        if (this.isStarted && !this.allTags.Contains(tag)) {
            Util.LogDbg("Missing tag: {0}, {1}", tag, categoryTag);
        }
    }
}
