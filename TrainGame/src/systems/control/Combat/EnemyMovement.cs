namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class TargetableWrap {
    public static int GetFirst(World w) {
        return w.GetFirstMatchingEntity([typeof(Frame), typeof(Targetable), typeof(Active)]);
    }
}

public static class EnemyMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Movement), typeof(Enemy), typeof(Frame), typeof(Active)], (w, e) => {

            Movement move = w.GetComponent<Movement>(e); 
            
            if (move.CanMove(w.Time)) {

                Frame f = w.GetComponent<Frame>(e); 

                int targetableEnt = TargetableWrap.GetFirst(w);
                (Frame targetFrame, bool success) = w.GetComponentSafe<Frame>(targetableEnt);

                if (!success) {
                    return;
                }

                Vector2 v = move.Move(w.Time, targetFrame.Position); 
                w.SetComponent<Velocity>(e, new Velocity(v));
            }

            if (!move.IsMoving(w.Time)) {
                w.SetComponent<Velocity>(e, new Velocity(Vector2.Zero));
            }
        });
    }
}