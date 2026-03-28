namespace TrainGame.Systems;

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class CyclicalMoveSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(CyclicalMovePattern), typeof(Enemy), typeof(Frame), 
                    typeof(Active), typeof(MoveTiming)], (w, e) => {
            
            MoveTiming mt = w.GetComponent<MoveTiming>(e);
            CyclicalMovePattern m = w.GetComponent<CyclicalMovePattern>(e); 
            Frame f = w.GetComponent<Frame>(e); 

            if (w.Time.IsAt(mt.StopMove)) {
                w.SetComponent<Velocity>(e, new Velocity(Vector2.Zero));
            } else if (w.Time.IsAfterOrAt(mt.CanMove)) {
                int i = m.MoveIndex; 

                Vector2 v = m.Directions[i] * m.Speed; 
                w.SetComponent<Velocity>(e, new Velocity(v));

                mt.Update(w.Time, m.TimeToMove, m.WaitTimes[i]);

                m.MoveIndex++; 
            }
        });
    }
}