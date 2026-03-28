namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.Systems;

public class DefaultMovePattern : IMovementType {

    public float Speed; 
    public WorldTime TimeToMove; 
    public WorldTime TimeToWait; 

    public DefaultMovePattern(int ticksToMove = 45, int ticksToWait = 120, float speed = Constants.DefaultEnemySpeed) {
        this.Speed = speed; 
        this.TimeToMove = new WorldTime(ticks: ticksToMove); 
        this.TimeToWait = new WorldTime(ticks: ticksToWait); 
    }

    public IMovementType Clone() => new DefaultMovePattern(TimeToMove.InTicks(), TimeToWait.InTicks(), Speed);
}