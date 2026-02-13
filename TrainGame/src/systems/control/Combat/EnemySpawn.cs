namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public enum CombatState {
    Fighting, 
    Reward,
    Cooldown
}

public class CombatReward {
    public bool Active; 
    public CombatReward() {
        Active = true; 
    }
}

public class CombatRewardCollectedMessage {}

public class EnemySpawner {
    private List<Health> hs = new(); 
    private List<CombatReward> rewards = new(); 
    private int round = 0;
    private WorldTime between_rounds = new WorldTime(minutes: 5); 
    private WorldTime next_round = new WorldTime(); 
    private CombatState state = CombatState.Cooldown;

    public int Round => round; 

    public bool CanSpawn(WorldTime now) {
        return state == CombatState.Cooldown && now.IsAfterOrAt(next_round); 
    }

    public void Update(WorldTime now) {
        if (state == CombatState.Fighting) {
            if (hs.Where(h => h.HP <= 0).ToList().Count == hs.Count) {
                hs.Clear(); 
                rewards.Clear();
                state = CombatState.Reward;
            }
        } else if (state == CombatState.Reward) {
            if (rewards.Count > 0 && rewards.Where(r => !r.Active).ToList().Count == rewards.Count) {
                next_round = now + between_rounds; 
                state = CombatState.Cooldown; 
            }
        } else if (state == CombatState.Cooldown) {
            if (hs.Count > 0) {
                state = CombatState.Fighting; 
            }
        }
    }

    public bool CanReward() {
        return state == CombatState.Reward && rewards.Count == 0; 
    }

    public void FinishRound() {
        hs.Clear(); 
        round++;
    } 

    public void Spawn(Health h) {
        hs.Add(h); 
    }

    public void Spawn(EnemyWrap ew) {
        Spawn(ew.GetHealth());
    }

    public void AddReward(CombatReward reward) {
        rewards.Add(reward); 
    }
}

public class EnemyConst {
    public EnemyType Type; 
    public float Size; 
    public int Damage; 
    public int HP; 
    public int TicksPerShot; 
    public int BulletSpeed; 
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

    public EnemyConst(EnemyType Type = EnemyType.Default, float Size = Constants.EnemySize, 
        int Damage = 1, int HP = 5, int TicksPerShot = 10, int BulletSpeed = 2, 
        int Ammo = 8, int Skill = 1, ShootPattern SPattern = ShootPattern.Default, 
        BulletType BType = BulletType.Default, OnExpireEffect OExpireEffect = OnExpireEffect.Default,
        int Armor = 0, float PatternSize = 0f, float MoveSpeed = 1f, int TicksBetweenMovement = 0,
        MoveType MType = MoveType.Default, int MovePatternLength = 1, int BulletsPerShot = 1) {
        
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
    }

}

public class EnemyWrap {
    private static Dictionary<EnemyType, EnemyConst> enemies = new() {
        [EnemyType.Default] = new EnemyConst()
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
            patternSize: e.PatternSize
        );
        w.SetComponent<Shooter>(enemyEnt, shooter); 

        w.SetComponent<Enemy>(enemyEnt, new Enemy()); 

        Movement movement = new Movement(
            speed: e.MoveSpeed,
            ticksBetweenMovement: e.TicksBetweenMovement,
            Type: e.MType,
            patternLength: e.MovePatternLength
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

class Ladder {}

public static class EnemySpawnSystem {
    private const float armorThresh = 0.05f; 
    private const float damageThresh = 0.01f; 
    private const float healthThresh = 0.5f; 
    private const float timeCrystalThresh = 1f;
    private const int numRewards = 2;

    public static void Register(World w) {
        Vector2 dx(int i) {
            return new Vector2((i * 110f) + 10f, 10f); 
        }

        w.AddSystem([typeof(EnemySpawner), typeof(Frame), typeof(Active)], (w, e) => {
            EnemySpawner spawner = w.GetComponent<EnemySpawner>(e); 
            Frame f = w.GetComponent<Frame>(e); 
            spawner.Update(w.Time); 
            int round = spawner.Round; 

            if (spawner.CanReward()) {

                for (int i = 0; i < numRewards; i++) {
                    Vector2 pos = f.Position + dx(i);

                    int rewardEnt = EntityFactory.AddUI(w, pos, Constants.TileWidth, 
                        Constants.TileWidth, setOutline: true, setInteractable: true);

                    CombatReward reward = new CombatReward(); 
                    w.SetComponent<CombatReward>(rewardEnt, reward); 
                    spawner.AddReward(reward); 

                    string rewardStr = ""; 
                    float rand = w.NextFloat(); 

                    if (rand < armorThresh) {
                        rewardStr = "Armor +1"; 
                        w.SetComponent<TempArmor>(rewardEnt, new TempArmor(1)); 
                    } else if (rand < damageThresh) {
                        rewardStr = "Damage +1"; 
                        w.SetComponent<DamagePotion>(rewardEnt, new DamagePotion(1)); 
                    } else if (rand < healthThresh) {
                        int hp = round; 
                        hp = 1 + (int)(w.NextFloat() * w.NextFloat() * hp); 
                        rewardStr = $"HP +{hp}";
                        w.SetComponent<HealthPotion>(rewardEnt, new HealthPotion(hp)); 
                    } else if (rand < timeCrystalThresh) {
                        int timeCrystals = 10 * round; 
                        timeCrystals = 1 + (int)(w.NextFloat() * 2 * timeCrystals); 
                        rewardStr = $"{timeCrystals} Time Crystals";
                        w.SetComponent<TimeCrystal>(rewardEnt, new TimeCrystal(timeCrystals)); 
                    }

                    w.SetComponent<TextBox>(rewardEnt, new TextBox(rewardStr)); 
                }

                int ladderEnt = EntityFactory.AddUI(w, f.Position + dx(numRewards), Constants.TileWidth,
                    Constants.TileWidth, setOutline: true, setInteractable: true, text: "Ladder");
                w.SetComponent<Ladder>(ladderEnt, new Ladder());
            }
        });
    }
}

public class Floor {
    private int number;
    
    public Floor() {
        number = 0; 
    }

    public static implicit operator int(Floor f) {
        return f.number;
    }

    public static Floor operator ++(Floor f) {
        f.number = f.number + 1; 
        return f;
    }
}

public static class LadderInteractSystem {
    public static void Register(World w) {
        InteractSystem.Register<Ladder>(w, (w, _) => {
            int playerEnt = PlayerWrap.GetEntity(w);
            (Floor f, bool s1) = w.GetComponentSafe<Floor>(playerEnt);
            if (!s1) {
                f = new Floor();
                w.SetComponent<Floor>(playerEnt, f);
            }

            (Inventory inv, bool s2) = w.GetComponentSafe<Inventory>(playerEnt); 
            if (!s2) {
                throw new InvalidOperationException($"Player data ent {playerEnt} did not have inv component");
            }

            f++;
            Layout.DrawRandom(w);
            
            foreach (int e in w.GetMatchingEntities(EnemyWrap.EnemySignature)) {
                w.SetComponent<Loot>(e, new Loot(ItemID.TimeCrystal, f, inv));
            }
        });
    }
}