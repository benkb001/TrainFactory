using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class DrawMapSystemTest {
    [Fact]
    public void DrawMapSystem_ShouldDrawASingleCityForEachCityEntity() {
        World w = WorldFactory.Build(); 

        for (int i = 0; i < 3; i++) {
            int cEntity = EntityFactory.Add(w, setData: true);
            City c = new City($"C{i}", new Inventory($"C{i}", 1, 1)); 
            w.SetComponent<City>(cEntity, c); 
        }
        
        int msgEntity = EntityFactory.Add(w); 
        w.SetComponent<DrawMapMessage>(msgEntity, DrawMapMessage.Get()); 

        w.Update(); 

        List<int> drawnCityEntities = w.GetMatchingEntities([typeof(CityUI)]);

        Assert.Equal(3, drawnCityEntities.Count);
        Assert.Single(drawnCityEntities, c => {
            return w.GetComponent<CityUI>(c).GetCity().CityId == "C0"; 
        }); 
        Assert.Single(drawnCityEntities, c => {
            return w.GetComponent<CityUI>(c).GetCity().CityId == "C1"; 
        }); 
        Assert.Single(drawnCityEntities, c => {
            return w.GetComponent<CityUI>(c).GetCity().CityId == "C2"; 
        }); 
    }
}