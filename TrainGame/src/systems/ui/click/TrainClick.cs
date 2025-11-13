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

//required order: 
// trainClick -> push -> drawTrainDetail 
public class TrainClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(TrainUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            //TODO:
            //if moving draw a summary of the train inventory (or the inventory itself, still havent decided there)
            if (w.GetComponent<Button>(e).Clicked) {
                Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
                if (!t.IsTraveling()) {
                    int pushEntity = EntityFactory.Add(w); 
                    w.SetComponent(pushEntity, PushSceneMessage.Get()); 

                    Vector2 embark_position = w.GetComponent<Frame>(e).Position; 
                    
                    int msgEntity = EntityFactory.Add(w); 
                    w.SetComponent<DrawEmbarkMessage>(msgEntity, 
                        new DrawEmbarkMessage(
                            t,
                            embark_position, 
                            Constants.EmbarkLayoutWidth, 
                            Constants.EmbarkLayoutHeight, 
                            Constants.EmbarkLayoutPadding
                        )); 
                    
                    Inventory trainInv = t.Inv; 
                    Inventory cityInv = t.ComingFrom.Inv; 
                    
                    float cellWidth = Constants.InventoryCellSize + Constants.InventoryPadding; 

                    float trainInvWidth = cellWidth * (trainInv.GetCols() + 1);
                    float trainInvHeight = cellWidth * (trainInv.GetRows() + 1);
                    float cityInvWidth = cellWidth * (cityInv.GetCols() + 1); 
                    float cityInvHeight = cellWidth * (cityInv.GetRows() + 1); 

                    Vector2 trainInvPosition = embark_position + new Vector2(Constants.EmbarkLayoutWidth, 0); 
                    Vector2 cityInvPosition = trainInvPosition + new Vector2(trainInvWidth, 0); 
                    DrawInventoryMessage drawTrainInv = new DrawInventoryMessage(
                        Width: trainInvWidth, 
                        Height: trainInvHeight, 
                        Position: trainInvPosition, 
                        Inv: trainInv, 
                        Padding: Constants.InventoryPadding, 
                        SetMenu: true
                    ); 

                    int drawTrainInvMsgEntity = EntityFactory.Add(w); 
                    w.SetComponent<DrawInventoryMessage>(drawTrainInvMsgEntity, drawTrainInv); 

                    DrawInventoryMessage drawCityInv = new DrawInventoryMessage(
                        Width: cityInvWidth, 
                        Height: cityInvHeight, 
                        Position: cityInvPosition, 
                        Inv: cityInv, 
                        Padding: Constants.InventoryPadding, 
                        SetMenu: true
                    ); 

                    int drawCityInvMsgEntity = EntityFactory.Add(w); 
                    w.SetComponent<DrawInventoryMessage>(drawCityInvMsgEntity, drawCityInv); 
                }
                
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}