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

    public string ItemID => itemID; 

    public int Transfer() {
        return destination.Add(itemID, count); 
    }

    public Loot(string itemID, int count, Inventory destination) {
        this.itemID = itemID; 
        this.count = count; 
        this.destination = destination; 
    }
}