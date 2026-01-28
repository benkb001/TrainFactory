namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public static class DrawCityInterfaceSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(DrawCityInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.CityInterface); 

            LinearLayoutContainer outerContainer = LinearLayoutWrap.AddOuter(w);

            City city = w.GetComponent<DrawCityInterfaceMessage>(e).GetCity();
            Inventory inv = city.Inv;
            Inventory playerInv = InventoryWrap.GetPlayerInv(w); 

            int mFlagEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(mFlagEnt, new Menu(city: city)); 
            
            LinearLayoutContainer mainRow = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                outerContainer.LLWidth, 
                outerContainer.LLHeight / 1.4f,
                direction: "horizontal", 
                outline: false
            );

            outerContainer.AddChild(mainRow.GetParentEntity(), w); 

            LinearLayoutContainer trainsView = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                0, 
                0, 
                direction: "vertical", 
                label: "Trains",
                outline: true, 
                usePaging: true, 
                childrenPerPage: 4
            );

            foreach (Train train in city.Trains.Values) {
                int tEnt = EntityFactory.Add(w); 
                trainsView.AddChild(tEnt, w); 
                w.SetComponent<TrainUI>(tEnt, new TrainUI(train)); 
                w.SetComponent<TextBox>(tEnt, new TextBox(train.Id)); 
                w.SetComponent<Button>(tEnt, new Button());
                w.SetComponent<Frame>(tEnt, new Frame(w.GetCameraTopLeft(), 100, 100));  
                w.SetComponent<Outline>(tEnt, new Outline()); 
            }

            mainRow.AddChild(trainsView.GetParentEntity(), w); 

            LinearLayoutContainer machinesView = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                0, 
                0, 
                direction: "vertical", 
                label: "Machines",
                outline: true, 
                usePaging: true, 
                childrenPerPage: 4
            );

            foreach (Machine machine in city.Machines.Values) {
                int mEnt = EntityFactory.Add(w); 
                machinesView.AddChild(mEnt, w); 
                w.SetComponent<MachineUI>(mEnt, new MachineUI(machine));
                w.SetComponent<TextBox>(mEnt, new TextBox(machine.Id)); 
                w.SetComponent<Button>(mEnt, new Button()); 
                w.SetComponent<Outline>(mEnt, new Outline()); 
            }

            mainRow.AddChild(machinesView.GetParentEntity(), w); 
            mainRow.ResizeChildren(w, recurse: true); 
            machinesView.ResizeChildren(w); 
            trainsView.ResizeChildren(w); 

            //add inventories to bottom     

            float invScale = 0.7f;
            (float invWidth, float invHeight) = InventoryWrap.GetUI(inv, invScale); 
            (float playerInvWidth, float playerInvHeight) = InventoryWrap.GetUI(playerInv, invScale); 

            InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, invWidth, invHeight, 
                Padding: Constants.InventoryPadding, DrawLabel: true); 

            float invRowWidth = outerContainer.LLWidth; 
            float invRowHeight = Math.Max(playerInvHeight, invHeight) + 20f; 

            LinearLayoutContainer invRow = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                invRowWidth, 
                invRowHeight, 
                direction: "horizontal", 
                outline: false
            );

            invRow.AddChild(invView.GetParentEntity(), w); 

            int upgradeDepotBtnEnt = EntityFactory.AddUI(w, Vector2.Zero, invWidth / 2, invWidth / 4, 
                setButton: true, setOutline: true, text: $"Upgrade {city.Id} Depot? Requires 1 Depot Upgrade");
            w.SetComponent<UpgradeDepotButton>(upgradeDepotBtnEnt, new UpgradeDepotButton(city)); 
            invRow.AddChild(upgradeDepotBtnEnt, w); 

            if (playerInv != null && city.HasPlayer) {
                InventoryView playerInvView = DrawInventoryCallback.Draw(w, playerInv, 
                    Vector2.Zero, playerInvWidth, playerInvHeight, DrawLabel: true);
            
                invRow.AddChild(playerInvView.GetParentEntity(), w); 
            }

            outerContainer.AddChild(invRow.GetParentEntity(), w);

            w.RemoveEntity(e);
        }); 
    }
}