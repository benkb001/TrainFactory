namespace TrainGame.Systems;

using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils; 

public static class GridShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<GridShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            //ICKY: If camera becomes more dynamic this could become abusable
            Vector2 topleft = w.GetCameraTopLeft() + (sp.Direction * sp.PatternIndex); 
            sp.PatternIndex++; 
            BulletContainer bcx = sp.BulletX; 
            BulletContainer bcy = sp.BulletY; 
            float dx = sp.Dx; 
            float dy = sp.Dy; 
            float[] deltas = {dx, dy}; 
            int[] bulletCounts = {sp.NumBulletsX, sp.NumBulletsY}; 
            BulletContainer[] bcs = {bcx, bcy};

            for (int axis = 0; axis < 2; axis++) {

                float delta = deltas[axis];
                int numBullets = bulletCounts[axis];

                for(int i = 0; i < numBullets; i++) {
                    V2 dPosition = new V2();
                    dPosition[axis] = delta * i; 
                    Vector2 pos = topleft + dPosition;
                    int bEnt = ShooterWrap.Add<U>(w, pos, pos, bcs[axis], e); 
                    w.SetComponent<Velocity>(bEnt, new Velocity(Vector2.Zero));
                }
            }

            return sp.NumBulletsX + sp.NumBulletsY; 
        });
    }
}