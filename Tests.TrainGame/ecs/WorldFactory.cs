
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class WorldFactoryTest {
    [Fact]
    public void WorldFactory_BuildShouldRegisterComponentsAndSystems() {
        World w = WorldFactory.Build(); 
        Assert.True(w.GetComponentTypeCount() >= 5); 
        Assert.True(w.SystemCount() >= 5); 
    }   
}