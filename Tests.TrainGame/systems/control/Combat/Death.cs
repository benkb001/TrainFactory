using TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class DeathSystemTest {
    [Fact]
    public void DeathSystem_ShouldRemoveActiveEntitiesWithHealthZero() {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Health>(e, new Health(0)); 
        w.Update(); 
        Assert.False(w.EntityExists(e)); 
    }
}