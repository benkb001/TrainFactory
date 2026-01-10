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
            int invEnt = EntityFactory.Add(w, setData: true); 
            w.SetComponent<Inventory>(invEnt, t.Inv); 
        }
        
        int tEnt = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Train>(tEnt, t); 

        foreach (KeyValuePair<CartType, Inventory> kvp in t.Carts) {
            Inventory cur = kvp.Value; 
            if (EntityFactory.GetDataEntity<Inventory>(w, cur) == -1) {
                int curEnt = EntityFactory.Add(w, setData: true); 
                w.SetComponent<Inventory>(curEnt, cur); 
            }
        }

        return tEnt; 
    }

    public static void Embark(Train t, City dest, World w) {
        t.Embark(dest, w.Time); 
    }
}