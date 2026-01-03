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

    private static void addButtons(LinearLayout container, int containerEnt, Train t, World w, float height) {
        //add column for buttons 

        LinearLayout buttonsContainer = new LinearLayout("Vertical", "alignlow"); 
        buttonsContainer.Padding = Constants.InventoryPadding; 
        int buttonsContainerEnt = EntityFactory.Add(w); 
        LinearLayoutWrap.AddChild(buttonsContainerEnt, containerEnt, container, w);

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
        LinearLayoutWrap.AddChild(upgradeBtnEnt, buttonsContainerEnt, buttonsContainer, w);

        //draw Add Cart button 
        //TODO: this should only be clickable if there is a cart to add at the city

        AddCartInterfaceButton cartBtn = new AddCartInterfaceButton(CartDest: t, CartSource: t.ComingFrom); 

        DrawButtonMessage<AddCartInterfaceButton> addCartMsg = new DrawButtonMessage<AddCartInterfaceButton>(
            Button: cartBtn,
            Position: Vector2.Zero, 
            Width: buttonWidth, 
            Height: buttonHeight
        ); 

        int addCartInterfaceEnt = DrawButtonSystem.Draw<AddCartInterfaceButton>(addCartMsg, w); 
        LinearLayoutWrap.AddChild(addCartInterfaceEnt, buttonsContainerEnt, buttonsContainer, w);

        //draw button to go to Set Train Program Interface
        int programBtn = EntityFactory.Add(w);
        LinearLayoutWrap.AddChild(programBtn, buttonsContainerEnt, buttonsContainer, w); 
        w.SetComponent<Frame>(programBtn, new Frame(0, 0, buttonWidth, buttonHeight)); 
        w.SetComponent<Outline>(programBtn, new Outline()); 
        w.SetComponent<TextBox>(programBtn, new TextBox("Program train?")); 
        w.SetComponent<Button>(programBtn, new Button()); 
        w.SetComponent<SetTrainProgramInterfaceButton>(programBtn, new SetTrainProgramInterfaceButton(t));
    }

    private static void addEmbark(LinearLayout container, int containerEnt, Train t, World w, float height) {
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

        LinearLayoutWrap.AddChild(embarkEnt, containerEnt, container, w);
    }

    private static void addInvs(LinearLayout container, int containerEnt, Train t, World w, float height) {
        Inventory trainInv = t.Inv; 
        Inventory cityInv = t.ComingFrom.Inv; 
        
        float cellWidth = Constants.InventoryCellSize + Constants.InventoryPadding; 

        (float trainInvWidth, float trainInvHeight) = InventoryWrap.GetUI(trainInv); 
        (float cityInvWidth, float cityInvHeight) = InventoryWrap.GetUI(cityInv); 
        float invsContainerWidth = Math.Max(trainInvWidth, cityInvWidth) + 2 * Constants.InventoryPadding; 
        float invsContainerHeight = height; 

        DrawInventoryContainerMessage<Train> containerDm = new DrawInventoryContainerMessage<Train>(
            new InventoryContainer<Train>(t), Vector2.Zero, trainInvWidth, trainInvHeight, SetMenu: true);
        
        int trainInvEnt = DrawInventoryContainerSystem.Draw<Train>(containerDm, w); 
        int trainOuterEnt = w.GetComponent<LLChild>(trainInvEnt).ParentEntity; 

        int cityInvEnt = DrawInventoryCallback.Draw(w, cityInv, Vector2.Zero, cityInvWidth, cityInvHeight, 
            Padding: Constants.InventoryPadding, SetMenu: true, DrawLabel: true);
        int cityOuterEnt = w.GetComponent<LLChild>(cityInvEnt).ParentEntity; 

        LinearLayout invsContainer = new LinearLayout("vertical", "alignlow"); 
        invsContainer.Padding = Constants.InventoryPadding;
        int invsContainerEnt = EntityFactory.Add(w); 
        
        LinearLayoutWrap.AddChild(invsContainerEnt, containerEnt, container, w);

        w.SetComponent<LinearLayout>(invsContainerEnt, invsContainer); 
        w.SetComponent<Frame>(invsContainerEnt, new Frame(0, 0, invsContainerWidth, invsContainerHeight));
        LinearLayoutWrap.AddChild(trainOuterEnt, invsContainerEnt, invsContainer, w); 
        LinearLayoutWrap.AddChild(cityOuterEnt, invsContainerEnt, invsContainer, w); 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(DrawTrainInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.TrainInterface); 
            DrawTrainInterfaceMessage dm = w.GetComponent<DrawTrainInterfaceMessage>(e); 
            Train t = dm.GetTrain(); 

            int menuEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(menuEnt, new Menu(train: t)); 

            Vector2 containerPos = w.GetCameraTopLeft() + new Vector2(10f, 10f); 
            float containerWidth = w.ScreenWidth - 20f; 
            float containerHeight = w.ScreenHeight - 20f; 
            float columnHeight = containerHeight - (3 * Constants.InventoryPadding); 

            int containerEnt = EntityFactory.Add(w); 
            LinearLayout container = new LinearLayout("horizontal", "alignlow"); 
            container.Padding = Constants.InventoryPadding; 

            w.SetComponent<LinearLayout>(containerEnt, container); 
            w.SetComponent<Frame>(containerEnt, new Frame(containerPos, containerWidth, containerHeight)); 

            addEmbark(container, containerEnt, t, w, columnHeight); 
            addInvs(container, containerEnt, t, w, columnHeight); 
            addButtons(container, containerEnt, t, w, columnHeight); 
             
            //TODO: add a way to remove carts here
            w.RemoveEntity(e); 
        }); 
    }
}