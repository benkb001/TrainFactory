namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Callbacks; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InventoryIndexSystem {
    public static void Register<T>(World w) where T : IInventorySource {
        ClickSystem.Register<InventoryIndexer<T>>(w, (w, e) => {
            InventoryIndexer<T> dexer = w.GetComponent<InventoryIndexer<T>>(e); 
            InventoryContainer<T> container = dexer.Container; 

            int containerEntity = dexer.ContainerEntity; 
            int index = container.Index + dexer.Delta; 

            List<Inventory> Inventories = container.GetInventories(); 

            if (index < 0) {
                index = Inventories.Count - 1; 
            } else if (index >= Inventories.Count) {
                index = 0; 
            }

            container.Index = index; 
            
            Frame f = w.GetComponent<Frame>(containerEntity); 

            if (w.ComponentContainsEntity<LLChild>(containerEntity)) {
                int outerEnt = w.GetComponent<LLChild>(containerEntity).ParentEntity; 
                f = w.GetComponent<Frame>(outerEnt); 

                int clearEnt = EntityFactory.Add(w); 
                w.SetComponent<ClearLLMessage>(clearEnt, new ClearLLMessage(outerEnt)); 
            }

            DrawInventoryContainerMessage<T> dm = new DrawInventoryContainerMessage<T>(
                container,
                f.Position,
                f.GetWidth(), 
                f.GetHeight()
            ); 
            int dmEnt = EntityFactory.Add(w); 
            w.SetComponent<DrawInventoryContainerMessage<T>>(dmEnt, dm); 
            
        }); 
    }
}