using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Callbacks; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InventoryFastTransferSystemTest {
    [Fact]
    public void InventoryFastTransferSystem_ShouldTransferAsManyItemsAsPossibleToOtherInventory() {
        World w = WorldFactory.Build(); 
        Inventory inv1 = new Inventory("T1", 1, 2); 
        Inventory inv2 = new Inventory("T2", 1, 1); 

        InventoryView invView1 = DrawInventoryCallback.Draw(w, inv1, Vector2.Zero, 100, 100); 
        InventoryView invView2 = DrawInventoryCallback.Draw(w, inv2, Vector2.Zero, 100, 100); 

        inv1.Add("StackSize1", 2); 
        w.SetComponent<Button>(invView1.GetCellEntity(0, 0, w), new Button(Click.Shift)); 
        w.Update(); 
        Assert.Equal(1, inv1.ItemCount("StackSize1")); 
        Assert.Equal(1, inv2.ItemCount("StackSize1")); 
    }
}