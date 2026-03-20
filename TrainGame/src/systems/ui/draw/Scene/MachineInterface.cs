namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Utils;

public static class DrawMachineInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<MachineInterfaceData>(w, (w, e, data) => {

            Machine m = data.GetMachine();
            City city = data.GetCity();
            bool playerAtMachine = m.PlayerAtMachine;
            
            //Make container 
            int containerEnt = EntityFactory.Add(w); 
            LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
            ll.Padding = 5f;
            w.SetComponent<LinearLayout>(containerEnt, ll); 
            Vector2 llPos = w.GetCameraTopLeft() + new Vector2(10, 10); 
            float llWidth = w.ScreenWidth - 20f; 
            float llHeight = w.ScreenHeight - 20f; 
            w.SetComponent<Frame>(containerEnt, new Frame(llPos, llWidth, llHeight)); 
            w.SetComponent<Outline>(containerEnt, new Outline()); 

            //draw machine inventory
            Inventory inv = m.Inv; 

            (float invWidth, float invHeight) = InventoryWrap.GetUI(inv); 
            float leftColWidth = invWidth + 10f; 

            int invEnt = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, invWidth, invHeight, 
                Padding: Constants.InventoryPadding, DrawLabel: true).GetInventoryEntity(); 
            int invContainerEnt = LinearLayoutContainer.GetParent(invEnt, w); 

            //draw left column and add machine inv to it
            int leftColEnt = EntityFactory.Add(w); 
            LinearLayout leftCol = new LinearLayout("vertical", "alignlow"); 
            leftCol.Padding = 5f;
            w.SetComponent<LinearLayout>(leftColEnt, leftCol); 
            w.SetComponent<Frame>(leftColEnt, new Frame(Vector2.Zero, leftColWidth, llHeight - 10f)); 
            w.SetComponent<Outline>(leftColEnt, new Outline()); 
            LinearLayoutContainer.AddChild(leftColEnt, containerEnt, ll, w); 

            //draw header
            
            int headerEntity = EntityFactory.Add(w); 
            LinearLayoutContainer.AddChild(headerEntity, leftColEnt, leftCol, w);
            float headerWidth = invWidth;
            float headerHeight = w.ScreenHeight - invHeight - Constants.LabelHeight - 20f; 

            w.SetComponent<TextBox>(headerEntity, new TextBox("")); 
            w.SetComponent<MachineHeader>(headerEntity, new MachineHeader(m)); 
            w.SetComponent<Frame>(headerEntity, new Frame(Vector2.Zero, headerWidth, headerHeight)); 
            w.SetComponent<Outline>(headerEntity, new Outline());

            //add inv to left col under header 
            LinearLayoutContainer.AddChild(invContainerEnt, leftColEnt, leftCol, w); 

            //draw steppers for priority and storage
            int midColEnt = EntityFactory.Add(w); 
            LinearLayout midCol = new LinearLayout("vertical", "alignlow"); 
            midCol.Padding = 5f; 
            float midColWidth = w.ScreenWidth / 8f; 
            float midColHeight = llHeight - 10f; 
            w.SetComponent<LinearLayout>(midColEnt, midCol); 
            w.SetComponent<Frame>(midColEnt, new Frame(Vector2.Zero, midColWidth, midColHeight)); 
            w.SetComponent<Outline>(midColEnt, new Outline()); 
            LinearLayoutContainer.AddChild(midColEnt, containerEnt, ll, w); 

            float stepperWidth = midColWidth - 10f;
            float stepperHeight = (midColHeight / 2f) - 20f;
            StepperContainer setPrioStepper = StepperWrap.Draw(w, stepperWidth, 
                stepperHeight, $"Set {m.Id} priority?", defaultVal: m.Priority);
            w.SetComponent<MachinePriorityStepper>(setPrioStepper.SubmitEnt, 
                new MachinePriorityStepper(m, setPrioStepper.Step));
            LinearLayoutContainer.AddChild(setPrioStepper.ContainerEnt, midColEnt, midCol, w); 

            StepperContainer setSizeStepper = StepperWrap.Draw(w, stepperWidth, stepperHeight, 
                $"Set {m.Id} storage size?", defaultVal: m.NumRecipeToStore);
            w.SetComponent<MachineStorageStepper>(setSizeStepper.SubmitEnt, 
                new MachineStorageStepper(m, setSizeStepper.Step));
            LinearLayoutContainer.AddChild(setSizeStepper.ContainerEnt, midColEnt, midCol, w); 

            //Draw progress bar

            float pbWidth = w.ScreenWidth - midColWidth - leftColWidth - 55f; 
            float pbHeight = pbWidth / 10f; 

            int pbEntity = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight); 
            w.SetComponent<Machine>(pbEntity, m); 
            
            //draw upgrade button, clickable only if player inv was specified 
            Vector2 upgradePosition = Vector2.Zero;  
            float upgradeWidth = pbWidth; 
            float upgradeHeight = pbHeight * 2; 
            int upgradeEntity = DrawUpgradeMachineButtonCallback.Draw(w, m, city, 
                upgradePosition, upgradeWidth, upgradeHeight);
            
            //draw manual progress bar if this machine can be manually collected 
            int manualPbEnt = -1; 
            int manualCraftButtonEnt = -1; 

            bool drawManualCraft = m.AllowManual && playerAtMachine;
            if (drawManualCraft) {
                float manualPbWidth = pbWidth; 
                float manualPbHeight = pbHeight; 
                manualPbEnt = DrawProgressBarCallback.Draw(w, Vector2.Zero, manualPbWidth, manualPbHeight); 

                manualCraftButtonEnt = EntityFactory.Add(w); 

                w.SetComponent<ManualCraftButton>(manualCraftButtonEnt, new ManualCraftButton(manualPbEnt)); 
                w.SetComponent<Machine>(manualCraftButtonEnt, m); 
                w.SetComponent<Button>(manualCraftButtonEnt, new Button()); 
                w.SetComponent<Frame>(manualCraftButtonEnt, new Frame(Vector2.Zero, manualPbWidth, manualPbHeight));
                w.SetComponent<Outline>(manualCraftButtonEnt, new Outline()); 
                w.SetComponent<TextBox>(manualCraftButtonEnt, new TextBox("Click and Hold to Craft")); 
            }

            //add to linear layout
            LinearLayout col = new LinearLayout("vertical", "alignlow"); 
            col.Padding = 5f; 
            int colEnt = EntityFactory.Add(w); 
            LinearLayoutContainer.AddChild(colEnt, containerEnt, ll, w); 
            w.SetComponent<LinearLayout>(colEnt, col); 
            w.SetComponent<Frame>(colEnt, new Frame(Vector2.Zero, pbWidth + 10f, w.ScreenHeight - 20f));
            w.SetComponent<Outline>(colEnt, new Outline()); 

            LinearLayoutContainer.AddChild(pbEntity, colEnt, col, w);
            LinearLayoutContainer.AddChild(upgradeEntity, colEnt, col, w); 
            if (drawManualCraft) {
                LinearLayoutContainer.AddChild(manualPbEnt, colEnt, col, w);
                LinearLayoutContainer.AddChild(manualCraftButtonEnt, colEnt, col, w); 
            }

            int upgradeSpeedBtnEnt = EntityFactory.AddUI(w, Vector2.Zero, pbWidth, pbWidth / 4f, 
                setOutline: true, text: $"Increase Craft Speed? Requires 1 {ItemID.Accelerator}",
                setButton: true);
            w.SetComponent<UpgradeMachineSpeedButton>(upgradeSpeedBtnEnt, new UpgradeMachineSpeedButton(m, city));
            LinearLayoutContainer.AddChild(upgradeSpeedBtnEnt, colEnt, col, w);

            int upgradeRatioBtnEnt = EntityFactory.AddUI(w, Vector2.Zero, pbWidth, pbWidth / 4f, 
                setOutline: true, text: $"Increase Product Count? Requires 1 {ItemID.Duplicator}",
                setButton: true);
            w.SetComponent<UpgradeMachineProductCountButton>(upgradeRatioBtnEnt, new UpgradeMachineProductCountButton(m, city));
            LinearLayoutContainer.AddChild(upgradeRatioBtnEnt, colEnt, col, w);

            w.RemoveEntity(e); 
        });
    }

    public static void AddMessage(World w, Machine m, City city) {
        MakeMessage.Add<DrawInterfaceMessage<MachineInterfaceData>>(w, 
            new DrawInterfaceMessage<MachineInterfaceData>(new MachineInterfaceData(m, city)));
    }
}

