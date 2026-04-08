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
        dom.Add("maxFloor", Globals.MaxFloor);

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
        Dictionary<string, int> tEnts = trains
        .Select(kvp => new KeyValuePair<string, int>(kvp.Key, ComponentID.GetEntity<Train>(kvp.Key, w)))
        .ToDictionary();

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
            int trainEnt = tEnts[t.ID];
            (TALBody<Train, City> exe, bool s) = w.GetComponentSafe<TALBody<Train, City>>(trainEnt);
            int nextInstruction = (!s) ? 0 : exe.NextInstruction(); 
            City comingFrom = w.GetComponent<ComingFromCity>(trainEnt);
            City goingTo = w.GetComponent<GoingToCity>(trainEnt);

            JsonObject trainJSON = new JsonObject() {
                ["comingFromID"] = comingFrom.GetID(),
                ["goingToID"] = goingTo.GetID(),
                ["inventoryID"] = t.Inv.GetID(),
                ["isTraveling"] = t.IsTraveling(),
                ["left"] = JSONObjectFromWorldTime(t.DepartureTime),
                ["mass"] = t.Mass, 
                ["milesOfFuel"] = t.MilesOfFuel,
                ["milesPerHour"] = t.MilesPerHour, 
                ["nextInstruction"] = nextInstruction,
                ["power"] = t.Power,
                ["program"] = t.Program,
                ["programName"] = t.ProgramName,
                ["x"] = t.Position.X, 
                ["y"] = t.Position.Y
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
                ["whitelist"] = new JsonArray(inv.GetWhitelist().Select(s => (JsonNode)s).ToArray()),
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
                ["priority"] = machine.Priority,
                ["productCount"] = machine.ProductCount,
                ["speedLevel"] = machine.SpeedLevel
            });
        })));

        Inventory playerInv = PlayerWrap.GetInventory(w); 
        Health playerHealth = PlayerWrap.GetHP(w);
        Parrier playerParrier = PlayerWrap.GetParrier(w);

        int maxHP = playerHealth.MaxHP; 
        int hp = playerHealth.HP;
        int parryHP = playerParrier.HP; 
        int maxParryHP = playerParrier.MaxHP; 

        dom.Add("player", new JsonObject() {
            ["inventoryID"] = playerInv.Id, 
            ["maxHP"] = maxHP,
            ["HP"] = hp,
            ["parryHP"] = parryHP,
            ["maxParryHP"] = maxParryHP
        });

        dom.Add("cities", new JsonObject(cities.Select(kvp => {
            string cityID = kvp.Key; 
            City city = kvp.Value; 

            JsonObject carts = new JsonObject(); 
            foreach(CartType type in Cart.AllTypes) {
                carts.Add(Convert.ToInt32(type).ToString(), city.NumCarts(type));
            }

            List<string> connections = city.AdjacentCities
            .Select(c => c.Id)
            .ToList();

            return new KeyValuePair<string, JsonNode>(cityID, new JsonObject() {
                ["carts"] = carts,
                ["connections"] = JsonNode.Parse(JsonSerializer.Serialize<List<string>>(connections)).AsArray()
            });
        })));

        dom.Add("playerGuns", new JsonObject(EquipmentSlot<PlayerGun>.EquipmentMap.Select(kvp => {
            string id = kvp.Key; 
            PlayerGun gun = kvp.Value; 

            return new KeyValuePair<string, JsonNode>(id, new JsonObject() {
                ["damageLevel"] = gun.DamageLevel
            });
        })));

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
        Globals.MaxFloor = (int)dom["maxFloor"];

        Dictionary<string, Inventory> inventories = new(); 
        Dictionary<string, int> inventoryEnts = new(); 
        Dictionary<string, City> cities = new(); 
        Dictionary<string, Train> trains = new(); 
        Dictionary<string, int> tEnts = new();
        Dictionary<string, (int, Machine)> machines = new(); 

        foreach (KeyValuePair<string, JsonNode> kvp in dom["inventories"].AsObject()) {
            string invID = kvp.Key; 
            JsonObject invData = kvp.Value.AsObject(); 

            (int e, Inventory inv) = InventoryWrap.Add(w, invID, (int)invData["rows"], (int)invData["cols"], 
                (int)invData["level"]); 

            foreach (string s in JsonSerializer.Deserialize<IEnumerable<string>>(invData["whitelist"])) {
                inv.Whitelist(s);
            }
            foreach (string itemID in ItemID.All) {
                inv.Add(itemID, (int)invData["items"][itemID]);
            }
            
            inventories.Add(invID, inv); 
            inventoryEnts[invID] = e; 
            w.SetComponent<InventoryUpdatedFlag>(e, InventoryUpdatedFlag.Get());
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
                int speedLevel = (int)machineData["speedLevel"];
                int productCount = (int)machineData["productCount"];

                Machine m = Machines.Get(
                    inv, 
                    machineID, 
                    curCraftTicks: curCraftTicks, 
                    state: state,
                    priority: priority,
                    level: level,
                    numRecipeToStore: numRecipeToStore
                ); 
                
                m.SetProductCount(productCount);
                m.UpgradeSpeed(speedLevel);
                m.SetLifetimeProductsCrafted(lifetimeProductsCrafted); 

                int e = EntityFactory.AddData<Machine>(w, m); 
                machines[machineID] = (e, m); 
                c.AddMachine(m); 
            }
        }

        JsonObject citiesJSON = dom["cities"].AsObject(); 

        foreach (KeyValuePair<string, City> kvp in cities) {
            string cityID = kvp.Key; 
            City city = kvp.Value; 

            JsonObject cityJSON = citiesJSON[cityID].AsObject(); 
            JsonObject cartsJSON = cityJSON["carts"].AsObject(); 

            foreach (KeyValuePair<string, JsonNode> cartAmount in cartsJSON) {
                CartType type = (CartType)(int.Parse(cartAmount.Key)); 
                for (int i = 0; i < (int)cartsJSON[cartAmount.Key]; i++) {
                    city.AddCart(type); 
                }
            }

            JsonArray connectionsJSON = cityJSON["connections"].AsArray();
            foreach (JsonNode otherJSON in connectionsJSON) {
                string otherID = (string)otherJSON;
                city.AddConnection(cities[otherID]);
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
            float milesOfFuel = (float)trainData["milesOfFuel"];
            string comingFromID = (string)trainData["comingFromID"]; 
            float x = (float)trainData["x"];
            float y = (float)trainData["y"];
            City comingFrom = cities[comingFromID]; 
            Dictionary<CartType, Inventory> carts = new(); 

            foreach (CartType type in Cart.AllTypes) {
                carts[type] = inventories[Train.GetCartID(type, trainID)]; 
            }

            Train t = new Train(inv, comingFrom.RealPosition, carts, trainID, milesPerHour, power, mass, milesOfFuel: milesOfFuel); 
            t.SetPosition(x, y);
            int trainEnt = TrainWrap.RegisterExisting(w, t, comingFrom); 
            tEnts[t.ID] = trainEnt;
            trains[trainID] = t; 
            string program = (string)trainData["program"];
            string programName = (string)trainData["programName"];

            int nextInstruction = (int)trainData["nextInstruction"];
            if (!string.IsNullOrEmpty(program)) {
                TAL.SetTrainProgram(program, t, trainEnt, w, nextInstruction, programName: programName);
            }

            if (isTraveling) {
                string goingToID = (string)trainData["goingToID"]; 
                City goingTo = cities[goingToID];
                WorldTime left = WorldTimeFromJSONObject(trainData["left"].AsObject()); 
                TrainWrap.Embark(t, trainEnt, goingTo, w, left);
            }
        }

        foreach (KeyValuePair<string, JsonNode> kvp in dom["playerGuns"].AsObject()) {

            string gunID = kvp.Key; 
            JsonObject gunData = kvp.Value.AsObject(); 
            int damageLevel = (int)gunData["damageLevel"];
            
            for (int i = 0; i < damageLevel; i++) {
                EquipmentSlot<PlayerGun>.EquipmentMap[gunID].UpgradeDamage();
            }
        }

        JsonObject playerJSON = dom["player"].AsObject(); 

        Inventory playerInv = inventories[(string)playerJSON["inventoryID"]];

        int maxHP = (int)playerJSON["maxHP"];
        int hp = (int)playerJSON["HP"];
        int parryHP = (int)playerJSON["parryHP"];
        int maxParryHP = (int)playerJSON["maxParryHP"];

        Parrier playerParrier = new Parrier(maxParryHP, parryHP);

        Health playerHealth = new Health(maxHP); 
        playerHealth.SetHP(hp);

        int playerDataEnt = PlayerWrap.AddData(w, playerInv, playerHealth, playerParrier);

        void registerEquipmentSlot<T>() where T : IEquippable {
            Inventory inv = inventories[Constants.EquipmentInvID<T>()];
            int equipInvEnt = inventoryEnts[inv.ID];
            w.SetComponent<EquipmentSlot<T>>(playerDataEnt, EquipmentSlotWrap.Add<T>(w, inv, equipInvEnt));
        }

        registerEquipmentSlot<PlayerGun>();
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
        w.SetComponent<CartAssembler>(liquidAssemblerEnt, 
            new CartAssembler(factory, liquidAssembler, CartType.Liquid));

        if (cities.ContainsKey(playerLocation)) {
            MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(cities[playerLocation]));
        } else if (trains.ContainsKey(playerLocation)) {
            Train t = trains[playerLocation];
            int tEnt = tEnts[t.ID];
            t.HasPlayer = true; 
            DrawTravelingInterfaceSystem.AddMessage(w, t, tEnt); 
        } else {
            throw new InvalidOperationException("Couldn't find player"); 
        }
    }
}