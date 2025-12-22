
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 

public class InventoryIndexSystemTest {
    [Fact]
    public void InventoryIndexSystem_ShouldChangeWhichInventoryIsDrawn() {
        World w = WorldFactory.Build(); 
        
        Inventory inv = new Inventory("TrainInv", 1, 1); 
        City city = new City("City", inv); 
        Train t = new Train(inv, city); 

        for (int i = 0; i < 3; i++) {
            t.AddCart(new Cart($"C{i}", CartType.Freight));
        }

        InventoryContainer<Train> container = new InventoryContainer<Train>(t); 
        int cEntity = EntityFactory.Add(w); 
        w.SetComponent<InventoryContainer<Train>>(cEntity, container); 
        w.SetComponent<Frame>(cEntity, new Frame(Vector2.Zero, 100, 100));

        int[] ds = [-1, 1]; 

        List<int> es = new(); 

        for (int i = 0; i < 2; i++) {
            InventoryIndexer<Train> dexer = new InventoryIndexer<Train>(container, cEntity, ds[i]);

            int dEnt = EntityFactory.Add(w); 
            w.SetComponent<InventoryIndexer<Train>>(dEnt, dexer); 

            es.Add(dEnt); 
        }

        w.SetComponent<Button>(es[0], new Button(true)); 
        w.Update(); 
        
        Assert.False(w.GetComponent<Button>(es[0]).Clicked); 
        Assert.Contains("C2", w.GetComponent<Inventory>(cEntity).Id); 
        
        w.SetComponent<Button>(es[1], new Button(true));
        w.Update(); 
        Assert.Contains("TrainInv", w.GetComponent<Inventory>(cEntity).Id); 
    }
}