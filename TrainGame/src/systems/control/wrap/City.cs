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
using TrainGame.Constants; 

public static class CityWrap {
    public static City GetTest() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("test", inv);
        return c; 
    }

    public static City GetByID(World w, string id) {
        return w.GetMatchingEntities([typeof(Data), typeof(City)])
        .Select(e => w.GetComponent<City>(e))
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    public static City GetCityWithPlayer(World w) {
        City c = w.GetMatchingEntities([typeof(City), typeof(Data)])
        .Select(e => w.GetComponent<City>(e))
        .Where(c => c.HasPlayer)
        .FirstOrDefault(); 

        if (c == null || c.Equals(default(City))) {
            throw new InvalidOperationException("There is no city with player");
        }

        return c;
    }
}