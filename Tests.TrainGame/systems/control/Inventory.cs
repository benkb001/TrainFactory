using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils;
using TrainGame.Callbacks; 
using TrainGame.Constants; 

public class InventoryControlSystemTest {

    private (World, Inventory, Inventory, Vector2, int, Draggable) init() {
        World w = new World(); 
        RegisterComponents.All(w);
        RegisterSystems.All(w); 

        Inventory inv1 = new Inventory("Test1", 2, 2); 
        Inventory inv2 = new Inventory("Test2", 2, 2); 

        Inventory.Item apple = new Inventory.Item(ItemId: "Apple", Count: 2); 
        Inventory.Item banana = new Inventory.Item(ItemId: "Banana", Count: 3); 

        Draggable d = new Draggable();
        Vector2 targetVector = new Vector2(10, 20); 

        inv1.Add(apple, 1, 1); 
        inv2.Add(banana, 0, 1); 

        Assert.Equal("Apple", inv1.Get(1, 1).ItemId);
        Assert.Equal("Banana", inv2.Get(0, 1).ItemId); 
        
        int msg = EntityFactory.Add(w); 
        InventoryOrganizeMessage organize = new InventoryOrganizeMessage(
            TargetRow: 0, 
            TargetColumn: 1, 
            CurRow: 1, 
            CurColumn: 1, 
            TargetItem: banana, 
            CurItem: apple, 
            CurDraggable: d, 
            TargetVector: targetVector
        ); 

        w.SetComponent<InventoryOrganizeMessage>(msg, organize); 
        w.Update(); 
        return (w, inv1, inv2, targetVector, msg, d); 
    }

    [Fact]
    public void InventoryControlSystem_ShouldUpdateCellsCorrespondingItems() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryWidth = 100f; 
        float inventoryPadding = 5f; 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, inventoryWidth, 
            Height: 200f, Entity: inventoryEntity, Padding: inventoryPadding);

        w.Update(); 

        Inventory.Item apple = new Inventory.Item(ItemId: "Apple", Count: 10); 
        inv.Add(apple, 3, 4); 

        w.Update(); 
        
        int rowEntity = w.GetComponent<LinearLayout>(inventoryEntity).GetChildren()[3]; 
        int cellEntity = w.GetComponent<LinearLayout>(rowEntity).GetChildren()[4]; 

        Assert.Equal("Apple", w.GetComponent<Inventory.Item>(cellEntity).ItemId); 
    }

    [Fact]
    public void InventoryControlSystem_ShouldSwapSpecifiedItemsInOrganizeMessage() {
        (World w, Inventory inv1, Inventory inv2, Vector2 targetVector, int msg, Draggable d) = init(); 

        Assert.Equal("Banana", inv1.Get(1, 1).ItemId); 
        Assert.Equal("Apple", inv2.Get(0, 1).ItemId);
    }

    [Fact]
    public void InventoryControlSystem_ShouldSetDraggableSnapPositionFromMessage() {
        (World w, Inventory inv1, Inventory inv2, Vector2 targetVector, int msg, Draggable d) = init(); 

        Assert.Equal(targetVector, d.SnapPosition); 
    }

    [Fact]
    public void InventoryControlSystem_ShouldRemoveOrganizeMessageEntities() {
        (World w, Inventory inv1, Inventory inv2, Vector2 targetVector, int msg, Draggable d) = init(); 

        Assert.False(w.EntityExists(msg)); 
    }

    [Fact]
    public void InventoryControlSystem_ShouldReturnExcessItemsToOriginalInventories() {
        World w = WorldFactory.Build(); 

        Inventory inv1 = new Inventory("Test1", 1, 2); 
        Inventory inv2 = new Inventory("Test2", 1, 2); 

        Inventory.Item woodLarge = new Inventory.Item(ItemId: ItemID.Wood, Count: Constants.ItemStackSize(ItemID.Wood) - 1); 
        Inventory.Item woodSmall = new Inventory.Item(ItemId: ItemID.Wood, Count: 2); 
        inv1.Add(woodLarge, 0, 0);
        inv2.Add(woodSmall, 0, 0);
        Draggable d = new Draggable();

        InventoryOrganizeMessage organize = new InventoryOrganizeMessage(
            TargetRow: 0, 
            TargetColumn: 0, 
            CurRow: 0, 
            CurColumn: 0, 
            TargetItem: woodLarge, 
            CurItem: woodSmall, 
            CurDraggable: d, 
            TargetVector: Vector2.Zero
        ); 

        int msg = EntityFactory.Add(w); 
        w.SetComponent<InventoryOrganizeMessage>(msg, organize); 
        w.Update(); 

        Assert.Equal(Constants.ItemStackSize(ItemID.Wood), inv1.ItemCount(ItemID.Wood));
        Assert.Equal(1, inv2.ItemCount(ItemID.Wood)); 

        InventoryOrganizeMessage organize2 = new InventoryOrganizeMessage(
            TargetRow: 0, 
            TargetColumn: 0, 
            CurRow: 0, 
            CurColumn: 0, 
            TargetItem: woodSmall, 
            CurItem: woodLarge, 
            CurDraggable: d, 
            TargetVector: Vector2.Zero
        ); 

        int msg2 = EntityFactory.Add(w); 
        w.SetComponent<InventoryOrganizeMessage>(msg2, organize); 
        w.Update(); 

        //now inv1 should have small and inv2 should have many 
        Assert.Equal(Constants.ItemStackSize(ItemID.Wood), inv2.ItemCount(ItemID.Wood));
        Assert.Equal(1, inv1.ItemCount(ItemID.Wood)); 
    }
}