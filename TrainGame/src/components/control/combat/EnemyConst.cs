namespace TrainGame.Components;

using System.Collections.Generic;

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.ECS;

public class EnemyConst {
    private IShootPattern shootPattern;

    public EnemyType Type; 
    public float Size; 
    public int Damage; 
    public int HP; 
    public int TicksPerShot; 
    public float BulletSpeed; 
    public int Ammo; 
    public int Skill; 
    public BulletType BType; 
    public OnExpireEffect OExpireEffect; 
    public int Armor; 
    public float PatternSize;
    public float MoveSpeed; 
    public int TicksBetweenMovement;
    public MoveType MType; 
    public int MovePatternLength;
    public int BulletsPerShot; 
    public int ReloadTicks;
    public int TicksToMove; 
    public int BulletSize; 
    public int BulletLifetimeTicks;
    public float SpreadDegrees;
    public WorldTime WarningDuration; 
    public bool BulletsAreRemovedOnCollision; 
    public int Difficulty;

    IShootPattern ShootPattern => shootPattern.Clone();

    public EnemyConst(IShootPattern shootPattern, EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int Damage = 5, int HP = 5, int TicksPerShot = 60, float BulletSpeed = 1.5f, 
        int Ammo = 3, int Skill = 1, 
        BulletType BType = BulletType.Default, OnExpireEffect OExpireEffect = OnExpireEffect.Default,
        int Armor = 0, float PatternSize = 0f, float MoveSpeed = 1f, int TicksBetweenMovement = 120,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int TicksToMove = 60, int BulletsPerShot = 1,
        int ReloadTicks = 120, int BulletSize = Constants.BulletSize, int BulletLifetimeTicks = 120,
        float SpreadDegrees = 10f, bool BulletsAreWarned = false, WorldTime WarningDuration = null,
        bool BulletsAreRemovedOnCollision = true, int Difficulty = 1) {
        
        this.shootPattern = shootPattern;
        this.Type = Type; 
        this.Size = Size; 
        this.Damage = Damage; 
        this.HP = HP; 
        this.TicksPerShot = TicksPerShot; 
        this.BulletSpeed = BulletSpeed; 
        this.Ammo = Ammo; 
        this.Skill = Skill; 
        this.BType = BType; 
        this.OExpireEffect = OExpireEffect;
        this.Armor = Armor;
        this.PatternSize = PatternSize;
        this.MoveSpeed = MoveSpeed;
        this.TicksBetweenMovement = TicksBetweenMovement;
        this.MType = MType; 
        this.MovePatternLength = MovePatternLength;
        this.BulletsPerShot = BulletsPerShot;
        this.ReloadTicks = ReloadTicks;
        this.TicksToMove = TicksToMove;
        this.BulletSize = BulletSize;
        this.BulletLifetimeTicks = BulletLifetimeTicks;
        this.SpreadDegrees = SpreadDegrees;
        this.WarningDuration = WarningDuration; 
        this.BulletsAreRemovedOnCollision = BulletsAreRemovedOnCollision; 
        this.Difficulty = Difficulty; 
    }
}