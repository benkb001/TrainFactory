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

public static class TrainWrap {
    public static Train GetTestTrain() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv); 
        return new Train(inv, c); 
    }

    public static int Add(World w, Train t) {
        if (EntityFactory.GetDataEntity<Inventory>(w, t.Inv) == -1) {
            InventoryWrap.Add(w, t.Inv); 
        }
        
        int tEnt = EntityFactory.AddData<Train>(w, t); 

        foreach (KeyValuePair<CartType, Inventory> kvp in t.Carts) {
            Inventory cur = kvp.Value; 
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
    }
}