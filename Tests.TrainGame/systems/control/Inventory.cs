using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils;

public class InventoryControlSystemTest {

    [Fact]
    public void InventoryControlSystem_ShouldUpdateCellsCorrespondingItems() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = w.AddEntity(); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryWidth = 100f; 
        float inventoryPadding = 5f; 

        int msg = w.AddEntity(); 
        w.SetComponent<DrawInventoryMessage>(msg, new DrawInventoryMessage(
            inventoryWidth, 
            200f, 
            Vector2.Zero, 
            inv,     
            inventoryEntity, 
            inventoryPadding
        )); 
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
        
        int msg = w.AddEntity(); 
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

        Assert.Equal("Banana", inv1.Get(1, 1).ItemId); 
        Assert.Equal("Apple", inv2.Get(0, 1).ItemId);
    }

    [Fact]
    public void InventoryControlSystem_ShouldSetDraggableSnapPositionFromMessage() {
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
        
        int msg = w.AddEntity(); 
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

        Assert.Equal(targetVector, d.SnapPosition); 
    }

    [Fact]
    public void InventoryControlSystem_ShouldRemoveOrganizeMessageEntities() {
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
        
        int msg = w.AddEntity(); 
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

        Assert.False(w.EntityExists(msg)); 
    }
}