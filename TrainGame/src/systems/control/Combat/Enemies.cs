namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public enum EnemyType {
    Default,
    Robot //moves left to right and shoots up/down in bursts
}

public class EnemyConst {
    public EnemyType Type; 
    public float Size; 
    public int Damage; 
    public int HP; 
    public int TicksPerShot; 
    public float BulletSpeed; 
    public int Ammo; 
    public int Skill; 
    public ShootPattern SPattern; 
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

    public EnemyConst(EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int Damage = 1, int HP = 5, int TicksPerShot = 10, float BulletSpeed = 2f, 
        int Ammo = 8, int Skill = 1, ShootPattern SPattern = ShootPattern.Default, 
        BulletType BType = BulletType.Default, OnExpireEffect OExpireEffect = OnExpireEffect.Default,
        int Armor = 0, float PatternSize = 0f, float MoveSpeed = 1f, int TicksBetweenMovement = 0,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int TicksToMove = 120, int BulletsPerShot = 1,
        int ReloadTicks = 30) {
        
        this.Type = Type; 
        this.Size = Size; 
        this.Damage = Damage; 
        this.HP = HP; 
        this.TicksPerShot = TicksPerShot; 
        this.BulletSpeed = BulletSpeed; 
        this.Ammo = Ammo; 
        this.Skill = Skill; 
        this.SPattern = SPattern; 
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
    }
}

public class EnemyWrap {
    private static Dictionary<EnemyType, EnemyConst> enemies = new() {
        [EnemyType.Default] = new EnemyConst(),
        [EnemyType.Robot] = new EnemyConst(
            Type: EnemyType.Robot, 
            HP: 8, 
            TicksPerShot: 2,
            BulletSpeed: 6f, 
            Ammo: 32,
            SPattern: ShootPattern.VerticalLine,
            BulletsPerShot: 2, 
            TicksBetweenMovement: 60, 
            PatternSize: 20f,
            TicksToMove: 360,
            MType: MoveType.Horizontal,
            MovePatternLength: 2,
            ReloadTicks: 60
        ),
    };

    public static Type[] EnemySignature = [typeof(Enemy), typeof(Health), typeof(Active)];

    public static EnemyWrap Draw(World w, Vector2 pos, EnemyType enemyType) {
        EnemyConst e = enemies[enemyType];

        int enemyEnt = EntityFactory.AddUI(w, pos, e.Size, e.Size, setOutline: true); 
        Health h = new Health(e.HP);
        w.SetComponent<Health>(enemyEnt, h); 
        
        Shooter shooter = new Shooter(
            bulletDamage: e.Damage,
            ticksPerShot: e.TicksPerShot,
            bulletSpeed: e.BulletSpeed,
            ammo: e.Ammo,
            skill: e.Skill,
            shootPattern: e.SPattern,
            bulletsPerShot: e.BulletsPerShot,
            patternSize: e.PatternSize,
            reloadTicks: e.ReloadTicks
        );
        w.SetComponent<Shooter>(enemyEnt, shooter); 

        w.SetComponent<Enemy>(enemyEnt, new Enemy()); 

        Movement movement = new Movement(
            speed: e.MoveSpeed,
            ticksBetweenMovement: e.TicksBetweenMovement,
            Type: e.MType,
            patternLength: e.MovePatternLength,
            ticksToMove: e.TicksToMove
        );
        w.SetComponent<Movement>(enemyEnt, movement); 

        w.SetComponent<Collidable>(enemyEnt, new Collidable()); 
        Armor armor = new Armor(e.Armor);
        w.SetComponent<Armor>(enemyEnt, armor); 
        
        return new EnemyWrap(enemyEnt, h, armor, movement, shooter);
    }

    private Armor armor; 
    private Movement movement; 
    private Shooter shooter; 
    private Health health; 
    private int e; 

    public Health GetHealth() => health; 
    public int Entity => e; 
    public Armor GetArmor() => armor; 
    public Movement GetMovement() => movement; 
    public Shooter GetShooter() => shooter; 

    private EnemyWrap(int e, Health health, Armor armor, Movement movement, Shooter shooter) {
        this.e = e; 
        this.armor = armor; 
        this.movement = movement; 
        this.shooter = shooter; 
        this.health = health; 
    }
}