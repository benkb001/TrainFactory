namespace TrainGame.Components;

using System.Collections.Generic;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.ECS;

public class EnemyConst {
    private Shooter shooter; 
    private IMovementType movement;

    public EnemyType Type; 
    public float Size; 
    public int HP; 
    public int Armor; 
    public int Difficulty;

    public Shooter GetShooter() => shooter.Clone();
    public IMovementType GetMovement() => movement.Clone();

    public EnemyConst(Shooter shooter, IMovementType movement, EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int HP = 5, int Armor = 0, int Difficulty = 1) {
        
        this.shooter = shooter;
        this.movement = movement; 
        
        this.Type = Type; 
        this.Size = Size; 
        this.HP = HP; 
        this.Armor = Armor;
        this.Difficulty = Difficulty; 
    }
}