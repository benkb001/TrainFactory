namespace TrainGame.Systems; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class DefaultEnemyMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(MoveTiming), typeof(DefaultMovePattern), 
            typeof(Enemy), typeof(Frame), typeof(Active)], (w, e) => {
            
            MoveTiming timing = w.GetComponent<MoveTiming>(e); 
            DefaultMovePattern m = w.GetComponent<DefaultMovePattern>(e); 

            if (w.Time.IsAfterOrAt(timing.CanMove)) {
                Vector2 v = new Vector2(m.Speed * Util.NextNeg1To1(), m.Speed * Util.NextNeg1To1());
                w.SetComponent<Velocity>(e, new Velocity(v));
                timing.StopMove = w.Time + m.TimeToMove; 
                timing.CanMove = w.Time + m.TimeToMove + m.TimeToWait;
            }

            if (w.Time.IsAfterOrAt(timing.StopMove)) {
                w.SetComponent<Velocity>(e, new Velocity(Vector2.Zero));
            }
        });
    }
}