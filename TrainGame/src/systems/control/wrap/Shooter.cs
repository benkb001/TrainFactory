namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public static class ShooterWrap {
    
    private static float[] ds = {-1f, 1f};

    private static void shootCardinal(World w, int axis, Frame f, Shooter shooter, Func<int> addBulletEnt) {
        V2 pos = new V2(f.Position);
        V2 size = new V2(new Vector2(f.GetWidth(), f.GetHeight()));
        int bullets = shooter.BulletsPerShot;

        float minBulletPos = pos[axis + 1] + (size[axis + 1] / 2) - (shooter.PatternSize / 2f);
        float center = pos[axis] + (size[axis] / 2f);
        float spacePerBullet = shooter.PatternSize / (float)(bullets - 1);

        foreach (int d in ds) {
            for (int i = 0; i < bullets; i++) {
                int bulletEnt = addBulletEnt(); 
                V2 vel = new V2(0f, 0f);
                vel[axis] = d * shooter.GetBulletSpeed();
                w.SetComponent<Velocity>(bulletEnt, new Velocity(vel));
                Frame bulletFrame = w.GetComponent<Frame>(bulletEnt);
                V2 bulletPos = new V2(0f, 0f); 
                bulletPos[axis] = center + (0.5f * size[axis] * d); 
                bulletPos[axis + 1] = minBulletPos + (spacePerBullet * i);
                bulletFrame.SetCoordinates(bulletPos);
            }
        }
    }

    public static void TryShoot(World w, Shooter shooter, Frame f, Vector2 dest, ShooterType type, int targetEnt) {
        if (shooter.CanShoot(w.Time)) {
            Vector2 pos = f.Position; 
            
            List<int> bulletEnts = new();
            float speed = shooter.GetBulletSpeed(); 
            int inaccuracy = shooter.Inaccuracy; 
            int bullets = shooter.BulletsPerShot; 
            Velocity bulletVelocity = new Velocity(Vector2.Zero);
            
            float offset;
            int bulletEnt; 

            int addBulletEnt() {
                int bEnt = EntityFactory.AddUI(w, pos, shooter.BulletSize, 
                    shooter.BulletSize, setOutline: true);
                bulletEnts.Add(bEnt);
                return bEnt;
            }

            switch (shooter.GetShootPattern()) {
                case ShootPattern.Default: 
                    bulletEnt = addBulletEnt();

                    offset = (float)(inaccuracy * w.NextDouble()); 
                    dest += new Vector2(offset, offset); 

                    bulletVelocity = new Velocity(Vector2.Normalize(dest - pos) * speed);
                    w.SetComponent<Velocity>(bulletEnt, bulletVelocity); 
                    break;
                case ShootPattern.Melee: 
                    bulletEnt = addBulletEnt(); 
                    Frame bulletFrame = w.GetComponent<Frame>(bulletEnt);
                    float dx = (bulletFrame.Width - f.Width) / 2f; 
                    float dy = (bulletFrame.Height - f.Height) / 2f; 
                    Vector2 bulletPos = bulletFrame.Position; 
                    bulletFrame.SetCoordinates(bulletPos.X - dx, bulletPos.Y - dy);
                    break;
                case ShootPattern.Multi: 
                    offset = (float)(inaccuracy * w.NextDouble()); 
                    dest += new Vector2(offset, offset); 
                    float spread = shooter.SpreadDegrees;
                    float degreesPerShot = spread / (float)(bullets - (bullets % 2));
                    float startDegree = -1 * degreesPerShot * (bullets / 2);

                    for (int i = 0; i < bullets; i++) {
                        bulletEnt = addBulletEnt();

                        Vector2 v = Vector2.Normalize(dest - pos) * speed; 
                        v = Util.Rotate(v, startDegree + (degreesPerShot * i));
                        w.SetComponent<Velocity>(bulletEnt, new Velocity(v)); 
                    }

                    break;
                case ShootPattern.HorizontalLine: 
                    shootCardinal(w, 0, f, shooter, addBulletEnt);
                    break;
                case ShootPattern.VerticalLine: 
                    shootCardinal(w, 1, f, shooter, addBulletEnt);
                    break;
                default: 
                    throw new InvalidOperationException("Undefined shoot pattern type");
            }
            
            bulletEnts.ForEach(ent => {
                switch (type) {
                    case ShooterType.Enemy: 
                        w.SetComponent<Enemy>(ent, new Enemy()); 
                        break;
                    case ShooterType.Player: 
                        w.SetComponent<Player>(ent, new Player());
                        break;
                    default: 
                        throw new InvalidOperationException("Undefined shooter type");  
                }

                w.SetComponent<Bullet>(ent, shooter.Shoot(w.Time)); 

                switch (shooter.GetBulletType()) {
                    case BulletType.Default: 
                        break; 
                    case BulletType.Homing: 
                        w.SetComponent<Homing>(ent, new Homing(targetEnt));
                        break;
                    default: 
                        throw new InvalidOperationException("Undefined bullet type");
                }

                if (shooter.BulletsAreWarned) {
                    w.RemoveComponent<Active>(ent); 
                    int warnEnt = EntityFactory.Add(w); 
                    Frame bulletFrame = w.GetComponent<Frame>(ent);
                    w.SetComponent<Frame>(warnEnt, new Frame(bulletFrame));
                    bulletFrame.SetCoordinates(SceneSystem.OffScreenPosition);
                    BulletWarning warn = new BulletWarning(w.Time + shooter.WarningDuration, ent);
                    w.SetComponent<BulletWarning>(warnEnt, warn); 
                    w.SetComponent<Outline>(warnEnt, new Outline(Colors.Warning));
                    TextBox tb = new TextBox("!"); 
                    tb.TextColor = Colors.Warning;
                    w.SetComponent<TextBox>(warnEnt, tb);
                }
            });
        }
    }
}