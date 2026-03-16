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

            LinearLayoutWrap.AddChild(w, mainRow.GetParentEntity(), outerContainer);

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
                LinearLayoutWrap.AddChild(w, tEnt, trainsView);
                //ICKY, can we pass entity nums into drawCityInterfaceMessage?
                int trainDataEnt = ComponentID.GetEntity<Train>(train.ID, w);
                w.SetComponent<TrainUI>(tEnt, new TrainUI(train, trainDataEnt)); 
                w.SetComponent<TextBox>(tEnt, new TextBox(train.Id)); 
                w.SetComponent<Button>(tEnt, new Button());
                w.SetComponent<Frame>(tEnt, new Frame(w.GetCameraTopLeft(), 100, 100));  
                w.SetComponent<Outline>(tEnt, new Outline()); 
            }

            LinearLayoutWrap.AddChild(w, trainsView.GetParentEntity(), mainRow);

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
                LinearLayoutWrap.AddChild(w, mEnt, machinesView);
                w.SetComponent<EnterInterfaceButton<MachineInterfaceData>>(mEnt, 
                    new EnterInterfaceButton<MachineInterfaceData>(new MachineInterfaceData(machine, city)));
                w.SetComponent<TextBox>(mEnt, new TextBox(machine.Id)); 
                w.SetComponent<Button>(mEnt, new Button()); 
                w.SetComponent<Outline>(mEnt, new Outline()); 
            }

            LinearLayoutWrap.AddChild(w, machinesView.GetParentEntity(), mainRow);
            LinearLayoutWrap.ResizeChildren(w, mainRow, recurse: true); 
            LinearLayoutWrap.ResizeChildren(w, machinesView);
            LinearLayoutWrap.ResizeChildren(w, trainsView);

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

            LinearLayoutWrap.AddChild(w, invView.GetParentEntity(), invRow); 

            if (playerInv != null && city.HasPlayer) {
                InventoryView playerInvView = DrawInventoryCallback.Draw(w, playerInv, 
                    Vector2.Zero, playerInvWidth, playerInvHeight, DrawLabel: true);
            
                LinearLayoutWrap.AddChild(w, playerInvView.GetParentEntity(), invRow);
            }

            int upgradeDepotBtnEnt = EntityFactory.AddUI(w, Vector2.Zero, invWidth / 2, invWidth / 4, 
                setButton: true, setOutline: true, text: $"Upgrade {city.Id} Depot? Requires 1 Depot Upgrade");
            w.SetComponent<UpgradeDepotButton>(upgradeDepotBtnEnt, new UpgradeDepotButton(city)); 
            LinearLayoutWrap.AddChild(w, upgradeDepotBtnEnt, invRow);

            LinearLayoutContainer connectButtons = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                invRowWidth / 4, 
                invRowHeight, 
                direction: "vertical", 
                outline: false
            ); 
            LinearLayoutWrap.AddChild(w, connectButtons.GetParentEntity(), invRow);

            List<string> connectedCities = city.AdjacentCities
            .Select(c => c.Id)
            .ToList(); 

            CityID.CityMap[city.Id].FutureConnections
            .Where(kvp => !connectedCities.Contains(kvp.Key))
            .ToList()
            .ForEach(kvp => {
                string otherID = kvp.Key; 
                Dictionary<string, int> cost = kvp.Value; 

                int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, setButton: true, setOutline: true,
                    text: $"Add Railroad to {otherID}? \n{Util.FormatMap(cost)}"); 
                City otherCity = CityWrap.GetByID(w, otherID); 
                w.SetComponent<ConnectCitiesButton>(btnEnt, new ConnectCitiesButton(city, otherCity, cost));
                LinearLayoutWrap.AddChild(w, btnEnt, connectButtons);  
            }); 

            LinearLayoutWrap.ResizeChildren(w, connectButtons);

            LinearLayoutWrap.AddChild(w, invRow.GetParentEntity(), outerContainer);

            w.RemoveEntity(e);
        }); 
    }
    
    public static void AddMessage(World w, City c) {
        MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(c)); 
    }
}