namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class LootDistribution {
    private int dropChanceTotal; 
    private Dictionary<string, int> dropChances; 
    private Dictionary<string, int> dropCounts; 

    public LootDistribution(Dictionary<string, int> dropChances, Dictionary<string, int> dropCounts) {
        if (dropChances.Count != dropCounts.Count) {
            throw new InvalidOperationException($"A loot distribution must have the same number of dropChance and dropCount");
        }

        foreach (string itemID in dropChances.Keys) {
            if (!dropCounts.ContainsKey(itemID)) {
                throw new InvalidOperationException("A loot distribution must have all itemIDs match in drop chance and drop count");
            }
        }

        this.dropChances = dropChances;
        this.dropCounts = dropCounts;
        this.dropChanceTotal = dropChances.Aggregate(0, (acc, cur) => acc + cur.Value);
    }

    public (string, int) GetRandom() {
        int max = dropChanceTotal;
        int rand = Util.NextInt(max); 
        int sum = 0; 

        foreach (KeyValuePair<string, int> dropChance in dropChances) {
            string itemID = dropChance.Key; 
            int chance = dropChance.Value; 

            sum += chance; 

            if (sum > rand) {
                int count = dropCounts[itemID];
                return (itemID, count); 
            }
        }

        throw new InvalidOperationException("Error in LootDistribution.GetRandom");
    }
}

public class Loot {
    private string itemID; 
    private int count; 
    private Inventory destination; 
    public int Count => count;

    public string GetItemID() => itemID; 

    public int Transfer() {
        return destination.Add(itemID, count); 
    }

    public Loot(string itemID, int count, Inventory destination) {
        this.itemID = itemID; 
        this.count = count; 
        this.destination = destination; 
    }
}

public class LootWrap {
    public static Inventory GetDestination(World w) {
        return CityWrap.GetCityWithPlayer(w).Inv;
    }
}