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
    private Dictionary<string, int> stored; 
    private bool craftComplete = false;
    private int craftTicks;
    private int curCraftTicks; 
    private string id; 
    private int level; 
    private int minTicks; 
    private string productItemId;
    private int productCount; 
    private float slowFactor; 
    private float startFactor; 
    private int productDelivered; 
    private int numCrafting; 
    private CraftState state = CraftState.Idle; 
    private string upgradeItemID; 
    private bool allowManual; 
    private int priority; 
    private int numRecipeToStore;

    public float Completion => (float)(((float)curCraftTicks) / craftTicks);
    public int CraftTicks => craftTicks; 
    public string Id => id; 
    public readonly Inventory Inv;
    public readonly Inventory PlayerInv; 
    public int Level => level;
    public int NumRecipeToStore => numRecipeToStore; 
    public int Priority => priority; 
    public int ProductCount => productCount; 
    public string ProductItemId => productItemId; 
    public bool CraftComplete => craftComplete; 
    public CraftState State => state; 
    public Dictionary<string, int> Recipe => recipe; 
    public string UpgradeItemID => upgradeItemID; 
    public bool AllowManual => allowManual; 

    public Machine(Inventory Inv, Dictionary<string, int> recipe, string productItemId, int productCount, int minTicks, 
        string id = "", float slowFactor = 0f, float startFactor = 1f, Inventory PlayerInv = null, 
        string upgradeItemID = ItemID.MachineUpgrade, bool allowManual = false) {
        this.Inv = Inv;
        this.PlayerInv = PlayerInv; 
        this.recipe = recipe;
        this.minTicks = minTicks; 
        this.numRecipeToStore = 1;
        this.slowFactor = slowFactor; 
        this.startFactor = startFactor; 
        this.priority = 0;
        this.productItemId = productItemId;
        this.productCount = productCount; 
        if (recipe != null) {
            this.stored = recipe.ToDictionary(i => i.Key, i => 0); 
        } else {
            stored = new(); 
        }
        this.id = id; 
        this.level = 0; 
        this.allowManual = allowManual;

        this.curCraftTicks = 0; 
        this.upgradeItemID = upgradeItemID; 

        SetCraftTicks(); 
    }

    public string GetId() {
        return id; 
    }
    
    public void SetCraftTicks() {
        craftTicks = (int)(minTicks + (slowFactor / (level + startFactor)));
    }

    public void SetPriority(int p) {
        this.priority = p; 
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

    public void StoreRecipe() {
        foreach (KeyValuePair<string, int> kvp in recipe) {
            string itemID = kvp.Key; 
            int cost = kvp.Value; 
            int maxToStore = cost * numRecipeToStore; 
            int numStored = stored[itemID]; 
            int taken = Inv.Take(itemID, maxToStore - numStored).Count;
            stored[itemID] += taken; 
        }
    }

    public int GetNumCraftable() {

        int max = Int32.MaxValue; 
        max = Math.Min(max, level + 1); 

        foreach (KeyValuePair<string, int> kvp in recipe) {
            string itemID = kvp.Key; 
            int baseCost = kvp.Value; 
            int storedCount = stored[itemID]; 
            int numCraftable = storedCount / baseCost; 
            max = Math.Min(max, numCraftable); 
            if (max == 0) {
                return 0; 
            }
        }

        return max; 
    }

    public void StartRecipe(int numToCraft = 1) {
        foreach (KeyValuePair<string, int> kvp in recipe) {
            string itemID = kvp.Key; 
            int baseCost = kvp.Value; 
            stored[itemID] -= baseCost * numToCraft;
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
        productDelivered += curDelivered; 
        if (productDelivered >= productToDeliver) {
            state = CraftState.Idle; 
            numCrafting = 0; 
        }
    }

    public void EndFrame() {
        craftComplete = false; 
    }

    public void UpdateCrafting() {
        curCraftTicks++; 
        craftComplete = curCraftTicks >= craftTicks; 
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

    public void SetStorageSize(int size) {
        this.numRecipeToStore = size; 
    }
}