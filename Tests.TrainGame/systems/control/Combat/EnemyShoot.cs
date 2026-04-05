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

public class EnemyShootSystemTest {
    [Fact]
    public void EnemyShootSystem_ShouldCreateABulletForEachShooterEnemy() {
        World w = WorldFactory.Build(); 
        int num_enemies = 3; 
        for (int i = 0; i < num_enemies; i++) {
            int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10);
            w.SetComponent<Enemy>(e, new Enemy()); 
            w.SetComponent<Shooter>(e, new Shooter());
            w.SetComponent<DefaultShootPattern>(e, new DefaultShootPattern(new BulletContainer(new Bullet(1))));
        }

        int targetEnt = EntityFactory.AddUI(w, Vector2.Zero, 10, 10);
        w.SetComponent<Targetable>(targetEnt, new Targetable());
        w.SetComponent<Player>(targetEnt, new Player());

        w.Update(); 

        Assert.Equal(3, w.GetMatchingEntities([typeof(Bullet), typeof(Velocity), typeof(Enemy), typeof(Active)]).Count); 
    }

    //TODO: Write 
    //Enemy bullets should roughly be pointing towards the player
}