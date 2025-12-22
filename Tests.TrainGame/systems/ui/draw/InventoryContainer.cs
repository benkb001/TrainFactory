
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 

public class DrawInventoryContainerSystemTest {
    [Fact]
    public void DrawInventoryContaierSystem_ShouldDrawAnInventoryContainer() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("TrainInv", 1, 1); 
        City city = new City("City", inv); 
        Train t = new Train(inv, city); 


        InventoryContainer<Train> container = new InventoryContainer<Train>(t); 
        int cEntity = EntityFactory.Add(w); 
        
        DrawInventoryContainerMessage<Train> dm = new DrawInventoryContainerMessage<Train>(
            t, Vector2.Zero, 100, 100
        ); 

        int dmEnt = EntityFactory.Add(w); 
        w.SetComponent<DrawInventoryContainerMessage<Train>>(dmEnt, dm); 
        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(InventoryContainer<Train>)]));
    }
}