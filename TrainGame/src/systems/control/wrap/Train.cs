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
        return new Train(inv, c, new Dictionary<CartType, Inventory>(), id); 
    }

    public static Train Assemble(City origin) {
        Inventory inv = new Inventory(ID.GetNext("Loc"), Constants.TrainRows, Constants.TrainCols); 
        Dictionary<CartType, Inventory> carts = new();

        string id = ID.GetNext("Train ");

        foreach (CartType type in Cart.AllTypes) {
            Inventory curInv = new Inventory(Train.GetCartID(type, id), Constants.CartRows, Constants.CartCols, 0, type);
            carts[type] = curInv; 
        }

        return new Train(inv, origin, carts, id, power: Constants.TrainDefaultPower, mass: Constants.TrainDefaultMass);
    }

    public static int AssembleToWorld(World w, City c) {
        return RegisterExisting(w, Assemble(c));
    }

    public static int RegisterExisting(World w, Train t) {
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

        return tEnt; 
    }

    public static Train GetTrainWithPlayer(World w) {
        List<Train> ts =  w.GetMatchingEntities([typeof(Train), typeof(Data)])
            .Select(e => w.GetComponent<Train>(e))
            .Where(t => t.HasPlayer)
            .ToList(); 
        
        return ts.Count > 0 ? ts[0] : null; 
    }

    public static void Embark(Train t, City dest, World w) {
        t.Embark(dest, w.Time); 
        MakeMessage.Add<TrainEmbarkedMessage>(w, new TrainEmbarkedMessage(t)); 
    }
}