namespace TrainGame.Systems; 

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

public static class EnemyShootSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Enemy), typeof(Shooter), typeof(Health), typeof(Frame), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            if (shooter.CanShoot(w.Time)) {
                Vector2 enemyPos = w.GetComponent<Frame>(e).Position; 
                List<int> playerEnts = w.GetMatchingEntities([typeof(Player), typeof(Health), typeof(Frame), typeof(Active)]); 
                foreach (int ent in playerEnts) {
                    Vector2 playerPos = w.GetComponent<Frame>(ent).Position; 
                    int bulletEnt = EntityFactory.AddUI(w, enemyPos, Constants.BulletSize, 
                        Constants.BulletSize, setOutline: true);
                    w.SetComponent<Enemy>(bulletEnt, new Enemy()); 

                    float speed = shooter.GetBulletSpeed(); 
                    w.SetComponent<Bullet>(bulletEnt, shooter.Shoot(w.Time)); 
                    Velocity bulletVelocity = new Velocity(Vector2.Normalize(playerPos - enemyPos) * speed);
                    w.SetComponent<Velocity>(bulletEnt, bulletVelocity); 
                }
            }
        });
    }
}