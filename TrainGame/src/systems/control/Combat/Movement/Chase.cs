namespace TrainGame.Systems;

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class ChaseMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ChaseMovePattern), typeof(Enemy), typeof(Frame), typeof(Active)], (w, e) => {
            ChaseMovePattern m = w.GetComponent<ChaseMovePattern>(e); 
            Frame enemyFrame = w.GetComponent<Frame>(e);

            int targetableEnt = w.GetFirstMatchingEntity([typeof(Targetable), typeof(Active), typeof(Frame)]);
            (Frame targetFrame, bool hasFrame) = w.GetComponentSafe<Frame>(targetableEnt); 

            if (hasFrame) {
                Vector2 v = Vector2.Normalize(targetFrame.Position - enemyFrame.Position) * m.Speed; 
                w.SetComponent<Velocity>(e, new Velocity(v));
            }
        });
    }
}