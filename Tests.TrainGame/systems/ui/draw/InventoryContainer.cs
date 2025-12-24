
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

public class DrawInventoryContainerSystemTest {

    private (World, Inventory, City, Train, InventoryContainer<Train>, DrawInventoryContainerMessage<Train>) init() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("TrainInv", 1, 1); 
        City city = new City("City", inv); 
        Train t = new Train(inv, city); 

        InventoryContainer<Train> container = new InventoryContainer<Train>(t); 

        DrawInventoryContainerMessage<Train> dm = new DrawInventoryContainerMessage<Train>(
            new InventoryContainer<Train>(t), Vector2.Zero, 100, 100
        ); 

        return (w, inv, city, t, container, dm);
    }

    [Fact]
    public void DrawInventoryContaierSystem_ShouldDrawAnInventoryContainer() {
        (World w, Inventory inv, City c, Train t, InventoryContainer<Train> container, DrawInventoryContainerMessage<Train> dm) = init(); 

        int cEntity = EntityFactory.Add(w); 
    
        int dmEnt = EntityFactory.Add(w); 
        w.SetComponent<DrawInventoryContainerMessage<Train>>(dmEnt, dm); 
        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(InventoryContainer<Train>)]));
    }

    [Fact]
    public void DrawInventoryContainerSystem_DrawShouldReturnAnEntityWithAnInventoryComponent() {
        (World w, Inventory inv, City c, Train t, InventoryContainer<Train> container, DrawInventoryContainerMessage<Train> dm) = init(); 
        int e = DrawInventoryContainerSystem.Draw<Train>(dm, w); 
        Assert.Equal(inv, w.GetComponent<Inventory>(e)); 
    }
}