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
    public WorldTime WarningDuration; 
    public bool BulletsAreRemovedOnCollision; 
    public int Difficulty;

    public EnemyConst(EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int Damage = 5, int HP = 5, int TicksPerShot = 60, float BulletSpeed = 1.5f, 
        int Ammo = 3, int Skill = 1, ShootPattern SPattern = ShootPattern.Default, 
        BulletType BType = BulletType.Default, OnExpireEffect OExpireEffect = OnExpireEffect.Default,
        int Armor = 0, float PatternSize = 0f, float MoveSpeed = 1f, int TicksBetweenMovement = 120,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int TicksToMove = 60, int BulletsPerShot = 1,
        int ReloadTicks = 120, int BulletSize = Constants.BulletSize, int BulletLifetimeTicks = 120,
        float SpreadDegrees = 10f, bool BulletsAreWarned = false, WorldTime WarningDuration = null,
        bool BulletsAreRemovedOnCollision = true, int Difficulty = 1) {
        
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
        this.WarningDuration = WarningDuration; 
        this.BulletsAreRemovedOnCollision = BulletsAreRemovedOnCollision; 
        this.Difficulty = Difficulty; 
    }
}

public class EnemyWrap {
    public static readonly Dictionary<EnemyType, EnemyConst> Enemies = new() {
        [EnemyType.Artillery] = new EnemyConst(
            Type: EnemyType.Artillery, 
            HP: 15, 
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
            BulletLifetimeTicks: 600,
            Damage: 15,
            Difficulty: 2
        ),
        [EnemyType.Barbarian] = new EnemyConst(
            Type: EnemyType.Barbarian, 
            HP: 30, 
            TicksPerShot: 200, 
            ReloadTicks: 200,
            Ammo: 1, 
            MType: MoveType.Chase, 
            MoveSpeed: Constants.PlayerSpeed / 3f,
            TicksBetweenMovement: 200, 
            TicksToMove: 60, 
            BulletLifetimeTicks: 15,
            BulletSpeed: 0f, 
            BulletsAreWarned: true, 
            WarningDuration: new WorldTime(ticks: 45),
            BulletSize: (int)Constants.TileWidth * 3,
            SPattern: ShootPattern.Melee,
            Damage: 25,
            Difficulty: 2
        ),
        [EnemyType.Default] = new EnemyConst(),
        [EnemyType.MachineGun] = new EnemyConst(
            Type: EnemyType.MachineGun,
            HP: 25,
            Ammo: 36, 
            ReloadTicks: 120,
            MType: MoveType.Chase,
            MoveSpeed: Constants.PlayerSpeed / 2f, 
            TicksBetweenMovement: 0,
            TicksToMove: 10,
            Skill: 90, 
            Damage: 15
        ),
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
        ),
        [EnemyType.Sniper] = new EnemyConst(
            Type: EnemyType.Default, 
            Ammo: 1, 
            ReloadTicks: 300,
            Damage: 40,
            HP: 40, 
            BulletSpeed: 10f, 
            BulletsAreWarned: true, 
            WarningDuration: new WorldTime(ticks: 20),
            BulletLifetimeTicks: 240,
            Skill: 100
        ),
        [EnemyType.Volley] = new EnemyConst(
            Type: EnemyType.Volley, 
            SPattern: ShootPattern.Multi,
            HP: 40, 
            TicksPerShot: 60, 
            ReloadTicks: 200, 
            Ammo: 24, 
            BulletsPerShot: 12, 
            SpreadDegrees: 80f,
            BulletLifetimeTicks: 60,
            BulletSpeed: 3f,
            Damage: 40
        ),
        [EnemyType.Warrior] = new EnemyConst(
            Type: EnemyType.Warrior, 
            SPattern: ShootPattern.Multi, 
            HP: 80,
            ReloadTicks: 480,
            BulletSpeed: 4f,
            WarningDuration: new WorldTime(ticks: 30),
            BulletLifetimeTicks: 240,
            Skill: 100,
            BulletsPerShot: 20,
            Ammo: 20,
            Damage: 60,
            SpreadDegrees: 180f,
            Size: Constants.TileWidth * 2
        )

    };

    public static Type[] EnemySignature = [typeof(Enemy), typeof(Health), typeof(Active)];

    public static EnemyWrap Draw(World w, Vector2 pos, EnemyType enemyType) {
        EnemyConst e = Enemies[enemyType];

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
            spreadDegrees: e.SpreadDegrees,
            WarningDuration: e.WarningDuration
        );
        w.SetComponent<Shooter>(enemyEnt, shooter); 

        w.SetComponent<Enemy>(enemyEnt, new Enemy(enemyType)); 

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

    public static int GetFirst(World w) {
        List<int> es = w.GetMatchingEntities(EnemySignature); 
        return es.Count > 0 ? es[0] : -1; 
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