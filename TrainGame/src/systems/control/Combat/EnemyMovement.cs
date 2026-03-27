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

public static class ChaseMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ChaseMovePattern), typeof(Enemy), typeof(Frame), typeof(Active)], (w, e) => {
            ChaseMovePattern m = w.GetComponent<ChaseMovePattern>(e); 
            Frame enemyFrame = w.GetComponent<Frame>(e);

            (Frame targetFrame, bool hasFrame) = w.GetComponentSafe<Frame>(m.TargetEntity); 

            if (!hasFrame) {
                Console.WriteLine($"removed");
                w.RemoveComponent<ChaseMovePattern>(e); 
                return;
            }

            Vector2 v = Vector2.Normalize(targetFrame.Position - enemyFrame.Position) * m.Speed; 
            w.SetComponent<Velocity>(e, new Velocity(v));
            Console.WriteLine($"set");
        });
    }
}