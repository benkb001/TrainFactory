
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 

public class HeldItemTest {
    [Fact]
    public void HeldItem_ShouldRespectConstructors() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 3); 
        int invEntity = EntityFactory.Add(w); 
        w.SetComponent<Inventory>(invEntity, inv); 

        HeldItem held = new HeldItem(inv, invEntity); 
        Assert.Equal(inv, held.GetInventory());
        Assert.Equal(invEntity, held.InventoryEntity); 
        Assert.Equal(0, held.InvIndex);
        Assert.Equal(3, held.InvSize); 
        Assert.Equal("", held.ItemId);
        Assert.Equal(0, held.ItemCount); 
        Assert.False(w.EntityExists(held.LabelEntity)); 
    }
    
    [Fact]
    public void HeldItem_ShouldUpdateSetItemIfInBounds() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 3); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2), 0, 1);
        int invEntity = EntityFactory.Add(w); 
        w.SetComponent<Inventory>(invEntity, inv); 

        HeldItem held = new HeldItem(inv, invEntity); 

        held.SetItem(1); 
        Assert.Equal(1, held.InvIndex); 
        Assert.Equal("Apple", held.ItemId); 
        Assert.Equal(2, held.ItemCount); 

        
    }

    [Fact]
    public void HeldItem_ShouldNotChangeSetItemIfOutOfBounds() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 3); 
        int invEntity = EntityFactory.Add(w); 
        w.SetComponent<Inventory>(invEntity, inv); 

        HeldItem held = new HeldItem(inv, invEntity); 
        Assert.Equal(0, held.InvIndex); 
        held.SetItem(-1);
        Assert.Equal(0, held.InvIndex); 
        held.SetItem(3); 
        Assert.Equal(0, held.InvIndex); 
    }
}