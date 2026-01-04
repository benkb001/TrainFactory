using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class MachineTest {

    private (Inventory, Machine) init() {
        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "Smoothie", 1, 
            minTicks: 10, slowFactor: 100, startFactor: 1, level: 0, numRecipeToStore: 1); 
        return (inv, m); 
    }

    [Fact]
    public void Machine_CraftTimeFunctionShouldCorrectlyDecreaseWithLevel() {
        (Inventory inv, Machine m) = init(); 
        Assert.Equal(110, m.CraftTicks);
        m.Upgrade(9);
        Assert.Equal(20, m.CraftTicks);  
    }

    [Fact]
    public void Machine_StartRecipeShouldSetStateToCrafting() {
        (Inventory inv, Machine m) = init(); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 2)); 

        Assert.Equal(1, inv.ItemCount("Apple")); 
        Assert.Equal(2, inv.ItemCount("Banana")); 

        Assert.Equal(CraftState.Idle, m.State);
        
        m.StartRecipe(); 
        Assert.Equal(CraftState.Crafting, m.State); 
    }

    [Fact]
    public void Machine_FinishRecipeShouldSetStateToDelivering() {
        (Inventory inv, Machine m) = init(); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 2)); 

        m.StartRecipe(); 
        m.FinishRecipe(); 
        Assert.Equal(CraftState.Delivering, m.State); 
    }

    [Fact]
    public void Machine_DeliverRecipeShouldSetStateToIdleOnceMaxProductIsDelivered() {
        (Inventory inv, Machine m) = init(); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 2)); 

        m.StartRecipe(); 
        m.FinishRecipe(); 
        m.DeliverRecipe(); 
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(CraftState.Idle, m.State); 
    }

    [Fact]
    public void Machine_ShouldDeliverItemsOnceTheInventoryHasSpace() {
        (Inventory inv, Machine m) = init(); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 2)); 

        m.StartRecipe(); 

        inv.Add(new Inventory.Item(ItemId: "Test1", Count: 1));
        inv.Add(new Inventory.Item(ItemId: "Test2", Count: 1));
        m.FinishRecipe(); 
        m.DeliverRecipe(); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        inv.Take(0, 0); 
        m.DeliverRecipe(); 
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
    }
}