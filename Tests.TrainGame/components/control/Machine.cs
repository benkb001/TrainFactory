using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class MachineTest {
    [Fact]
    public void Machine_ShouldNotCraftIfNoItemsAreRequested() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        m.Update();
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(1, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void Machine_ShouldNotCraftIfNotEnoughTicksHavePassed() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 2); 
        m.Update();
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(1, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void Machine_ShouldNotCraftIfInsufficientItemsArePresent() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 0)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        m.Update();
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void Machine_CraftingShouldConsumeItemsInRecipe() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        m.Request(1); 
        m.Update();
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void Machine_CraftingShouldOnlyProduceAsManyAsRecipeAllows() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 20)); 
        //should only craft 5 max because we have 5 bananas and it costs 1 banana
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 5)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        int requestNum = 20; 
        m.Request(requestNum); 
        for (int i = 0; i < requestNum; i++) {
            m.Update(); 
        }

        Assert.Equal(5, inv.ItemCount("Smoothie")); 
        Assert.Equal(10, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void Machine_CraftTimeFunctionShouldCorrectlyDecreaseWithLevel() {
        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "Smoothie", 1, minTicks: 10, slowFactor: 100, startFactor: 1); 
        Assert.Equal(110, m.CraftTicks);
        m.Upgrade(9);
        Assert.Equal(20, m.CraftTicks);  
    }
}