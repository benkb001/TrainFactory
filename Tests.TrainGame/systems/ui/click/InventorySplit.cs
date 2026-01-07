using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class InventorySplitSystemTest {
    private (InventoryView, int, World) init() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 1, 3); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2), 0, 0);
        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 100);
        int cellEnt = invView.GetCellEntity(0, 0, w); 
        w.SetComponent<Button>(cellEnt, new Button(Click.Right));
        return (invView, cellEnt, w); 
    }

    [Fact]
    public void InventorySplitSystem_ShouldPutHalfOfClickedCellsItemsInNextAvailableCell() {
        (InventoryView invView, int cellEnt, World w) = init(); 
        
        w.Update(); 
        Assert.Equal(1, w.GetComponent<Inventory.Item>(cellEnt).Count); 
        Assert.Equal(2, invView.GetInventory().ItemCount("Apple")); 
        Assert.Equal(1, w.GetComponent<Inventory.Item>(invView.GetCellEntity(0, 1, w)).Count);
    }

    [Fact]
    public void InventorySplitSystem_ShouldPutItemsBackIfFull() {
        (InventoryView invView, int cellEnt, World w) = init(); 
        Inventory inv = invView.GetInventory(); 
        inv.Add("Filler1", 1); 
        inv.Add("Filler2", 1); 
        w.Update(); 
        Assert.Equal(2, w.GetComponent<Inventory.Item>(cellEnt).Count); 
    }
}