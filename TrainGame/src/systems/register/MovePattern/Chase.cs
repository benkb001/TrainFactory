namespace TrainGame.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class RegisterChaseMovementType {
    public static void Register() {
        MovementRegistry.Register<ChaseMovePattern>((w, m, e) => {
            int targetEnt = TargetableWrap.GetFirst(w); 
            (Frame enemyFrame, bool enemyHasFrame) = w.GetComponentSafe<Frame>(e); 
            (Frame targetFrame, bool targetHasFrame) = w.GetComponentSafe<Frame>(targetEnt); 

            if (enemyHasFrame && targetHasFrame) {
                w.SetComponent<Homing>(e, new Homing(targetEnt, m.Speed)); 
                Vector2 v = Vector2.Normalize(targetFrame.Position - enemyFrame.Position) * m.Speed; 
                w.SetComponent<Velocity>(e, new Velocity(v));
            }
        });
    }
}