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
            DrawMachineInterfaceMessage dm = w.GetComponent<DrawMachineInterfaceMessage>(e); 
            Machine m = dm.GetMachine(); 
            bool playerAtMachine = dm.PlayerAtMachine;

            //draw machine inventory
            Inventory inv = m.Inv; 

            (float invWidth, float invHeight) = InventoryWrap.GetUI(inv); 
            float invY = w.ScreenHeight - invHeight - 10f; 
            float invX = 25f; 

            DrawInventoryCallback.Draw(w, inv, w.GetCameraTopLeft() + new Vector2(invX, invY), invWidth, invHeight, 
                Padding: Constants.InventoryPadding, DrawLabel: true); 

            //draw header
            
            Vector2 topLeft = w.GetCameraTopLeft(); 
            float headerWidth = w.ScreenWidth / 2.5f; 
            float headerHeight = w.ScreenHeight - invHeight - 20f; 
            Vector2 headerPosition = topLeft + new Vector2(10, 10); 
            
            string hStr = $"{m.Id}\n"; 
            hStr += $"Level: {m.Level}\n"; 
            hStr += $"Craft Speed: {m.GetCraftSpeedFormatted()}"; 
            hStr += $"Recipe: \n{m.GetRecipeFormatted()}"; 
            TextBox headerTextBox = new TextBox(hStr); 

            int headerEntity = EntityFactory.Add(w); 
            w.SetComponent<TextBox>(headerEntity, new TextBox(hStr)); 
            w.SetComponent<Frame>(headerEntity, new Frame(headerPosition, headerWidth, headerHeight)); 
            w.SetComponent<Outline>(headerEntity, new Outline()); 

            //draw request stepper if not produce infinite (which means it produces as much as it can all the time)
            float requestWidth = w.ScreenWidth / 8f; 
            float requestHeight = headerHeight;
            Vector2 requestPosition = headerPosition + new Vector2(headerWidth + 10f, 0f); 

            if (!m.ProduceInfinite) {
                DrawMachineRequestCallback.Draw(w, m, requestPosition, requestWidth, requestHeight); 
            }

            //Draw progress bar

            float pbWidth = w.ScreenWidth - requestWidth - headerWidth - 45f; 
            float pbHeight = pbWidth / 10f; 

            int pbEntity = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight); 
            w.SetComponent<Machine>(pbEntity, m); 
            
            //draw upgrade button, clickable only if player inv was specified 
            Vector2 upgradePosition = Vector2.Zero;  
            float upgradeWidth = pbWidth; 
            float upgradeHeight = pbHeight * 2; 
            int upgradeEntity = DrawUpgradeMachineButtonCallback.Draw(w, m, playerAtMachine, 
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
            Vector2 llPos = requestPosition + new Vector2(requestWidth + 10, 0); 
            LinearLayout ll = new LinearLayout("vertical", "alignlow"); 
            ll.Padding = 5f; 
            int llEnt = EntityFactory.Add(w); 
            w.SetComponent<LinearLayout>(llEnt, ll); 
            w.SetComponent<Frame>(llEnt, new Frame(llPos, pbWidth + 10f, w.ScreenHeight - 20f));
            w.SetComponent<Outline>(llEnt, new Outline()); 

            LinearLayoutWrap.AddChild(pbEntity, llEnt, ll, w);
            LinearLayoutWrap.AddChild(upgradeEntity, llEnt, ll, w); 
            if (drawManualCraft) {
                LinearLayoutWrap.AddChild(manualPbEnt, llEnt, ll, w);
                LinearLayoutWrap.AddChild(manualCraftButtonEnt, llEnt, ll, w); 
            }

            w.RemoveEntity(e); 
        });
    }
}