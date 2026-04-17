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