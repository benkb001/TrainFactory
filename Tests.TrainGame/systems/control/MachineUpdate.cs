
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

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, craftTicks: 0);
        m.Request(1); 
        
        int mEntity = EntityFactory.Add(w, setData: true);
        w.SetComponent<Machine>(mEntity, m); 
        w.Update(); 

        Assert.Equal(1, inv.ItemCount("Smoothie")); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        Assert.Equal(0, inv.ItemCount("Banana")); 
    }
}