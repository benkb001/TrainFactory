
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils;

public class MachineUpdateSystemTest {

    private (World, Inventory, Machine, int) init() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 20)); 
        //should only craft 5 max because we have 5 bananas and it costs 1 banana
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 5)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 1); 
        int requestNum = 20; 
        m.Request(requestNum); 
        
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Machine>(e, m); 

        return (w, inv, m, requestNum); 
    }

    private void update(Machine m) {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Machine>(e, m); 
        w.Update(); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldLetMachinesCraft() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0);
        m.Request(1); 
        
        int mEntity = EntityFactory.Add(w, setData: true);
        w.SetComponent<Machine>(mEntity, m); 
        w.Update(); 

        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotCraftIfNoItemsAreRequested() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        update(m); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(1, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotCraftIfNotEnoughTicksHavePassed() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 2); 
        update(m); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(1, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotCraftIfInsufficientItemsArePresent() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 0)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        update(m); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_CraftingShouldConsumeItemsInRecipe() {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0); 
        m.Request(1); 
        update(m); 
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_CraftingShouldOnlyProduceAsManyAsRecipeAllows() {
        (World w, Inventory inv, Machine m, int requestNum) = init(); 

        for (int i = 0; i < requestNum; i++) {
            w.Update(); 
        }

        Assert.Equal(5, inv.ItemCount("Smoothie")); 
        Assert.Equal(10, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotStartCraftingIfStillDelivering() {
        //inv is 2x2, machine is 2 apple, 1 banana => 1 smoothie in 1 tick, inv has 20 apple 5 banana
        (World w, Inventory inv, Machine m, int requestNum) = init(); 

        inv.Add(new Inventory.Item(ItemId: "Test1", Count: 10)); 
        inv.Add(new Inventory.Item(ItemId: "Test2", Count: 10)); 

        w.Update(); 

        Assert.Equal(18, inv.ItemCount("Apple")); 
        Assert.Equal(4, inv.ItemCount("Banana")); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 

        w.Update(); 


        Assert.Equal(18, inv.ItemCount("Apple")); 
        Assert.Equal(4, inv.ItemCount("Banana")); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 

        inv.Take(1, 1); 

        w.Update(); 

        Assert.Equal(18, inv.ItemCount("Apple")); 
        Assert.Equal(4, inv.ItemCount("Banana")); 
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
    }
}