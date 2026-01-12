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

public static class DrawMachineInterfaceSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(DrawMachineInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.MachineInterface); 

            DrawMachineInterfaceMessage dm = w.GetComponent<DrawMachineInterfaceMessage>(e); 
            Machine m = dm.GetMachine(); 
            bool playerAtMachine = m.PlayerAtMachine;

            int menuEnt = EntityFactory.Add(w); 

            City city = dm.GetCity();
            w.SetComponent<Menu>(menuEnt, new Menu(city: city, machine: m));
            
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
            int invContainerEnt = LinearLayoutWrap.GetParent(invEnt, w); 

            //draw left column and add machine inv to it
            int leftColEnt = EntityFactory.Add(w); 
            LinearLayout leftCol = new LinearLayout("vertical", "alignlow"); 
            leftCol.Padding = 5f;
            w.SetComponent<LinearLayout>(leftColEnt, leftCol); 
            w.SetComponent<Frame>(leftColEnt, new Frame(Vector2.Zero, leftColWidth, llHeight - 10f)); 
            w.SetComponent<Outline>(leftColEnt, new Outline()); 
            LinearLayoutWrap.AddChild(leftColEnt, containerEnt, ll, w); 

            //draw header
            
            int headerEntity = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(headerEntity, leftColEnt, leftCol, w);
            float headerWidth = invWidth;
            float headerHeight = w.ScreenHeight - invHeight - Constants.LabelHeight - 20f; 

            w.SetComponent<TextBox>(headerEntity, new TextBox("")); 
            w.SetComponent<MachineHeader>(headerEntity, new MachineHeader(m)); 
            w.SetComponent<Frame>(headerEntity, new Frame(Vector2.Zero, headerWidth, headerHeight)); 
            w.SetComponent<Outline>(headerEntity, new Outline());

            //add inv to left col under header 
            LinearLayoutWrap.AddChild(invContainerEnt, leftColEnt, leftCol, w); 

            //draw steppers for priority and storage
            int midColEnt = EntityFactory.Add(w); 
            LinearLayout midCol = new LinearLayout("vertical", "alignlow"); 
            midCol.Padding = 5f; 
            float midColWidth = w.ScreenWidth / 8f; 
            float midColHeight = llHeight - 10f; 
            w.SetComponent<LinearLayout>(midColEnt, midCol); 
            w.SetComponent<Frame>(midColEnt, new Frame(Vector2.Zero, midColWidth, midColHeight)); 
            w.SetComponent<Outline>(midColEnt, new Outline()); 
            LinearLayoutWrap.AddChild(midColEnt, containerEnt, ll, w); 

            float stepperWidth = midColWidth - 10f;
            float stepperHeight = (midColHeight / 2f) - 20f;
            StepperContainer setPrioStepper = StepperWrap.Draw(stepperWidth, 
                stepperHeight, $"Set {m.Id} priority?", w, defaultVal: m.Priority);
            w.SetComponent<MachinePriorityStepper>(setPrioStepper.SubmitEnt, 
                new MachinePriorityStepper(m, setPrioStepper.Step));
            LinearLayoutWrap.AddChild(setPrioStepper.ContainerEnt, midColEnt, midCol, w); 

            StepperContainer setSizeStepper = StepperWrap.Draw(stepperWidth, stepperHeight, 
                $"Set {m.Id} storage size?", w, defaultVal: m.NumRecipeToStore);
            w.SetComponent<MachineStorageStepper>(setSizeStepper.SubmitEnt, 
                new MachineStorageStepper(m, setSizeStepper.Step));
            LinearLayoutWrap.AddChild(setSizeStepper.ContainerEnt, midColEnt, midCol, w); 

            //Draw progress bar

            float pbWidth = w.ScreenWidth - midColWidth - leftColWidth - 55f; 
            float pbHeight = pbWidth / 10f; 

            int pbEntity = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight); 
            w.SetComponent<Machine>(pbEntity, m); 
            
            //draw upgrade button, clickable only if player inv was specified 
            Vector2 upgradePosition = Vector2.Zero;  
            float upgradeWidth = pbWidth; 
            float upgradeHeight = pbHeight * 2; 
            int upgradeEntity = DrawUpgradeMachineButtonCallback.Draw(w, m, 
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
            LinearLayoutWrap.AddChild(colEnt, containerEnt, ll, w); 
            w.SetComponent<LinearLayout>(colEnt, col); 
            w.SetComponent<Frame>(colEnt, new Frame(Vector2.Zero, pbWidth + 10f, w.ScreenHeight - 20f));
            w.SetComponent<Outline>(colEnt, new Outline()); 

            LinearLayoutWrap.AddChild(pbEntity, colEnt, col, w);
            LinearLayoutWrap.AddChild(upgradeEntity, colEnt, col, w); 
            if (drawManualCraft) {
                LinearLayoutWrap.AddChild(manualPbEnt, colEnt, col, w);
                LinearLayoutWrap.AddChild(manualCraftButtonEnt, colEnt, col, w); 
            }

            w.RemoveEntity(e); 
        });
    }
}