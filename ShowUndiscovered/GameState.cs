// Copyright © 2023 Mike Brown; see LICENSE at the root of this package

using System.Collections.Generic;
using System;
using System.Reflection;
using Klei.CustomSettings;
using Klei.AI;

namespace ShowUndiscovered;

public class GameState
{
    private SUOptions opts;
    private bool baseGameOnly;
    private Critters critters;
    private Seeds seeds;
    private Medicines medicines;
    private Clothes clothes;
    private Elements elements;
    private Extras extras;
    private List<Tag> allTags;
    private bool isStarted;

    public GameState() {
        this.opts = null;
        if (DlcManager.IsPureVanilla() || !DlcManager.IsExpansion1Active()) {
            this.baseGameOnly = true;
        } else {
            this.baseGameOnly = false;
        }
        this.critters = new Critters(this.baseGameOnly);
        this.seeds = new Seeds(this.baseGameOnly);
        this.medicines = new Medicines(this.baseGameOnly);
        this.clothes = new Clothes(this.baseGameOnly);
        this.elements = new Elements(this.baseGameOnly);
        this.extras = new Extras(this.baseGameOnly);
        this.allTags = new List<Tag>();
        this.isStarted = false;
    }

    public void discoverAll() {
        // discoverBuildings();
        discoverAllFoods();
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

    private void discoverAllFoods() {
        foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes()) {
            if (this.baseGameOnly && foodInfo.DlcId != "") {
                continue;
            }
            // this food was never released
            if (foodInfo.Id == GammaMushConfig.ID) {
                continue;
            }

            var tag = TagManager.Create(foodInfo.Id);
            this.allTags.Add(tag);
            var catTag = GameTags.Edible;
            if (foodInfo.CaloriesPerUnit <= 0.0) {
                catTag = GameTags.CookingIngredient;
            }
            DiscoveredResources.Instance.Discover(tag, catTag);
        }
    }

    public void logMissing(Tag tag, Tag categoryTag) {
        if (this.isStarted && !this.allTags.Contains(tag)) {
            Util.LogDbg("Missing tag: {0}, {1}", tag, categoryTag);
        }
    }
}
