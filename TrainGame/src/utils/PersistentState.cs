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

        //TODO: Catch-Up, once we change how carts
        //are implemented, that info should be saved here

        dom.Add("trains", new JsonObject(trains.Select(kvp => {
            Train t = kvp.Value; 
            string id = kvp.Key; 
            return new KeyValuePair<string, JsonNode>(id, new JsonObject() {
                ["arrivalTime"] = JSONObjectFromWorldTime(t.ArrivalTime),
                ["comingFromID"] = t.ComingFrom.GetID(),
                ["goingToID"] = t.GoingTo.GetID(),
                ["inventoryID"] = t.Inv.GetID(),
                ["isTraveling"] = t.IsTraveling(),
                ["left"] = JSONObjectFromWorldTime(t.DepartureTime),
                ["mass"] = t.Mass, 
                ["milesPerHour"] = t.MilesPerHour, 
                ["power"] = t.Power,
                ["program"] = t.Program
            });
        })));

        dom.Add("cities", new JsonObject(cities.Select(kvp => {
            City city = kvp.Value; 
            string id = kvp.Key; 
            return new KeyValuePair<string, JsonNode>(id, new JsonObject() {
                ["inventoryID"] = city.Inv.GetID(),
                ["trains"] = JsonSerializer.Serialize(city.Trains.Select(trainKVP => trainKVP.Key).ToArray()),
            });
        })));

        dom.Add("inventories", new JsonObject(invs.Select(kvp => {
            Inventory inv = kvp.Value; 
            string id = kvp.Key; 
            JsonObject invJson = new JsonObject(ItemID.All.Select(itemID => {
                return new KeyValuePair<string, JsonNode>(itemID, inv.ItemCount(itemID));
            }));
            return new KeyValuePair<string, JsonNode>(id, invJson); 
        })));

        File.WriteAllText(filepath, dom.ToString());
    }

    public static void Load(World w, string filepath) {
        
    }
}
