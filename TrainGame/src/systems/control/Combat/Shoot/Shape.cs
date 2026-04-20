namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class ShapeShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<ShapeShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            Vector2 pos = f.Position; 
            ParametricCurve shape = sp.Shape; 
            int bullets = sp.BulletsPerShot; 
            int dtPerBullet = shape.Range / bullets; 
            Vector2 dv = ShooterWrap.Aim(f.Position, targetPosition, sp.Bullet.Speed);
            shape.T = 0;
            
            for (int i = 0; i < bullets; i++) {
                pos += shape.GetDelta(shape.T + dtPerBullet, shape.T);
                shape.T += dtPerBullet;
                
                int bulletEnt = ShooterWrap.Add<U>(w, pos, targetPosition, sp.Bullet, e);
                w.SetComponent<Velocity>(bulletEnt, new Velocity(dv));
            }

            return bullets;
        });
    }
}