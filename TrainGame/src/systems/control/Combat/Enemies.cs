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
    Artillery, //Big, Shoots vertically, homing bullets
    Default,
    Ninja, //Dashes around, shoots occasionally
    Robot, //moves left to right and shoots up/down in bursts
    Shotgun
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
    public int BulletSize; 
    public int BulletLifetimeTicks;
    public float SpreadDegrees;

    public EnemyConst(EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int Damage = 1, int HP = 5, int TicksPerShot = 60, float BulletSpeed = 1.5f, 
        int Ammo = 3, int Skill = 1, ShootPattern SPattern = ShootPattern.Default, 
        BulletType BType = BulletType.Default, OnExpireEffect OExpireEffect = OnExpireEffect.Default,
        int Armor = 0, float PatternSize = 0f, float MoveSpeed = 1f, int TicksBetweenMovement = 120,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int TicksToMove = 60, int BulletsPerShot = 1,
        int ReloadTicks = 120, int BulletSize = Constants.BulletSize, int BulletLifetimeTicks = 120,
        float SpreadDegrees = 10f) {
        
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
        this.BulletSize = BulletSize;
        this.BulletLifetimeTicks = BulletLifetimeTicks;
        this.SpreadDegrees = SpreadDegrees;
    }
}

public class EnemyWrap {
    private static Dictionary<EnemyType, EnemyConst> enemies = new() {
        [EnemyType.Artillery] = new EnemyConst(
            Type: EnemyType.Artillery, 
            HP: 10, 
            TicksPerShot: 300, 
            ReloadTicks: 100, 
            BulletSpeed: 2f, 
            Ammo: 4, 
            SPattern: ShootPattern.VerticalLine, 
            BulletsPerShot: 2, 
            PatternSize: Constants.TileWidth * 2, 
            Size: Constants.TileWidth * 2, 
            MoveSpeed: 0.5f, 
            TicksBetweenMovement: 60, 
            TicksToMove: 60, 
            MType: MoveType.Chase,
            BType: BulletType.Homing,
            BulletSize: Constants.BulletSize * 2,
            BulletLifetimeTicks: 600
        ),
        [EnemyType.Default] = new EnemyConst(),
        [EnemyType.Ninja] = new EnemyConst(
            Type: EnemyType.Ninja, 
            HP: 6, 
            TicksPerShot: 2, 
            Ammo: 2, 
            BulletsPerShot: 1, 
            ReloadTicks: 120, 
            MType: MoveType.Chase, 
            MoveSpeed: Constants.PlayerSpeed / 1.5f, 
            TicksBetweenMovement: 15,
            TicksToMove: 20,
            Skill: 90
        ),
        [EnemyType.Robot] = new EnemyConst(
            Type: EnemyType.Robot, 
            HP: 8, 
            TicksPerShot: 2,
            BulletSpeed: 6f, 
            Ammo: 32,
            SPattern: ShootPattern.VerticalLine,
            BulletsPerShot: 2, 
            TicksBetweenMovement: 60, 
            PatternSize: Constants.TileWidth,
            TicksToMove: 360,
            MType: MoveType.Horizontal,
            MovePatternLength: 2,
            ReloadTicks: 60
        ),
        [EnemyType.Shotgun] = new EnemyConst(
            Type: EnemyType.Shotgun, 
            SPattern: ShootPattern.Multi,
            HP: 8, 
            TicksPerShot: 120, 
            ReloadTicks: 240, 
            Ammo: 12, 
            BulletsPerShot: 4, 
            SpreadDegrees: 40f,
            BulletLifetimeTicks: 180,
            BulletSpeed: 1.5f
        )
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
            reloadTicks: e.ReloadTicks,
            bulletType: e.BType, 
            bulletSize: e.BulletSize,
            bulletLifetimeTicks: e.BulletLifetimeTicks,
            spreadDegrees: e.SpreadDegrees
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