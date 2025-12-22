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
            //actually should probably make a different component for that

            if (w.GetComponent<Button>(e).Clicked) {
                Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
                if (!t.IsTraveling()) {
                    View.EnterMenu(w);
                    PushFactory.Build(w); 
                    
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

                    int containerDmEnt = EntityFactory.Add(w, setScene: false); 
                    DrawInventoryContainerMessage<Train> containerDm = new DrawInventoryContainerMessage<Train>(
                        t, trainInvPosition, trainInvWidth, trainInvHeight, SetMenu: true, DrawLabel: true);
                    w.SetComponent<DrawInventoryContainerMessage<Train>>(containerDmEnt, containerDm); 

                    DrawInventoryCallback.Create(w, cityInv, cityInvPosition, cityInvWidth, cityInvHeight, 
                        SetMenu: true, DrawLabel: true);

                    //add upgrade button 
                    //TODO: This should only be clickable if the player is at the city
                    int upgradeBtnEntity = EntityFactory.Add(w, setScene: false); 
                    Vector2 upgradePosition = trainInvPosition + new Vector2(trainInvWidth + 10f, 0); 
                    float toprightX = w.GetCameraTopLeft().X + w.ScreenWidth; 
                    float upgradeWidth = toprightX - (upgradePosition.X + 10f); 
                    float upgradeHeight = upgradeWidth * 0.5f;
                    UpgradeTrainButton upgradeTrainBtn = new UpgradeTrainButton(t); 

                    DrawButtonMessage<UpgradeTrainButton> upgradeMsg = new DrawButtonMessage<UpgradeTrainButton>(
                        Position: upgradePosition, 
                        Width: upgradeWidth, 
                        Height: upgradeHeight, 
                        Button: upgradeTrainBtn
                    );

                    w.SetComponent<DrawButtonMessage<UpgradeTrainButton>>(upgradeBtnEntity, upgradeMsg); 

                    //draw Add Cart button 
                    //TODO: this should only be clickable if there is a cart to add at the city
                    int addCartEntity = EntityFactory.Add(w, setScene: false); 
                    Vector2 addCartPosition = upgradePosition + new Vector2(0f, upgradeHeight + 10f); 
                    float addCartWidth = upgradeWidth; 
                    float addCartHeight = upgradeHeight; 
                    AddCartInterfaceButton cartBtn = new AddCartInterfaceButton(CartDest: t, CartSource: t.ComingFrom); 

                    DrawButtonMessage<AddCartInterfaceButton> addCartMsg = new DrawButtonMessage<AddCartInterfaceButton>(
                        Button: cartBtn,
                        Position: addCartPosition, 
                        Width: addCartWidth, 
                        Height: addCartHeight
                    ); 

                    w.SetComponent<DrawButtonMessage<AddCartInterfaceButton>>(addCartEntity, addCartMsg); 

                    //TODO: add a way to remove carts here
                }
                
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}