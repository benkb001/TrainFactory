namespace TrainGame.Components;

using System.Collections.Generic;
using System.Linq; 

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

    public Shooter GetShooter() => shooter.Clone();
    public IMovementType GetMovement() => movement.Clone();
    public IShootPattern GetShootPattern() => shoot.Clone();
    public List<IEnemyTrait> Traits;
    public int Difficulty; 

    public EnemyConst(Shooter shooter, IShootPattern shoot, IMovementType movement, EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int HP = 5, int Armor = 0, int Difficulty = 1, List<IEnemyTrait> traits = null) {
        
        this.shooter = shooter;
        this.movement = movement; 
        this.shoot = shoot;
        
        this.Type = Type; 
        this.Size = Size; 
        this.HP = HP; 
        this.Armor = Armor;

        if (traits == null) {
            traits = new List<IEnemyTrait>();
        }
        
        this.Traits = traits; 
        this.Difficulty = Difficulty;
    }
}