using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 

public class InvSource : IInventorySource {
    public List<Inventory> ls; 

    public InvSource(List<Inventory> ls) {
        this.ls = ls; 
    }

    public List<Inventory> GetInventories() {
        return ls; 
    }
}

public class InventoryContainerTest {
    [Fact]
    public void InventoryContainer_GetInventoriesShouldReturnInventoriesOfAssociatedSource() {
        List<Inventory> invs = new(); 

        invs.Add(new Inventory("Test", 1, 1)); 
        InvSource src = new InvSource(invs); 
        InventoryContainer<InvSource> container = new InventoryContainer<InvSource>(src); 
        Assert.Equal(invs, container.GetInventories()); 
    }
}