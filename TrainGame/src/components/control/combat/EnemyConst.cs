namespace TrainGame.Components;

using System.Collections.Generic;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.ECS;

public class EnemyConst {
    private Shooter shooter; 

    public EnemyType Type; 
    public float Size; 
    public int HP; 
    public int Armor; 
    public float MoveSpeed; 
    public int TicksBetweenMovement;
    public MoveType MType; 
    public int MovePatternLength;
    public int TicksToMove; 
    public int Difficulty;
    public Shooter GetShooter() => shooter.Clone();

    public EnemyConst(Shooter shooter, EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int HP = 5, int Armor = 0, float MoveSpeed = 1f, int TicksBetweenMovement = 120,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int TicksToMove = 60, int Difficulty = 1) {
        
        this.shooter = shooter;
        this.Type = Type; 
        this.Size = Size; 
        this.HP = HP; 
        this.Armor = Armor;
        this.MoveSpeed = MoveSpeed;
        this.TicksBetweenMovement = TicksBetweenMovement;
        this.MType = MType; 
        this.MovePatternLength = MovePatternLength;
        this.TicksToMove = TicksToMove;
        this.Difficulty = Difficulty; 
    }
}