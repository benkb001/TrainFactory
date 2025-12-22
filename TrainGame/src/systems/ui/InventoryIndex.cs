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
            
            LinearLayoutWrap.Clear(containerEntity, w);
            w.RemoveComponent<Label>(e); 
            w.RemoveComponent<Inventory>(e); 

            Inventory inv = Inventories[index]; 
            Frame f = w.GetComponent<Frame>(containerEntity); 

            Frame labelFrame = new Frame(f.Position, 0, 0); 
            int labelEntity = -1; 

            if (w.ComponentContainsEntity<Body>(containerEntity)) {
                labelEntity = w.GetComponent<Body>(containerEntity).LabelEntity; 
                labelFrame = w.GetComponent<Frame>(labelEntity); 
            }

            DrawInventoryCallback.Draw(w, inv, labelFrame.Position, f.GetWidth(), f.GetHeight() + labelFrame.GetHeight(), 
                Entity: containerEntity, Padding: Constants.InventoryPadding, DrawLabel: true);
            
            w.RemoveEntity(labelEntity);
        }); 
    }
}