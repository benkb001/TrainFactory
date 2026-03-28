namespace TrainGame.Systems; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class ShootSystem {
    //tf should return the number of bullets shot
    public static void Register<T, U>(World w, Func<World, T, Frame, Vector2, int, int> tf) 
    where U : IFlag<U> 
    where T : IShootPattern {
        w.AddSystem([typeof(T), typeof(U), typeof(ShotMessage), typeof(Shooter), 
                     typeof(Frame), typeof(Active)], (w, e) => {

                    
            Frame f = w.GetComponent<Frame>(e); 
            T t = w.GetComponent<T>(e);
            Vector2 targetPosition = w.GetComponent<ShotMessage>(e).TargetPosition; 

            int bulletsShot = tf(w, t, f, targetPosition, e);

            Shooter shooter = w.GetComponent<Shooter>(e); 
            shooter.Update(w.Time, bulletsShot);
        });
    }
}