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
using TrainGame.ECS;
using TrainGame.Systems;

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
        World w = WorldFactory.Build();
        w.AddComponentType<InvSource>();
        w.AddComponentType<InventoryContainer<InvSource>>(); 
        w.AddComponentType<InventoryIndexer<InvSource>>(); 
        List<Inventory> invs = new(); 

        invs.Add(new Inventory("Test", 1, 1)); 
        InvSource src = new InvSource(invs); 
        DrawInventoryContainerMessage<InvSource> dm = new DrawInventoryContainerMessage<InvSource>(
            src, 
            Vector2.Zero, 
            100,
            100
        );

        InventoryContainer<InvSource> container = DrawInventoryContainerSystem.Draw<InvSource>(
            dm, w
        );

        Assert.Equal(invs, container.GetInventories()); 
    }
}