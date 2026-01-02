namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Utils;
using TrainGame.Constants;

public enum CraftState {
    Idle, 
    Crafting, 
    Delivering
}

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
    private int productDelivered; 
    private int numCrafting; 
    private CraftState state = CraftState.Idle; 
    private string upgradeItemID; 
    private bool allowManual; 

    public float Completion => (float)(((float)curCraftTicks) / craftTicks);
    public int CraftTicks => craftTicks; 
    public string Id => id; 
    public readonly Inventory Inv;
    public readonly Inventory PlayerInv; 
    public int Level => level;
    public bool ProduceInfinite => produceInfinite; 
    public int ProductCount => productCount; 
    public string ProductItemId => productItemId; 
    public int RequestedAmount => requestedAmount; 
    public bool CraftComplete => curCraftTicks >= craftTicks; 
    public CraftState State => state; 
    public Dictionary<string, int> Recipe => recipe; 
    public string UpgradeItemID => upgradeItemID; 
    public bool AllowManual => allowManual; 

    public Machine(Inventory Inv, Dictionary<string, int> recipe, string productItemId, int productCount, int minTicks, 
        string id = "", bool produceInfinite = false, float slowFactor = 0f, float startFactor = 1f, Inventory PlayerInv = null, 
        string upgradeItemID = ItemID.MachineUpgrade, bool allowManual = false) {
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
        this.allowManual = allowManual;

        this.requestedAmount = 0; 
        this.curCraftTicks = 0; 
        this.upgradeItemID = upgradeItemID; 

        SetCraftTicks(); 
    }

    public void Request(int amount) {
        requestedAmount += amount; 
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
        return $"{productCount * (1 + level)} {productItemId}/{seconds.ToString("F2")} seconds\n";
    }

    public string GetRecipeFormatted() {
        string r = "";  
        foreach (KeyValuePair<string, int> kvp in recipe) {
            r = $"{r}{kvp.Key}: {kvp.Value}\n";
        }
        return r; 
    }

    public int GetNumCraftable() {
        int max = requestedAmount; 

        if (produceInfinite) {
            max = Int32.MaxValue; 
        }

        max = Math.Min(max, level + 1); 

        foreach (KeyValuePair<string, int> kvp in recipe) {
            string itemID = kvp.Key; 
            int baseCost = kvp.Value; 
            int materialCount = Inv.ItemCount(itemID); 
            int numCraftable = materialCount / baseCost; 
            max = Math.Min(max, numCraftable); 
            if (max == 0) {
                return 0; 
            }
        }

        return max; 
    }

    public void StartRecipe(int numToCraft = 1) {
        foreach (KeyValuePair<string, int> kvp in recipe) {
            Inv.Take(kvp.Key, kvp.Value * numToCraft); 
        }
        state = CraftState.Crafting; 
        this.numCrafting = numToCraft; 
    }

    public void FinishRecipe() {
        productDelivered = 0; 
        curCraftTicks = 0; 
        state = CraftState.Delivering; 
    }

    public void DeliverRecipe() {
        int productToDeliver = productCount * numCrafting; 
        int productLeft = productToDeliver - productDelivered; 
        int curDelivered = Inv.Add(new Inventory.Item(ItemId: productItemId, Count: productLeft));
        requestedAmount -= curDelivered; 
        productDelivered += curDelivered; 
        if (productDelivered >= productToDeliver) {
            state = CraftState.Idle; 
            numCrafting = 0; 
        }
    }

    public void UpdateCrafting() {
        curCraftTicks++; 
    }

    public bool InvHasRequiredItems() {
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