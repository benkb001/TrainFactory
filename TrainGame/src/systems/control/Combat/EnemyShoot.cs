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
    private static float[] ds = {-1f, 1f};

    private static void shootCardinal(World w, int axis, Frame enemyFrame, Shooter shooter, Func<int> addBulletEnt) {
        V2 enemyPosition = new V2(enemyFrame.Position);
        V2 enemySize = new V2(new Vector2(enemyFrame.GetWidth(), enemyFrame.GetHeight()));
        int bullets = shooter.BulletsPerShot;

        float minBulletPos = enemyPosition[axis + 1] + (enemySize[axis + 1] / 2) - 
            (shooter.PatternSize / 2f);
        float enemyCenter = enemyPosition[axis] + (enemySize[axis] / 2f);
        float spacePerBullet = shooter.PatternSize / (float)(bullets - 1);

        foreach (int d in ds) {
            for (int i = 0; i < bullets; i++) {
                int bulletEnt = addBulletEnt(); 
                V2 vel = new V2(0f, 0f);
                vel[axis] = d * shooter.GetBulletSpeed();
                w.SetComponent<Velocity>(bulletEnt, new Velocity(vel));
                Frame f = w.GetComponent<Frame>(bulletEnt);
                V2 pos = new V2(0f, 0f); 
                pos[axis] = enemyCenter + (0.5f * enemySize[axis] * d); 
                pos[axis + 1] = minBulletPos + (spacePerBullet * i);
                f.SetCoordinates(pos);
            }
        }
    }

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
                            shootCardinal(w, 0, enemyFrame, shooter, addBulletEnt);
                            break;
                        case ShootPattern.VerticalLine: 
                            shootCardinal(w, 1, enemyFrame, shooter, addBulletEnt);
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