
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils;
using TrainGame.Callbacks; 
using TrainGame.Constants; 

public class BootstrapTest {
    [Fact]
    public void Bootstrap_InitWorldShouldMakeACityDataEntForEachCity() {
        World w = WorldFactory.Build(); 
        Bootstrap.InitWorld(w); 
        int numCities = CityID.CityMap.Count; 
        Assert.Equal(numCities, w.GetMatchingEntities([typeof(City), typeof(Data)]).Count);
    }
}