namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

//required order: 
// trainClick -> push -> drawTrainDetail 
public class TrainClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(TrainUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            //TODO:
            //if moving draw a view detailing the train's inventory, fuel, etc 
            if (w.GetComponent<Button>(e).Clicked) {
                Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
                if (!t.IsTraveling()) {
                    View.EnterMenu(w);
                    int pushEntity = EntityFactory.Add(w); 
                    w.SetComponent(pushEntity, PushSceneMessage.Get()); 
                    
                    Vector2 embark_position = w.GetCameraTopLeft() + new Vector2(10f, 10f); 
                    
                    int msgEntity = EntityFactory.Add(w); 
                    float embarkWidth = w.ScreenWidth / 3f;
                    w.SetComponent<DrawEmbarkMessage>(msgEntity, 
                        new DrawEmbarkMessage(
                            t,
                            embark_position, 
                            embarkWidth, 
                            w.ScreenHeight - 25f, 
                            w.ScreenHeight / 100f
                        )
                    ); 
                    
                    Inventory trainInv = t.Inv; 
                    Inventory cityInv = t.ComingFrom.Inv; 
                    
                    float cellWidth = Constants.InventoryCellSize + Constants.InventoryPadding; 

                    float trainInvWidth = cellWidth * trainInv.GetCols();
                    float trainInvHeight = cellWidth * trainInv.GetRows();
                    float cityInvWidth = cellWidth * cityInv.GetCols(); 
                    float cityInvHeight = cellWidth * cityInv.GetRows(); 

                    Vector2 trainInvPosition = embark_position + new Vector2(embarkWidth + 10f, 0); 
                    Vector2 cityInvPosition = trainInvPosition + new Vector2(0, trainInvHeight + 10f); 

                    DrawInventoryCallback.Create(w, trainInv, trainInvPosition, trainInvWidth, trainInvHeight, 
                        SetMenu: true, DrawLabel: true);

                    DrawInventoryCallback.Create(w, cityInv, cityInvPosition, cityInvWidth, cityInvHeight, 
                        SetMenu: true, DrawLabel: true);
                }
                
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}