namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class DrawInventoryContainerSystem {
    public static void Register<T>(World w) where T : IInventorySource {
        w.AddSystem(
            [typeof(DrawInventoryContainerMessage<T>)], 
            (w, e) => {
                DrawInventoryContainerMessage<T> dm = w.GetComponent<DrawInventoryContainerMessage<T>>(e); 

                InventoryContainer<T> invContainer = new InventoryContainer<T>(dm.InvSource);  

                int containerEntity = DrawInventoryCallback.Draw(w, dm.InvSource.GetInventories()[0], 
                    dm.Position, dm.Width, dm.Height, Padding: Constants.InventoryPadding, SetMenu: dm.SetMenu, 
                    DrawLabel: dm.DrawLabel);
                
                w.SetComponent<InventoryContainer<T>>(containerEntity, invContainer); 
                
                int indexerEntity = EntityFactory.Add(w); 

                float indexContainerWidth = dm.Width / 8f; 
                float indexContainerHeight = indexContainerWidth / 2f; 

                float indexerWidth = indexContainerWidth / 2.5f; 
                float indexerHeight = indexContainerHeight - (2 * Constants.InventoryPadding); 

                Vector2 indexerPosition = dm.Position + new Vector2(dm.Width - indexContainerWidth, 0f); 

                w.SetComponent<Frame>(indexerEntity, new Frame(indexerPosition, indexContainerWidth, indexContainerHeight));
                w.SetComponent<Outline>(indexerEntity, new Outline()); 

                LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
                ll.Padding = Constants.InventoryPadding;
                w.SetComponent<LinearLayout>(indexerEntity, ll); 

                int[] directions = [-1, 1]; 
                foreach (int d in directions) {
                    int indexEntity = EntityFactory.Add(w); 
                    w.SetComponent<InventoryIndexer<T>>(indexEntity, new InventoryIndexer<T>(invContainer, containerEntity, 1)); 
                    w.SetComponent<Button>(indexEntity, new Button()); 
                    w.SetComponent<Outline>(indexEntity, new Outline()); 

                    List<Vector2> points = new(); 
                    points.Add(new Vector2(0, 0)); 
                    points.Add(new Vector2(0, indexerHeight));
                    points.Add(new Vector2(indexerWidth * d, indexerHeight / 2f)); 

                    w.SetComponent<Frame>(indexEntity, new Frame(points));

                    ll.AddChild(indexEntity); 
                }
                w.RemoveEntity(e); 
            }
        );
    }
}