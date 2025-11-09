
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class EntityFactoryTest {
    [Fact]
    public void EntityFactory_AddShouldRespectArguments() {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w, setScene: true, setActive: true);
        Assert.True(w.EntityExists(e)); 
        Assert.True(w.ComponentContainsEntity<Scene>(e)); 
        Assert.True(w.ComponentContainsEntity<Active>(e)); 

        int e2 = EntityFactory.Add(w, setScene: false, setActive: false); 
        Assert.False(w.ComponentContainsEntity<Scene>(e2)); 
        Assert.False(w.ComponentContainsEntity<Active>(e2)); 
    }   
}