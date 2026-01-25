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

public class Movement {

    private float speed; 
    private int ticksToMove; 
    private int ticksBetweenMovement; 
    private float direction; 
    private WorldTime canMove; 
    private WorldTime stopMove;

    public float Speed => speed; 
    public float Direction => direction; 

    public Movement(float speed = 2f, int ticksToMove = 120, int ticksBetweenMovement = 300) {
        canMove = new WorldTime(); 

        this.speed = speed; 
        this.ticksToMove = ticksToMove; 
        this.ticksBetweenMovement = ticksBetweenMovement; 
    }

    public bool CanMove(WorldTime t) {
        return t.IsAfterOrAt(canMove); 
    }  

    public bool IsMoving(WorldTime t) {
        return !t.IsAfterOrAt(stopMove); 
    }

    public void SetDirection(float d) {
        direction = d; 
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
                float direction = (float)(move.Speed * w.NextDouble()); 
                move.SetDirection(direction);
            }

            Velocity v; 

            if (move.IsMoving(w.Time)) {
                float direction = move.Direction; 
                v = new Velocity(Vector2.Normalize(new Vector2(direction, direction)));
            } else {
                v = new Velocity(Vector2.Zero); 
            }

            w.SetComponent<Velocity>(e, v); 
        });
    }
}