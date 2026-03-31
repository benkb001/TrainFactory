namespace TrainGame.Systems;

using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class RandomShotgunShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<RandomShotgunShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            int[] bulletCounts = {sp.NumStrongBullets, sp.NumWeakBullets};
            BulletContainer[] bullets = {sp.BulletStrong, sp.BulletWeak};

            Vector2 pos = f.Position;
            Vector2 delta = targetPosition - pos;

            double targetAngle = Math.Atan2(delta.Y, delta.X); 
            double startAngle = targetAngle - (sp.Radians / 2); 
            double endAngle = targetAngle + (sp.Radians / 2);

            for (int j = 0; j < 2; j++) {

                int bulletCount = bulletCounts[j]; 
                BulletContainer bullet = bullets[j];
                float speed = bullet.Speed;

                for (int i = 0; i < bulletCount; i++) {
                    float dx = (float)(Math.Cos(Util.NextBetweenRange(startAngle, endAngle)) * speed);
                    float dy = (float)(Math.Sin(Util.NextBetweenRange(startAngle, endAngle)) * speed);
                    int bulletEnt = ShooterWrap.Add<U>(w, pos, pos, bullet, e); 
                    w.SetComponent<Velocity>(bulletEnt, new Velocity(new Vector2(dx, dy)));
                }
            }

            return sp.NumStrongBullets + sp.NumWeakBullets; 
        });
    }
}