public class UpgradeMachineSpeedButton {
    private Machine machine; 
    private City city;
    public Machine GetMachine() => machine; 
    public City GetCity() => city;

    public UpgradeMachineSpeedButton(Machine machine, City city) {
        this.machine = machine; 
        this.city = city;
    }
}

public class UpgradeMachineProductCountButton {
    private Machine machine; 
    private City city;
    public Machine GetMachine() => machine; 
    public City GetCity() => city;

    public UpgradeMachineProductCountButton(Machine machine, City city) {
        this.machine = machine; 
        this.city = city;
    }
}

public static class UpgradeMachineSpeedClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeMachineSpeedButton>(w, (w, e, btn) => {
            Machine m = btn.GetMachine();
            City c = btn.GetCity();
            if (m.Inv.Take(ItemID.Accelerator, 1).Count == 1) {
                m.UpgradeSpeed();
                DrawMachineInterfaceSystem.AddMessage(w, m, c);
            }
        });
    }
}

public static class UpgradeMachineProductCountClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeMachineProductCountButton>(w, (w, e, btn) => {
            Machine m = btn.GetMachine(); 
            City c = btn.GetCity();
            if (m.Inv.Take(ItemID.Duplicator, 1).Count == 1) {
                m.UpgradeProductCountExponential(); 
                DrawMachineInterfaceSystem.AddMessage(w, m, c);
            }
        });
    }
}