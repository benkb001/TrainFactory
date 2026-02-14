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

    //TODO: drop chances should also shift with floor? 
    private static List<(string, int)> drops = new() {
        (ItemID.Plasma, 40),
        (ItemID.Credit, 30), 
        (ItemID.Iron, 15), 
        (ItemID.Glass, 5),
        (ItemID.Wood, 5),
        (ItemID.Assembler, 3), 
        (ItemID.Motherboard, 1),
        (ItemID.TimeCrystal, 1)
    };

    private static int maxDrop => drops.Aggregate(0, (acc, cur) => acc + cur.Item2);

    private static Dictionary<string, Func<int, int>> dropCounts = new() {
        [ItemID.Plasma] = (f) => f, 
        [ItemID.Credit] = (f) => Util.Pow(f * 2, 1.1), 
        [ItemID.Iron] = (f) => 10 + f, 
        [ItemID.Glass] = (f) => f, 
        [ItemID.Wood] = (f) => f, 
        [ItemID.Assembler] = (f) => 1 + (f / 5), 
        [ItemID.Motherboard] = (f) => 1, 
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

    public static Loot GetRandom(int floor, Inventory destination, World w) {
        int rand = w.NextInt(maxDrop + 1); 
        int sum = 0; 

        foreach ((string itemID, int chance) in drops) {
            sum += chance; 
            if (sum >= rand) {
                return new Loot(itemID, dropCounts[itemID](floor), destination);
            }
        }

        throw new InvalidOperationException($"Loot randomization error, maxDrop: {maxDrop}, rand: {rand}");
    }
}