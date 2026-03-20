namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TrainGame.Utils;
using TrainGame.Constants;

public interface IMovePattern {
    Vector2 Move(Vector2 targetPosition);
    int GetTicksTillNextMovement();
    int GetTicksToMove();
    IMovePattern Clone();
}

public class DefaultMovePattern : IMovePattern {
    private float speed; 
    private int ticksToMove; 
    private int ticksToWait;

    public DefaultMovePattern(int ticksToMove = 45, int ticksToWait = 120, float speed = Constants.DefaultEnemySpeed) {
        this.ticksToMove = ticksToMove; 
        this.ticksToWait = ticksToWait; 
        this.speed = speed; 
    }

    public Vector2 Move(Vector2 _) => Vector2.Normalize(new Vector2(Util.NextNeg1To1(), Util.NextNeg1To1())) * speed;
    public int GetTicksTillNextMovement() => ticksToMove + ticksToWait;
    public int GetTicksToMove() => ticksToMove;
    public IMovePattern Clone() => new DefaultMovePattern(ticksToMove, ticksToWait, speed);
}

public class Movement {

    private WorldTime canMove; 
    private WorldTime stopMove;
    private IMovePattern pattern;

    public Movement(IMovePattern pattern) {
        canMove = new WorldTime(); 
        stopMove = new WorldTime();
        this.pattern = pattern.Clone();
    }

    public bool CanMove(WorldTime t) {
        return t.IsAfterOrAt(canMove); 
    }  

    public bool IsMoving(WorldTime t) {
        return !t.IsAfterOrAt(stopMove); 
    }

    public Vector2 Move(WorldTime t, Vector2 targetPosition) {
        if (CanMove(t)) {
            canMove = t + new WorldTime(ticks: pattern.GetTicksTillNextMovement()); 
            stopMove = t + new WorldTime(ticks: pattern.GetTicksToMove());
            return pattern.Move(targetPosition);
        } 
        
        return Vector2.Zero;
    } 

    public Movement Clone() {
        return new Movement(pattern);
    }
}