namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TrainGame.Utils;

public class Movement {

    private int patternLength; 
    private int patternIndex;
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
        int patternLength = 1, MoveType Type = MoveType.Default, int patternIndex = 0) {
        canMove = new WorldTime(); 

        this.patternLength = patternLength;
        this.speed = speed; 
        this.ticksToMove = ticksToMove; 
        this.ticksBetweenMovement = ticksBetweenMovement; 
        this.Type = Type;
        this.patternIndex = patternIndex;
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
        canMove = t + new WorldTime(ticks: ticksToMove + ticksBetweenMovement); 
        stopMove = t + new WorldTime(ticks: ticksToMove);
    } 
}