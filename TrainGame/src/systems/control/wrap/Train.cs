namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS;
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Constants;

public static class TrainWrap {
    public static Train GetTestTrain() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv); 
        string id = ID.GetNext("Train ");
        return new Train(inv, c.RealPosition, new Dictionary<CartType, Inventory>(), id); 
    }

    public static Train Assemble(City origin) {
        Inventory inv = new Inventory(ID.GetNext("Loc"), Constants.TrainRows, Constants.TrainCols); 
        Dictionary<CartType, Inventory> carts = new();

        string id = ID.GetNext("Train ");

        foreach (CartType type in Cart.AllTypes) {
            Inventory curInv = new Inventory(Train.GetCartID(type, id), Constants.CartRows, Constants.CartCols, 0, type);
            carts[type] = curInv; 
        }

        return new Train(inv, origin.RealPosition, carts, id, power: Constants.TrainDefaultPower, mass: Constants.TrainDefaultMass);
    }

    public static int AssembleToWorld(World w, City c) {
        return RegisterExisting(w, Assemble(c), c);
    }

    public static int RegisterExisting(World w, Train t, City c) {
        if (EntityFactory.GetDataEntity<Inventory>(w, t.Inv) == -1) {
            InventoryWrap.Add(w, t.Inv); 
        }
        
        int tEnt = EntityFactory.AddData<Train>(w, t); 

        List<Inventory> invs = t.Carts.Values.ToList(); 
        invs.Add(t.Inv);
        foreach (Inventory cur in invs) {
            if (EntityFactory.GetDataEntity<Inventory>(w, cur) == -1) {
                InventoryWrap.Add(w, cur); 
            }
        }

        w.SetComponent<ComingFromCity>(tEnt, new ComingFromCity(c));
        w.SetComponent<GoingToCity>(tEnt, new GoingToCity(c));
        c.AddTrain(t);

        return tEnt; 
    }

    public static Train GetTrainWithPlayer(World w) {
        List<Train> ts =  w.GetMatchingEntities([typeof(Train), typeof(Data)])
            .Select(e => w.GetComponent<Train>(e))
            .Where(t => t.HasPlayer)
            .ToList(); 
        
        return ts.Count > 0 ? ts[0] : null; 
    }

    public static void Embark(Train t, int trainEnt, City dest, World w, WorldTime left = null) {
        City comingFrom = w.GetComponent<ComingFromCity>(trainEnt);
        if (comingFrom != dest) {
            if (left == null) {
                left = w.Time;
            }
            t.Embark(dest.RealPosition, left); 
            MakeMessage.Add<TrainEmbarkedMessage>(w, new TrainEmbarkedMessage(t)); 
            w.SetComponent<GoingToCity>(trainEnt, new GoingToCity(dest));
            dest.SendTrain(t, comingFrom);
        }
    }

    public static City GetComingFrom(World w, Train t) {
        int trainEnt = ComponentID.GetEntity<Train>(t.ID, w); 
        return w.GetComponent<ComingFromCity>(trainEnt);
    }
}