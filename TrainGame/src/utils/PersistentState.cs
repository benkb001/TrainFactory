namespace TrainGame.Utils; 
using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes; 
using System.Linq; 
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Callbacks; 
using TrainGame.Systems; 
using TrainGame.Constants;

public static class PersistentState {
    
    public static void Save(World w, string filepath) {
        JsonObject dom = new JsonObject(); 

        JsonObject JSONObjectFromWorldTime(WorldTime wt) {
            return new JsonObject() {
                ["days"] = wt.Days, 
                ["hours"] = wt.Hours, 
                ["minutes"] = wt.Minutes,
                ["ticks"] = wt.Ticks
            };
        }

        dom.Add("time", JSONObjectFromWorldTime(w.Time));

        Type[] ts = [typeof(Train), typeof(City), typeof(Inventory), typeof(Machine)];

        List<List<int>> ents = ts.Select(
            t => {
                List<int> es = w.GetMatchingEntities([t, typeof(Data)]);
                return es; 
            }).ToList(); 

        Dictionary<string, T> toDict<T>(int i) where T : IID {
            return ents[i].Select(e => {
                T t = w.GetComponent<T>(e); 
                return new KeyValuePair<string, T>(t.GetID(), t); 
            }).ToDictionary();
        }
        
        Dictionary<string, Train> trains = toDict<Train>(0); 
        Dictionary<string, City> cities = toDict<City>(1); 
        Dictionary<string, Inventory> invs = toDict<Inventory>(2); 
        Dictionary<string, Machine> machines = toDict<Machine>(3); 

        City cityWithPlayer = cities.Where(kvp => kvp.Value.HasPlayer).FirstOrDefault().Value; 
        string playerLocation = ""; 
        if (cityWithPlayer == null) {
            Train trainWithPlayer = trains.Where(kvp => kvp.Value.HasPlayer).FirstOrDefault().Value; 
            if (trainWithPlayer.Equals(default(Train))) {
                throw new InvalidOperationException("Cannot save, was unable to find player"); 
            } else {
                playerLocation = trainWithPlayer.Id; 
            }
        } else {
            playerLocation = cityWithPlayer.Id; 
        }

        dom.Add("playerLocation", playerLocation);

        dom.Add("trains", new JsonObject(trains.Select(kvp => {
            Train t = kvp.Value; 
            string id = kvp.Key; 
            int nextInstruction = (t.Executable == null) ? 0 : t.Executable.NextInstruction; 
            JsonObject trainJSON = new JsonObject() {
                ["comingFromID"] = t.ComingFrom.GetID(),
                ["goingToID"] = t.GoingTo.GetID(),
                ["inventoryID"] = t.Inv.GetID(),
                ["isTraveling"] = t.IsTraveling(),
                ["left"] = JSONObjectFromWorldTime(t.DepartureTime),
                ["mass"] = t.Mass, 
                ["milesPerHour"] = t.MilesPerHour, 
                ["nextInstruction"] = nextInstruction,
                ["power"] = t.Power,
                ["program"] = t.Program
            };

            return new KeyValuePair<string, JsonNode>(id, trainJSON);
        })));

        dom.Add("inventories", new JsonObject(invs.Select(kvp => {
            Inventory inv = kvp.Value; 
            string id = kvp.Key; 
            JsonObject itemsJson = new JsonObject(ItemID.All.Select(itemID => {
                return new KeyValuePair<string, JsonNode>(itemID, inv.ItemCount(itemID));
            }));

            return new KeyValuePair<string, JsonNode>(id, new JsonObject() {
                ["cols"] = inv.Cols, 
                ["filter"] = Convert.ToInt32(inv.Filter),
                ["items"] = itemsJson,
                ["level"] = inv.Level,
                ["rows"] = inv.Rows
            }); 
        })));

        dom.Add("machines", new JsonObject(machines.Select(kvp => {
            string machineID = kvp.Key; 
            Machine machine = kvp.Value; 
            return new KeyValuePair<string, JsonNode>(machineID, new JsonObject() {
                ["state"] = Convert.ToInt32(machine.State),
                ["curCraftTicks"] = machine.CurCraftTicks,
                ["level"] = machine.Level,
                ["lifetimeProductsCrafted"] = machine.LifetimeProductsCrafted,
                ["numRecipeToStore"] = machine.NumRecipeToStore,
                ["priority"] = machine.Priority
            });
        })));

        int playerEnt = PlayerWrap.GetEntity(w); 
        Inventory playerInv = w.GetComponent<Inventory>(playerEnt); 
        Inventory armorInv = w.GetComponent<EquipmentSlot<Armor>>(playerEnt).GetInventory();
        int maxHP = w.GetComponent<Health>(playerEnt).MaxHP; 
        int armor = w.GetComponent<Armor>(playerEnt).Defense; 

        dom.Add("player", new JsonObject() {
            ["armor"] = armor,
            ["armorInventoryID"] = armorInv.Id,
            ["inventoryID"] = playerInv.Id, 
            ["maxHP"] = maxHP
        });

        File.WriteAllText(filepath, dom.ToString());
    }

