
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems;
using TrainGame.Callbacks; 

public class InventoryIndexSystemTest {
    [Fact]
    public void InventoryIndexSystem_ShouldChangeWhichInventoryIsDrawn() {
        World w = WorldFactory.Build(); 
        
        Inventory inv = new Inventory("TrainInv", 1, 1); 
        City city = new City("City", inv); 
        Train t = new Train(inv, city); 

        for (int i = 0; i < 3; i++) {
            t.AddCart(new Cart(CartType.Freight));
        }

        DrawInventoryContainerMessage<Train> dm = new DrawInventoryContainerMessage<Train>(
            t, 
            Vector2.Zero, 
            100, 
            100
        );

        InventoryContainer<Train> container = DrawInventoryContainerSystem.Draw<Train>(dm, w);

        w.SetComponent<Button>(container.PageBackwardEnt, new Button(true)); 
        w.Update(); 
        
        Assert.Contains("Freight", w.GetComponent<Inventory>(w.GetMatchingEntities([typeof(Inventory)])[0]).Id); 
        //todo: could test more but its kinda annoying since the whole container gets redrawn on each click
    }
}