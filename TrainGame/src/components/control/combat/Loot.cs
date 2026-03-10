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

public class Loot {
    private string itemID; 
    private int count; 
    private Inventory destination; 
    public int Count => count;

    //TODO: drop chances should also shift with floor? 
    private static List<(string, int)> drops1 = new() {
        (ItemID.Plasma, 39),
        (ItemID.Credit, 69), 
        (ItemID.TimeCrystal, 1)
    };

    private static List<(string, int)> drops2 = new() {
        (ItemID.Carbon, 28),
        (ItemID.Credit, 70),
        (ItemID.TimeCrystal, 2)
    };

    private static List<(string, int)> drops3 = new() {
        (ItemID.Adamantite, 17), 
        (ItemID.Credit, 80),
        (ItemID.TimeCrystal, 3)
    };

    private static List<(string, int)> getDrops(int floor) {
        if (floor < 20) {
            return drops1;
        } else if (floor < 40) {
            return drops2;
        } else {
            return drops3; 
        }
    }

    private static int maxDrop(List<(string, int)> drops) {
        return drops.Aggregate(0, (acc, cur) => acc + cur.Item2);
    }

    private static Dictionary<string, Func<int, int>> dropCounts = new() {
        [ItemID.Plasma] = (f) => f + (int)(f * 3 * Util.NextDoublePositive()), 
        [ItemID.Credit] = (f) => f + (int)((double)(f * f) * Util.NextDoublePositive() * Util.NextDoublePositive()), 
        [ItemID.Carbon] = (f) => f + (int)(f * 2 * Util.NextDoublePositive()),
        [ItemID.Adamantite] = (f) => f + (int)(f * Util.NextDoublePositive()),
        [ItemID.TimeCrystal] = (f) => 10
    };

    public string GetItemID() => itemID; 

    public int Transfer() {
        return destination.Add(itemID, count); 
    }

    public Loot(string itemID, int count, Inventory destination) {
        this.itemID = itemID; 
        this.count = count; 
        this.destination = destination; 
    }

    public static Loot GetRandom(int floor, Inventory destination, World w, int difficulty = 1) {
        List<(string, int)> drops = getDrops(floor);

        int max = maxDrop(drops); 
        int rand = w.NextInt(max); 
        int sum = 0; 

        foreach ((string itemID, int chance) in drops) {
            sum += chance; 
            if (sum > rand) {
                int count = dropCounts[itemID](floor);
                return new Loot(itemID, count * difficulty, destination);
            }
        }

        throw new InvalidOperationException($"Loot randomization error, maxDrop: {maxDrop}, rand: {rand}");
    }
}

public class LootWrap {
    public static Inventory GetDestination(World w) {
        return CityWrap.GetCityWithPlayer(w).Inv;
    }
}