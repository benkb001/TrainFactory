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

public enum MoveType {
    Default,
    Horizontal,
    Vertical,
    Chase
}

public class Movement {

    private int patternLength; 
    private int patternIndex = 0;
    private float speed; 
    private int ticksToMove; 
    private int ticksBetweenMovement; 
    private Vector2 direction; 
    private WorldTime canMove; 
    private WorldTime stopMove;
    public readonly MoveType Type;

    public float Speed => speed; 
    public Vector2 Direction => direction; 
    public int PatternIndex => patternIndex;

    public Movement(float speed = 2f, int ticksToMove = 120, int ticksBetweenMovement = 300,
        int patternLength = 1, MoveType Type = MoveType.Default) {
        canMove = new WorldTime(); 

        this.patternLength = patternLength;
        this.speed = speed; 
        this.ticksToMove = ticksToMove; 
        this.ticksBetweenMovement = ticksBetweenMovement; 
        this.Type = Type;
    }

    public bool CanMove(WorldTime t) {
        return t.IsAfterOrAt(canMove); 
    }  

    public bool IsMoving(WorldTime t) {
        return !t.IsAfterOrAt(stopMove); 
    }

    public void SetDirection(Vector2 d) {
        direction = d; 
        patternIndex = (patternIndex + 1) % patternLength;
    }

    public void Move(WorldTime t) {
        canMove = t + new WorldTime(ticks: ticksBetweenMovement); 
        stopMove = t + new WorldTime(ticks: ticksToMove);
    } 
}

public static class EnemyMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Movement), typeof(Enemy), typeof(Frame), typeof(Active)], (w, e) => {

            Movement move = w.GetComponent<Movement>(e); 
            if (move.CanMove(w.Time)) {
                move.Move(w.Time); 
                Frame f = w.GetComponent<Frame>(e); 
                Frame playerFrame = w.GetComponent<Frame>(PlayerWrap.GetRPGEntity(w)); 
                Vector2 direction = move.Type switch {
                    MoveType.Default => new Vector2(move.Speed * w.NextNeg1To1(), move.Speed * w.NextNeg1To1()),
                    MoveType.Horizontal => new Vector2(move.Speed * (move.PatternIndex == 0 ? -1 : 1), 0f),
                    MoveType.Vertical => new Vector2(0f, move.Speed * (move.PatternIndex == 0 ? -1 : 1)),
                    MoveType.Chase => Vector2.Normalize(playerFrame.Position - f.Position) * move.Speed,
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