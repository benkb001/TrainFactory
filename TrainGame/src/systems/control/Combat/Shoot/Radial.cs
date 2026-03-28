namespace TrainGame.Systems;

using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class RadialShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<RadialShootPattern, U>(w, (w, sp, f, _, e) => {
            int bullets = sp.BulletsPerShot; 
            float speed = sp.Bullet.Speed;
            Vector2 pos = f.Position;

            double radiansPerShot = 2*Math.PI / bullets;
            double offset = sp.OffsetRadians;
            
            for (int i = 0; i < bullets; i++) {
                float dx = (float)(Math.Cos((radiansPerShot * i) + offset) * speed);
                float dy = (float)(Math.Sin((radiansPerShot * i) + offset) * speed);
                int bulletEnt = ShooterWrap.Add<U>(w, pos, pos, sp.Bullet); 
                w.SetComponent<Velocity>(bulletEnt, new Velocity(new Vector2(dx, dy)));
            }

            return bullets; 
        });
    }
}