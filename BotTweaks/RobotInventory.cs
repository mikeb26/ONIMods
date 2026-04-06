/*
 * Original Copyright 2026 Peter Han; see ./ResourceScreen/LICENSE from this directory
 */

using KSerialization;
using PeterHan.PLib.Detours;
using System;
using System.Collections.Generic;

using RobotInventoryPerType = System.Collections.Generic.Dictionary<Tag, PeterHan.
    RobotInventory.RobotTotals>;

namespace BotTweaks;

/// <summary>
/// Stores the inventory of each critter type. One is created per world.
/// </summary>
[SerializationConfig(MemberSerialization.OptIn)]
public sealed class RobotInventory : KMonoBehaviour, IRender200ms {
    // EX1-452242 made these fields private
    private static readonly IDetouredField<FactionAlignment, bool> FACTION_TARGETABLE =
        PDetours.DetourField<FactionAlignment, bool>("targetable");

    private static readonly IDetouredField<FactionAlignment, bool> FACTION_TARGETED =
        PDetours.DetourField<FactionAlignment, bool>("targeted");

    /// <summary>
    /// The total quantity of creatures.
    /// </summary>
    private readonly IDictionary<RobotType, RobotInventoryPerType> counts;

    /// <summary>
    /// Flags whether new critter types have been discovered.
    /// </summary>
    private bool discovered;

    /// <summary>
    /// The critter types that are currently pinned.
    /// </summary>
    [Serialize]
    private Dictionary<RobotType, HashSet<Tag>> pinned;

#pragma warning disable CS0649
#pragma warning disable IDE0044
    // This field is automatically populated by KMonoBehaviour
    [MyCmpReq]
    private WorldContainer worldContainer;
#pragma warning restore IDE0044
#pragma warning restore CS0649

    public RobotInventory() {
        this.counts = new Dictionary<RobotType, RobotInventoryPerType>(4);
        foreach (var type in Enum.GetValues(typeof(RobotType))) {
            if (type is RobotType ct) {
                this.counts.Add(ct, new RobotInventoryPerType(32));
            }
        }
    }

    /// <summary>
    /// Adds a critter in the current world to the inventory.
    /// </summary>
    /// <param name="id">The prefab ID of the creature to add.</param>
    private void AddRobot(KPrefabID id) {
        if (this.counts.TryGetValue(id.GetRobotType(), out var byType)) {
            var species = id.PrefabTag;
            bool targeted = false, targetable = false;
            // Create critter totals if not present
            if (!byType.TryGetValue(species, out var totals)) {
                byType.Add(species, totals = new RobotTotals());
                this.discovered = true;
            }
            totals.Total++;
            if (id.TryGetComponent(out FactionAlignment alignment)) {
                targeted = FACTION_TARGETED.Get(alignment);
                targetable = FACTION_TARGETABLE.Get(alignment);
            }
            // Reserve wrangled, marked for attack, and trussed/bagged creatures
            if ((id.TryGetComponent(out Capturable capturable) && capturable.
                    IsMarkedForCapture) || (targeted && targetable) || id.HasTag(
                    GameTags.Creatures.Bagged)) {
                totals.Reserved++;
            }
        }
    }

    /// <summary>
    /// Gets the total quantity of critters of a specific type.
    /// </summary>
    /// <param name="type">The critter type, wild or tame.</param>
    /// <param name="species">The critter species to examine.</param>
    /// <returns>The total quantity of critters of that type and species.</returns>
    internal RobotTotals GetBySpecies(RobotType type, Tag species) {
        if (!this.counts.TryGetValue(type, out var byType)) {
            throw new ArgumentOutOfRangeException(nameof(type));
        }
        if (!byType.TryGetValue(species, out var totals)) {
            totals = new RobotTotals();
        }
        return totals;
    }

    /// <summary>
    /// Gets the species that are pinned for a given critter type.
    /// </summary>
    /// <param name="type">The critter type to look up.</param>
    /// <returns>The pinned species, or null if pins are not yet initialized.</returns>
    public ISet<Tag> GetPinnedSpecies(RobotType type) {
        if (this.pinned == null || !this.pinned.TryGetValue(type, out var result)) {
            result = null;
        }
        return result;
    }

    /// <summary>
    /// Gets the world ID of this inventory. It is parented to the same component as
    /// WorldInventory, so the same method is used.
    /// </summary>
    /// <returns>The world ID to use for inventory.</returns>
    private int GetWorldID() {
        return (this.worldContainer != null) ? this.worldContainer.id : -1;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        // Initialize, if not deserialized from the file
        if (this.pinned == null) {
            this.pinned = new Dictionary<RobotType, HashSet<Tag>>(4);
            foreach (var pair in this.counts) {
                this.pinned.Add(pair.Key, new HashSet<Tag>());
            }
        }
    }

    /// <summary>
    /// Gets the total quantity of each critter of a specific type.
    /// </summary>
    /// <param name="type">The critter type, wild or tame.</param>
    /// <param name="results">The location to populate the results per species.</param>
    /// <returns>The total quantity of critters of that type.</returns>
    internal RobotTotals PopulateTotals(RobotType type,
            IDictionary<Tag, RobotTotals> results) {
        if (!this.counts.TryGetValue(type, out var byType)) {
            throw new ArgumentOutOfRangeException(nameof(type));
        }
        var all = new RobotTotals();
        foreach (var pair in byType) {
            var totals = pair.Value;
            var species = pair.Key;
            if (results != null && !results.ContainsKey(species)) {
                results.Add(species, totals);
            }
            all.Total += totals.Total;
            all.Reserved += totals.Reserved;
        }
        return all;
    }

    /// <summary>
    /// Updates the contents of the critter inventory.
    /// </summary>
    public void Render200ms(float _) {
        int id = this.GetWorldID();
        var clusterManager = ClusterManager.Instance;
        if (clusterManager != null && clusterManager.activeWorldId == id) {
            // Reset existing count to zero
            foreach (var typePair in this.counts) {
                foreach (var speciesPair in typePair.Value) {
                    var species = speciesPair.Value;
                    species.Reserved = 0;
                    species.Total = 0;
                }
            }
            this.discovered = false;
            RobotInventoryUtils.GetRobots(id, this.AddRobot);
            var inst = AllResourcesScreen.Instance;
            if (this.discovered && inst != null) {
                inst.Populate();
            }
        }
    }
}
