namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.Systems;

public interface IMovementType {
    IMovementType Clone(); 
}

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

public class ChaseMovePattern : IMovementType {
    public float Speed; 
    public int TargetEntity; 

    public ChaseMovePattern(float Speed) {
        this.Speed = Speed; 
    }

    public IMovementType Clone() => new ChaseMovePattern(Speed);
}

public class MoveTiming {

    public WorldTime CanMove; 
    public WorldTime StopMove; 

    public MoveTiming(WorldTime now) {
        CanMove = now + new WorldTime(ticks: 20 + Util.NextInt(60));
        StopMove = new WorldTime();
    }
}

public static class RegisterDefaultMovementType {
    public static void Register() {
        MovementRegistry.Register<DefaultMovePattern>((w, m, e) => {
            w.SetComponent<DefaultMovePattern>(e, m);
            w.SetComponent<MoveTiming>(e, new MoveTiming(w.Time));
        });
    }
}

public static class RegisterChaseMovementType {
    public static void Register() {
        MovementRegistry.Register<ChaseMovePattern>((w, m, e) => {
            m.TargetEntity = w.GetFirstMatchingEntity([typeof(Targetable), typeof(Frame)]);
            w.SetComponent<ChaseMovePattern>(e, m); 
            Console.WriteLine($"added");
        });
    }
}

public static class RegisterMovementTypes {
    public static void All() {
        RegisterDefaultMovementType.Register();
        RegisterChaseMovementType.Register();
    }
}