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
        w.AddSystem([typeof(Enemy), typeof(Shooter), typeof(Frame), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            if (shooter.CanShoot(w.Time)) {
                Vector2 enemyPos = w.GetComponent<Frame>(e).Position; 
                List<int> playerEnts = w.GetMatchingEntities([typeof(Player), typeof(Health), typeof(Frame), typeof(Active)]); 
                Frame enemyFrame = w.GetComponent<Frame>(e);

                foreach (int ent in playerEnts) {
                    Vector2 playerPos = w.GetComponent<Frame>(ent).Position; 
                    List<int> bulletEnts = new();
                    float speed = shooter.GetBulletSpeed(); 
                    int inaccuracy = shooter.Inaccuracy; 
                    int bullets = shooter.BulletsPerShot; 
                    Velocity bulletVelocity = new Velocity(Vector2.Zero);
                    Frame playerFrame = w.GetComponent<Frame>(ent);

                    float offset;
                    int bulletEnt; 
                    float direction; 
                    float spacePerBullet;

                    int addBulletEnt() {
                        int bEnt = EntityFactory.AddUI(w, enemyPos, shooter.BulletSize, 
                            shooter.BulletSize, setOutline: true);
                        bulletEnts.Add(bEnt);
                        return bEnt;
                    }

                    switch (shooter.GetShootPattern()) {
                        case ShootPattern.Default: 
                            bulletEnt = addBulletEnt();

                            offset = (float)(inaccuracy * w.NextDouble()); 
                            playerPos += new Vector2(offset, offset); 

                            bulletVelocity = new Velocity(Vector2.Normalize(playerPos - enemyPos) * speed);
                            w.SetComponent<Velocity>(bulletEnt, bulletVelocity); 
                            break;
                        case ShootPattern.Multi: 
                            offset = (float)(inaccuracy * w.NextDouble()); 
                            playerPos += new Vector2(offset, offset); 
                            float spread = shooter.SpreadDegrees;
                            float degreesPerShot = spread / (float)(bullets - (bullets % 2));
                            float startDegree = -1 * degreesPerShot * (bullets / 2);

                            for (int i = 0; i < bullets; i++) {
                                bulletEnt = addBulletEnt();

                                Vector2 v = Vector2.Normalize(playerPos - enemyPos) * speed; 
                                v = Util.Rotate(v, startDegree + (degreesPerShot * i));
                                w.SetComponent<Velocity>(bulletEnt, new Velocity(v)); 
                            }

                            break;
                        case ShootPattern.HorizontalLine: 
                            direction = playerFrame.Position.X > enemyFrame.Position.X ? 1f : -1f; 
                            float topBulletPos = enemyFrame.Position.Y - (shooter.PatternSize / 2f);
                            spacePerBullet = shooter.PatternSize / (float)(bullets - 1);

                            for (int i = 0; i < bullets; i++) {
                                bulletEnt = addBulletEnt(); 
                                Vector2 v = new Vector2(direction * speed, 0f);
                                w.SetComponent<Velocity>(bulletEnt, new Velocity(v));
                                Frame f = w.GetComponent<Frame>(bulletEnt);
                                f.SetCoordinates(f.Position.X, topBulletPos + (spacePerBullet * i));
                            }

                            break;
                        case ShootPattern.VerticalLine: 
                            direction = playerFrame.Position.Y > enemyFrame.Position.Y ? 1f : -1f; 
                            float leftBulletPos = enemyFrame.Position.X - (shooter.PatternSize / 2f);
                            spacePerBullet = shooter.PatternSize / (float)(bullets - 1);

                            for (int i = 0; i < bullets; i++) {
                                bulletEnt = addBulletEnt(); 
                                Vector2 v = new Vector2(0f, direction * speed);
                                w.SetComponent<Velocity>(bulletEnt, new Velocity(v));
                                Frame f = w.GetComponent<Frame>(bulletEnt);
                                f.SetCoordinates(leftBulletPos + (spacePerBullet * i), f.Position.Y);
                            }
                            
                            break;
                        default: 
                            throw new InvalidOperationException("Undefined shoot pattern type");
                    }

                    
                    bulletEnts.ForEach(ent => {
                        w.SetComponent<Enemy>(ent, new Enemy()); 
                        w.SetComponent<Bullet>(ent, shooter.Shoot(w.Time)); 

                        switch (shooter.GetBulletType()) {
                            case BulletType.Default: 
                                break; 
                            case BulletType.Homing: 
                                w.SetComponent<Homing>(ent, new Homing(playerEnts[0]));
                                break;
                            default: 
                                throw new InvalidOperationException("Undefined bullet type");
                        }
                    });
                }
            }
        });
    }
}