namespace TrainGame.Systems;

using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class ShotgunShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<ShotgunShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            int bullets = sp.BulletsPerShot; 
            float speed = sp.Bullet.Speed;
            Vector2 pos = f.Position;
            Vector2 delta = targetPosition - pos;
            double targetAngle = Math.Atan2(delta.Y, delta.X); 
            double startAngle = targetAngle - (sp.Radians / 2); 
            double radiansPerShot = sp.Radians / bullets;

            for (int i = 0; i < bullets; i++) {
                float dx = (float)(Math.Cos((radiansPerShot * i) + startAngle) * speed);
                float dy = (float)(Math.Sin((radiansPerShot * i) + startAngle) * speed);
                int bulletEnt = ShooterWrap.Add<U>(w, pos, pos, sp.Bullet, e); 
                w.SetComponent<Velocity>(bulletEnt, new Velocity(new Vector2(dx, dy)));
            }

            return bullets; 
        });
    }
}