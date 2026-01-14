namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class PurchaseButton {
    private string itemID; 
    private int itemCount; 
    private Dictionary<string, int> cost; 
    private Inventory destination; 

    public string ItemID => itemID; 
    public int ItemCount => itemCount; 
    public Dictionary<string, int> Cost => cost; 
    public Inventory Dest => destination; 

    public PurchaseButton(string itemID, Dictionary<string, int> cost, Inventory dest, int itemCount = 1) {
        this.itemID = itemID; 
        this.cost = cost; 
        this.destination = dest; 
        this.itemCount = itemCount; 
    }
}
