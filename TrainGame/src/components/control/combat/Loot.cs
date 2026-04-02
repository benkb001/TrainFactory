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

public class MaxAmmo {}

public class Distribution<T, U> {
    private int chanceTotal; 
    private Dictionary<T, int> chances; 
    private Dictionary<T, U> events; 

    public Distribution(Dictionary<T, int> chances, Dictionary<T, U> events) {
        if (chances.Count != events.Count) {
            throw new InvalidOperationException($"A  distribution must have the same number of chance and event");
        }

        foreach (T key in chances.Keys) {
            if (!events.ContainsKey(key)) {
                throw new InvalidOperationException("A distribution must have all keys match in chances and events");
            }
        }

        this.chances = chances;
        this.events = events;
        this.chanceTotal = chances.Aggregate(0, (acc, cur) => acc + cur.Value);
    }

    public (T, U) GetRandom() {
        int r = Util.NextInt(chanceTotal); 
        int total = 0; 

        foreach (KeyValuePair<T, int> chance in chances) {
            T key = chance.Key;
            int c = chance.Value; 

            total += c; 

            
            if (total > r) {
                U eve = events[key]; 
                return (key, eve); 
            }
        }

        throw new InvalidOperationException("Error in LootDistribution.GetRandom");
    }
}

public class LootDistribution {
    private Distribution<string, int> dist; 

    public LootDistribution(Dictionary<string, int> dropChances, Dictionary<string, int> dropCounts) {
        this.dist = new Distribution<string, int>(dropChances, dropCounts); 
    }

    public (string, int) GetRandom() {
        return dist.GetRandom(); 
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