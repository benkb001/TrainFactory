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
using TrainGame.Systems;

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

            container.Redraw(index, w); 
        }); 
    }
}