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
                move.Move(w.Time); 
                Frame f = w.GetComponent<Frame>(e); 

                int targetableEnt = TargetableWrap.GetFirst(w);
                (Frame targetFrame, bool success) = w.GetComponentSafe<Frame>(targetableEnt);

                if (!success) {
                    return;
                }

                Vector2 direction = move.Type switch {
                    MoveType.Default => new Vector2(move.Speed * w.NextNeg1To1(), move.Speed * w.NextNeg1To1()),
                    MoveType.Horizontal => new Vector2(move.Speed * (move.PatternIndex == 0 ? -1 : 1), 0f),
                    MoveType.Vertical => new Vector2(0f, move.Speed * (move.PatternIndex == 0 ? -1 : 1)),
                    MoveType.Chase => Vector2.Normalize(targetFrame.Position - f.Position) * move.Speed,
                    _ => throw new InvalidOperationException("Unknown movement type")
                };

                move.SetDirection(direction);
                w.SetComponent<Velocity>(e, new Velocity(direction));
            }

            if (!move.IsMoving(w.Time)) {
                w.SetComponent<Velocity>(e, new Velocity(Vector2.Zero));
            }
        });
    }
}