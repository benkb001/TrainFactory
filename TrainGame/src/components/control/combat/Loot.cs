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
    public int Count => count;
    public string ItemID => itemID;

    public Loot(string itemID, int count) {
        this.itemID = itemID; 
        this.count = count; 
    }
}

public class LootWrap {
    public static Inventory GetDestination(World w) {
        return CityWrap.GetCityWithPlayer(w).Inv;
    }
}