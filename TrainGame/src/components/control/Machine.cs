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
    
    private Dictionary<string, int> recipe;
    private int craftTicks;
    private int curCraftTicks; 
    private string id; 
    private int level; 
    private int minTicks; 
    private bool produceInfinite; 
    private string productItemId;
    private int productCount; 
    private int requestedAmount;
    private float slowFactor; 
    private float startFactor; 

    public float Completion => (float)(((float)curCraftTicks) / craftTicks);
    public int CraftTicks => craftTicks; 
    public string Id => id; 
    public readonly Inventory Inv;
    public readonly Inventory PlayerInv; 
    public int Level => level;
    public bool ProduceInfinite => produceInfinite; 
    public int ProductCount => productCount; 
    public int RequestedAmount => requestedAmount; 

    public Machine(Inventory Inv, Dictionary<string, int> recipe, string productItemId, int productCount, int minTicks, 
        string id = "", bool produceInfinite = false, float slowFactor = 0f, float startFactor = 1f, Inventory PlayerInv = null) {
        this.Inv = Inv;
        this.PlayerInv = PlayerInv; 
        this.recipe = recipe;
        this.minTicks = minTicks; 
        this.slowFactor = slowFactor; 
        this.startFactor = startFactor; 
        this.produceInfinite = produceInfinite; 
        this.productItemId = productItemId;
        this.productCount = productCount; 
        this.id = id; 
        this.level = 0; 

        this.requestedAmount = 0; 
        this.curCraftTicks = 0; 

        SetCraftTicks(); 
    }

    public void Request(int amount) {
        requestedAmount += amount; 
    }

    public void Update() {
        if ((requestedAmount >= productCount || produceInfinite) && invHasRequiredItems()) {
            if (curCraftTicks >= craftTicks) {
                foreach (KeyValuePair<string, int> kvp in recipe) {
                    Inv.Take(kvp.Key, kvp.Value); 
                }
                Inv.Add(new Inventory.Item(ItemId: productItemId, Count: productCount));
                requestedAmount -= productCount; 
                curCraftTicks = 0; 
            }
            curCraftTicks++; 
        }
    }

    public string GetId() {
        return id; 
    }
    
    public void SetCraftTicks() {
        craftTicks = (int)(minTicks + (slowFactor / (level + startFactor)));
    }

    public void Upgrade(int levels = 1) {
        this.level += levels; 
        SetCraftTicks(); 
    }

    public string GetCraftSpeedFormatted() {
        float seconds = (float)(craftTicks / 60f); 
        return $"{productCount} {productItemId}/{seconds.ToString("F2")} seconds\n";
    }

    public string GetRecipeFormatted() {
        string r = "";  
        foreach (KeyValuePair<string, int> kvp in recipe) {
            r = $"{r}{kvp.Key}: {kvp.Value}\n";
        }
        return r; 
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