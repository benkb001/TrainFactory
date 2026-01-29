namespace TrainGame.Components; 

using System.Linq; 

using TrainGame.ECS;

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
}