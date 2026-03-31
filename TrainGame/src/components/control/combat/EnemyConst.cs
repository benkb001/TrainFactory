namespace TrainGame.Components;

using System.Collections.Generic;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.ECS;

public class EnemyConst {
    private Shooter shooter; 
    private IMovementType movement;
    private IShootPattern shoot;

    public EnemyType Type; 
    public float Size; 
    public int HP; 
    public int Armor; 
    public int Difficulty;

    public Shooter GetShooter() => shooter.Clone();
    public IMovementType GetMovement() => movement.Clone();
    public IShootPattern GetShootPattern() => shoot.Clone();
    public List<IEnemyTrait> Traits;

    public EnemyConst(Shooter shooter, IShootPattern shoot, IMovementType movement, EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int HP = 5, int Armor = 0, int Difficulty = 1, List<IEnemyTrait> traits = null) {
        
        this.shooter = shooter;
        this.movement = movement; 
        this.shoot = shoot;
        
        this.Type = Type; 
        this.Size = Size; 
        this.HP = HP; 
        this.Armor = Armor;
        this.Difficulty = Difficulty; 

        if (traits == null) {
            traits = new List<IEnemyTrait>();
        }
        
        this.Traits = traits; 
    }
}