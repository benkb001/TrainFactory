namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Machine {
    public readonly Inventory Inv;
    private Dictionary<string, int> recipe;
    private int craftTicks;
    private int curCraftTicks; 
    private string productItemId;
    private int productCount; 
    private int requestedAmount;
    
    public Machine(Inventory Inv, Dictionary<string, int> recipe, string productItemId, int productCount, int craftTicks) {
        this.Inv = Inv;
        this.recipe = recipe;
        this.craftTicks = craftTicks; 
        this.productItemId = productItemId;
        this.productCount = productCount; 
        this.requestedAmount = 0; 
        this.curCraftTicks = 0; 
    }

    public void Request(int amount) {
        requestedAmount += amount; 
    }

    public void Update() {
        if (curCraftTicks >= craftTicks) {
            if ( requestedAmount >= productCount && invHasRequiredItems()) {
                foreach (KeyValuePair<string, int> kvp in recipe) {
                    Inv.Take(kvp.Key, kvp.Value); 
                }
                Inv.Add(new Inventory.Item(ItemId: productItemId, Count: productCount));
                requestedAmount -= productCount; 
                curCraftTicks = 0; 
            }
        } else {
            curCraftTicks++; 
        }
    }

    private bool invHasRequiredItems() {
        foreach (KeyValuePair<string, int> kvp in recipe) {
            string itemId = kvp.Key; 
            int cost = kvp.Value;

            int numItems = Inv.ItemCount(itemId);
            if (numItems < cost) {
                return false; 
            }
        }
        return true; 
    }
}