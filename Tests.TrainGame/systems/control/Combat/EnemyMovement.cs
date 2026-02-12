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

public class EnemyMovementSystemTest {
    [Fact]
    public void EnemyMovementSystem_ShouldSetANonZeroVelocityToEnemyEntity() {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10);

        PlayerWrap.AddTest(w);

        w.SetComponent<Enemy>(e, new Enemy()); 
        w.SetComponent<Movement>(e, new Movement()); 
        w.Update(); 
        Assert.True(w.ComponentContainsEntity<Velocity>(e));
        Assert.NotEqual(Vector2.Zero, w.GetComponent<Velocity>(e).Vector);
    }
}