    public static void Load(World w, string filepath) {
        w.Clear(); 
        EquipmentID.InitMaps(); 

        WorldTime WorldTimeFromJSONObject(JsonObject wtData) {
            return new WorldTime(
                days: (int)wtData["days"],
                hours: (int)wtData["hours"],
                minutes: (int)wtData["minutes"],
                ticks: (int)wtData["ticks"]
            );
        }

        JsonObject dom = JsonNode.Parse(File.ReadAllText(filepath)).AsObject(); 

        w.PassTime(WorldTimeFromJSONObject(dom["time"].AsObject()));

        Dictionary<string, Inventory> inventories = new(); 
        Dictionary<string, City> cities = new(); 
        Dictionary<string, Train> trains = new(); 
        Dictionary<string, (int, Machine)> machines = new(); 

        foreach (KeyValuePair<string, JsonNode> kvp in dom["inventories"].AsObject()) {
            string invID = kvp.Key; 
            JsonObject invData = kvp.Value.AsObject(); 
            CartType filter = JsonSerializer.Deserialize<CartType>((int)invData["filter"]);
            Inventory inv = new Inventory(invID, (int)invData["rows"], (int)invData["cols"], 
                (int)invData["level"], filter); 
            foreach (string itemID in ItemID.All) {
                inv.Add(itemID, (int)invData["items"][itemID]);
            }
            inventories.Add(invID, inv); 
            EntityFactory.AddData<Inventory>(w, inv); 
        }

        foreach (KeyValuePair<string, CityArg> kvp in CityID.CityMap) {
            string cityId = kvp.Key; 
            CityArg args = kvp.Value; 

            Inventory inv = inventories[City.GetInvID(cityId)];

            City c = new City(cityId, inv, args.UiX, args.UiY, args.RealX, args.RealY); 
            int cityEnt = EntityFactory.AddData<City>(w, c); 
            cities[cityId] = c;

            foreach (string machineID in args.Machines) {
                JsonObject machineData = dom["machines"][machineID].AsObject(); 
                CraftState state = JsonSerializer.Deserialize<CraftState>((int)machineData["state"]);
                int curCraftTicks = (int)machineData["curCraftTicks"];
                int priority = (int)machineData["priority"]; 
                int level = (int)machineData["level"];
                int numRecipeToStore = (int)machineData["numRecipeToStore"];
                int lifetimeProductsCrafted = (int)machineData["lifetimeProductsCrafted"];

                Machine m = Machines.Get(
                    inv, 
                    machineID, 
                    curCraftTicks: curCraftTicks, 
                    state: state,
                    priority: priority,
                    level: level,
                    numRecipeToStore: numRecipeToStore
                ); 

                m.SetLifetimeProductsCrafted(lifetimeProductsCrafted); 

                int e = EntityFactory.AddData<Machine>(w, m); 
                machines[machineID] = (e, m); 
                c.AddMachine(m); 
            }
        }

        foreach (KeyValuePair<string, City> kvp in cities) {
            string cityID = kvp.Key; 
            City city = kvp.Value; 
            
            foreach (string otherCityID in CityID.CityMap[cityID].AdjacentCities) {
                City otherCity = cities[otherCityID]; 
                city.AddConnection(otherCity);
            }
        }

        foreach (KeyValuePair<string, JsonNode> kvp in dom["trains"].AsObject()) {
            string trainID = kvp.Key; 
            JsonObject trainData = kvp.Value.AsObject(); 
            Inventory inv = inventories[(string)trainData["inventoryID"]]; 
            bool isTraveling = (bool)trainData["isTraveling"]; 
            float mass = (float)trainData["mass"]; 
            float power = (float)trainData["power"]; 
            float milesPerHour = (float)trainData["milesPerHour"]; 
            string comingFromID = (string)trainData["comingFromID"]; 
            City comingFrom = cities[comingFromID]; 
            Dictionary<CartType, Inventory> carts = new(); 

            foreach (CartType type in Cart.AllTypes) {
                carts[type] = inventories[Train.GetCartID(type, trainID)]; 
            }

            Train t = new Train(inv, comingFrom, trainID, milesPerHour, power, mass, Carts: carts); 
            TrainWrap.Add(w, t); 
            trains[trainID] = t; 
            string program = (string)trainData["program"];
            int nextInstruction = (int)trainData["nextInstruction"];
            if (!string.IsNullOrEmpty(program)) {
                TAL.SetTrainProgram(program, t, w, nextInstruction);
            }

            if (isTraveling) {
                string goingToID = (string)trainData["goingToID"]; 
                City goingTo = cities[goingToID];
                WorldTime left = WorldTimeFromJSONObject(trainData["left"].AsObject()); 
                
                t.Embark(goingTo, left);
            }
        }

        JsonObject playerJSON = dom["player"].AsObject(); 

        Inventory playerInv = inventories[(string)playerJSON["inventoryID"]];
        Inventory armorInv = inventories[(string)playerJSON["armorInventoryID"]];
        int armor = (int)playerJSON["armor"];
        int maxHP = (int)playerJSON["maxHP"];

        Armor playerArmor = new Armor(armor); 
        Health playerHealth = new Health(maxHP); 
        EquipmentSlot<Armor> armorSlot = new EquipmentSlot<Armor>(armorInv); 

        int playerDataEnt = EntityFactory.AddData<Inventory>(w, playerInv); 

        w.SetComponent<Player>(playerDataEnt, new Player()); 
        w.SetComponent<Inventory>(playerDataEnt, playerInv); 
        w.SetComponent<Armor>(playerDataEnt, playerArmor); 
        w.SetComponent<Health>(playerDataEnt, playerHealth); 
        w.SetComponent<EquipmentSlot<Armor>>(playerDataEnt, armorSlot); 
        w.SetComponent<RespawnLocation>(playerDataEnt, new RespawnLocation(cities[CityID.Factory]));

        string playerLocation = (string)dom["playerLocation"];

        City factory = cities[CityID.Factory];
        //set assembler components
        (int locomotiveAssemblerEnt, Machine locomotiveAssembler) = machines[MachineID.LocomotiveAssembler]; 
        w.SetComponent<TrainAssembler>(locomotiveAssemblerEnt, new TrainAssembler(factory, locomotiveAssembler)); 

        (int cargoWagonAssemblerEnt, Machine cargoAssembler) = machines[MachineID.CargoWagonAssembler]; 
        w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
            new CartAssembler(factory, cargoAssembler, CartType.Freight));

        (int liquidAssemblerEnt, Machine liquidAssembler) = machines[MachineID.LiquidWagonAssembler]; 
        w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
            new CartAssembler(factory, liquidAssembler, CartType.Liquid));

        if (cities.ContainsKey(playerLocation)) {
            MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(cities[playerLocation]));
        } else if (trains.ContainsKey(playerLocation)) {
            Train t = trains[playerLocation];
            t.HasPlayer = true; 
            MakeMessage.Add<DrawTravelingInterfaceMessage>(w, new DrawTravelingInterfaceMessage(t));
        } else {
            throw new InvalidOperationException("Couldn't find player"); 
        }
    }
}