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

    private static void addButtons(LinearLayout container, int containerEnt, Train t, int trainEnt, World w, float height) {
        //add column for buttons 

        LinearLayout buttonsContainer = new LinearLayout("Vertical", "alignlow"); 
        buttonsContainer.Padding = Constants.InventoryPadding; 
        int buttonsContainerEnt = EntityFactory.Add(w); 
        LinearLayoutContainer.AddChild(buttonsContainerEnt, containerEnt, container, w);

        float buttonsContainerWidth = w.ScreenWidth / 5f; 
        float buttonsContainerHeight = height; 
        w.SetComponent<LinearLayout>(buttonsContainerEnt, buttonsContainer);
        w.SetComponent<Frame>(buttonsContainerEnt, new Frame(0, 0, buttonsContainerWidth, buttonsContainerHeight)); 

        float buttonWidth = buttonsContainerWidth - (2 * Constants.InventoryPadding); 
        float buttonHeight = buttonsContainerHeight / 6f; 

        //add train summary 
        string summary = t.GetSummary();

        int sumEnt = EntityFactory.AddUI(w, Vector2.Zero, buttonWidth, buttonHeight * 2, setOutline: true, 
            text: summary);
        LinearLayoutContainer.AddChild(sumEnt, buttonsContainerEnt, buttonsContainer, w);

        //draw Add Cart button 
        //TODO: this should only be clickable if there is a cart to add at the city

        int upgradeTrainButtonEnt = EntityFactory.AddUI(w, Vector2.Zero, buttonWidth, buttonHeight, 
            setButton: true, setOutline: true, text: "Upgrade Train"); 
        w.SetComponent<EnterInterfaceButton<UpgradeTrainInterfaceData>>(upgradeTrainButtonEnt, 
            new EnterInterfaceButton<UpgradeTrainInterfaceData>(new UpgradeTrainInterfaceData(t, trainEnt)));

        LinearLayoutContainer.AddChild(upgradeTrainButtonEnt, buttonsContainerEnt, buttonsContainer, w);

        //draw button to go to Set Train Program Interface
        int programBtn = EntityFactory.Add(w);
        LinearLayoutContainer.AddChild(programBtn, buttonsContainerEnt, buttonsContainer, w); 
        w.SetComponent<Frame>(programBtn, new Frame(0, 0, buttonWidth, buttonHeight)); 
        w.SetComponent<Outline>(programBtn, new Outline()); 
        w.SetComponent<TextBox>(programBtn, new TextBox("Program train")); 
        w.SetComponent<Button>(programBtn, new Button()); 
        w.SetComponent<SetTrainProgramInterfaceButton>(programBtn, new SetTrainProgramInterfaceButton(t, trainEnt));
        (TALBody<Train, City> exe, bool hasExe) = w.GetComponentSafe<TALBody<Train, City>>(trainEnt);

        if (hasExe) {
            buttonsContainer.AddChild(PauseTrainProgramButtonWrap.Add(w, exe, buttonWidth, buttonHeight));
        }
    }

    private static void addEmbark(LinearLayout container, int containerEnt, Train t, int trainEnt, City comingFrom, World w, float height) {
        float embarkWidth = w.ScreenWidth / 5f;
        int embarkEnt = DrawEmbarkSystem.Draw(
            new DrawEmbarkMessage(
                t,
                trainEnt, 
                comingFrom,
                Vector2.Zero, 
                embarkWidth, 
                height,
                w.ScreenHeight / 100f
            ), 
            w
        ); 

        LinearLayoutContainer.AddChild(embarkEnt, containerEnt, container, w);
    }

    private static void addInvs(LinearLayout container, int containerEnt, Train t, Inventory cityInv, World w, float height) {
        Inventory trainInv = t.Inv;

        (float trainInvWidth, float trainInvHeight) = InventoryWrap.GetUI(trainInv); 
        (float cityInvWidth, float cityInvHeight) = InventoryWrap.GetUI(cityInv); 
        float invsContainerWidth = Math.Max(trainInvWidth, cityInvWidth) + 2 * Constants.InventoryPadding; 
        float invsContainerHeight = height; 

        DrawInventoryContainerMessage<Train> containerDm = new DrawInventoryContainerMessage<Train>(
            t, Vector2.Zero, trainInvWidth, trainInvHeight);
        
        InventoryContainer<Train> trainInvContainer = DrawInventoryContainerSystem.Draw<Train>(containerDm, w); 

        InventoryView cityInvView = DrawInventoryCallback.Draw(w, cityInv, Vector2.Zero, cityInvWidth, cityInvHeight, 
            Padding: Constants.InventoryPadding, DrawLabel: true);

        LinearLayout invsContainer = new LinearLayout("vertical", "alignlow"); 
        invsContainer.Padding = Constants.InventoryPadding;
        int invsContainerEnt = EntityFactory.Add(w); 
        
        LinearLayoutContainer.AddChild(invsContainerEnt, containerEnt, container, w);

        w.SetComponent<LinearLayout>(invsContainerEnt, invsContainer); 
        w.SetComponent<Frame>(invsContainerEnt, new Frame(0, 0, invsContainerWidth, invsContainerHeight));
        LinearLayoutContainer.AddChild(trainInvContainer.GetParentEntity(), invsContainerEnt, invsContainer, w); 
        LinearLayoutContainer.AddChild(cityInvView.GetParentEntity(), invsContainerEnt, invsContainer, w); 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(DrawTrainInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.TrainInterface); 
            DrawTrainInterfaceMessage dm = w.GetComponent<DrawTrainInterfaceMessage>(e); 
            Train t = dm.GetTrain(); 
            int trainEnt = dm.TrainEntity;
            (City comingFrom, bool hasComingFrom) = TrainWrap.GetComingFrom(w, trainEnt);
            
            if (!hasComingFrom) {
                throw new InvalidOperationException(
                    $"Tried to draw train interface but {trainEnt} has no comingFromCity");
            }

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

            addButtons(container, containerEnt, t, trainEnt, w, columnHeight); 
            addInvs(container, containerEnt, t, comingFrom.Inv, w, columnHeight); 
            addEmbark(container, containerEnt, t, trainEnt, comingFrom, w, columnHeight); 
             
            w.RemoveEntity(e); 
        }); 
    }
}