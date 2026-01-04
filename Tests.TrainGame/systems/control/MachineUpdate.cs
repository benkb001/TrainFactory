
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

    private (World, Inventory, Machine, int) init(int numApple, int numBanana) {
        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: numApple)); 
        //should only craft 5 max because we have 5 bananas and it costs 1 banana
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: numBanana)); 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", 
            productCount: 1, minTicks: 1, level: 0, numRecipeToStore: 1); 
        
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Machine>(e, m); 

        return (w, inv, m, 5); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldLetMachinesCraft() {
        (World w, Inventory inv, Machine m, int _) = init(2, 1); 
        w.Update(); 

        Assert.Equal(1, inv.ItemCount("Smoothie")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotCraftIfNotEnoughTicksHavePassed() {
        (World w, Inventory inv, Machine m, int _) = init(2, 1);
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotCraftIfInsufficientItemsArePresent() {
        (World w, Inventory inv, Machine m, int _) = init(2, 0); 
        w.Update(); 
        Assert.Equal(0, inv.ItemCount("Smoothie")); 
    }

    [Fact]
    public void MachineUpdateSystem_CraftingShouldConsumeItemsInRecipe() {
        (World w, Inventory inv, Machine m, int _) = init(2, 1); 
        w.Update();
        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }

    [Fact]
    public void MachineUpdateSystem_CraftingShouldOnlyProduceAsManyAsRecipeAllows() {
        (World w, Inventory inv, Machine m, int numCraftable) = init(20, 5); 

        for (int i = 0; i < numCraftable + 1; i++) {
            w.Update(); 
        }

        Assert.Equal(5, inv.ItemCount("Smoothie")); 
    }

    [Fact]
    public void MachineUpdateSystem_ShouldNotStartCraftingIfStillDelivering() {
        //inv is 2x2, machine is 2 apple, 1 banana => 1 smoothie in 1 tick, inv has 20 apple 5 banana
        (World w, Inventory inv, Machine m, int requestNum) = init(20, 5); 

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