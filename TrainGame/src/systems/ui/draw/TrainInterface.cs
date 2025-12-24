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

public class DrawTrainInterfaceSystem {

    private static void addButtons(LinearLayout container, Train t, World w, float height) {
        //add column for buttons 

        LinearLayout buttonsContainer = new LinearLayout("Vertical", "alignlow"); 
        buttonsContainer.Padding = Constants.InventoryPadding; 
        int buttonsContainerEnt = EntityFactory.Add(w); 
        container.AddChild(buttonsContainerEnt); 

        float buttonsContainerWidth = w.ScreenWidth / 5f; 
        float buttonsContainerHeight = height; 
        w.SetComponent<LinearLayout>(buttonsContainerEnt, buttonsContainer);
        w.SetComponent<Frame>(buttonsContainerEnt, new Frame(0, 0, buttonsContainerWidth, buttonsContainerHeight)); 

        //add upgrade button    
        //TODO: This should only be clickable if the player is at the city

        float buttonWidth = buttonsContainerWidth - (2 * Constants.InventoryPadding); 
        float buttonHeight = buttonsContainerHeight / 6f; 

        Inventory playerInv = InventoryWrap.GetyByEntityOrId(w, inventoryId: Constants.PlayerInvID);

        UpgradeTrainButton upgradeTrainBtn = new UpgradeTrainButton(t, playerInv); 
        DrawButtonMessage<UpgradeTrainButton> upgradeMsg = new DrawButtonMessage<UpgradeTrainButton>(
            Position: Vector2.Zero, 
            Width: buttonWidth, 
            Height: buttonHeight, 
            Button: upgradeTrainBtn
        );

        int upgradeBtnEnt = DrawButtonSystem.Draw<UpgradeTrainButton>(upgradeMsg, w); 
        buttonsContainer.AddChild(upgradeBtnEnt); 

        //draw Add Cart button 
        //TODO: this should only be clickable if there is a cart to add at the city
        int addCartEntity = EntityFactory.Add(w, setScene: false); 

        AddCartInterfaceButton cartBtn = new AddCartInterfaceButton(CartDest: t, CartSource: t.ComingFrom); 

        DrawButtonMessage<AddCartInterfaceButton> addCartMsg = new DrawButtonMessage<AddCartInterfaceButton>(
            Button: cartBtn,
            Position: Vector2.Zero, 
            Width: buttonWidth, 
            Height: buttonHeight
        ); 

        int addCartInterfaceEnt = DrawButtonSystem.Draw<AddCartInterfaceButton>(addCartMsg, w); 
        buttonsContainer.AddChild(addCartInterfaceEnt); 
    }

    private static void addEmbark(LinearLayout container, Train t, World w, float height) {
        float embarkWidth = w.ScreenWidth / 5f;
        int embarkEnt = DrawEmbarkSystem.Draw(
            new DrawEmbarkMessage(
                t,
                Vector2.Zero, 
                embarkWidth, 
                height,
                w.ScreenHeight / 100f
            ), 
            w
        ); 

        container.AddChild(embarkEnt);
    }

    private static void addInvs(LinearLayout container, Train t, World w, float height) {
        Inventory trainInv = t.Inv; 
        Inventory cityInv = t.ComingFrom.Inv; 
        
        float cellWidth = Constants.InventoryCellSize + Constants.InventoryPadding; 

        float trainInvWidth = cellWidth * trainInv.GetCols();
        float trainInvHeight = cellWidth * trainInv.GetRows();
        float cityInvWidth = cellWidth * cityInv.GetCols(); 
        float cityInvHeight = cellWidth * cityInv.GetRows(); 
        float invsContainerWidth = Math.Max(trainInvWidth, cityInvWidth) + 2 * Constants.InventoryPadding; 
        float invsContainerHeight = height; 

        DrawInventoryContainerMessage<Train> containerDm = new DrawInventoryContainerMessage<Train>(
            new InventoryContainer<Train>(t), Vector2.Zero, trainInvWidth, trainInvHeight, SetMenu: true);
        
        int trainInvEnt = DrawInventoryContainerSystem.Draw<Train>(containerDm, w); 
        int trainOuterEnt = w.GetComponent<LLChild>(trainInvEnt).ParentEntity; 

        int cityInvEnt = DrawInventoryCallback.Draw(w, cityInv, Vector2.Zero, cityInvWidth, cityInvHeight, 
            SetMenu: true, DrawLabel: true);
        int cityOuterEnt = w.GetComponent<LLChild>(cityInvEnt).ParentEntity; 

        LinearLayout invsContainer = new LinearLayout("vertical", "alignlow"); 
        invsContainer.Padding = Constants.InventoryPadding;
        int invsContainerEnt = EntityFactory.Add(w); 
        container.AddChild(invsContainerEnt); 

        w.SetComponent<LinearLayout>(invsContainerEnt, invsContainer); 
        w.SetComponent<Frame>(invsContainerEnt, new Frame(0, 0, invsContainerWidth, invsContainerHeight));
        invsContainer.AddChild(trainOuterEnt); 
        invsContainer.AddChild(cityOuterEnt);
    }

    public static void Register(World w) {
        w.AddSystem([typeof(DrawTrainInterfaceMessage)], (w, e) => {
            DrawTrainInterfaceMessage dm = w.GetComponent<DrawTrainInterfaceMessage>(e); 
            Train t = dm.GetTrain(); 

            Vector2 containerPos = w.GetCameraTopLeft() + new Vector2(10f, 10f); 
            float containerWidth = w.ScreenWidth - 20f; 
            float containerHeight = w.ScreenHeight - 20f; 
            float columnHeight = containerHeight - (3 * Constants.InventoryPadding); 

            int containerEnt = EntityFactory.Add(w); 
            LinearLayout container = new LinearLayout("horizontal", "alignlow"); 
            container.Padding = Constants.InventoryPadding; 

            w.SetComponent<LinearLayout>(containerEnt, container); 
            w.SetComponent<Frame>(containerEnt, new Frame(containerPos, containerWidth, containerHeight)); 

            addEmbark(container, t, w, columnHeight); 
            addInvs(container, t, w, columnHeight); 
            addButtons(container, t, w, columnHeight); 
             
            //TODO: add a way to remove carts here
            w.RemoveEntity(e); 
        }); 
    }
}