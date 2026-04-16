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

public class BulletSizeIncrease {}
public class BulletSpeedIncrease {}
public class UnloadSpeedIncrease {}
public class ReloadSpeedIncrease {}
public class AddExplosion {}
public class AddHoming {}
public class AddShield {}
public class AddKnockback {}

public static class RewardInteractSystem {
    public static void Register<T>(World w, Action<World, int, int> tf) {
        InteractSystem.Register<T>(w, (w, e) => {
            int interactorEntity = w.GetComponent<Interactable>(e).InteractorEntity; 
            tf(w, e, interactorEntity);
            MakeMessage.Add<CombatRewardCollectedMessage>(w, new CombatRewardCollectedMessage()); 
        });
    }

    public static void Register<T, U>(World w, Action<World, T, U, int> tf) {
        InteractSystem.Register<T>(w, (w, e, t) => {
            int interactorEntity = w.GetComponent<Interactable>(e).InteractorEntity; 
            (U u, bool hasU) = w.GetComponentSafe<U>(interactorEntity); 
            if (hasU) {
                tf(w, t, u, interactorEntity);
            }
            MakeMessage.Add<CombatRewardCollectedMessage>(w, new CombatRewardCollectedMessage()); 
        });
    }

    public static void RegisterRemove(World w) {
        w.AddSystem([typeof(CombatReward), typeof(Active)], (w, e) => {
            if (w.Time.IsAfterOrAt(w.GetComponent<CombatReward>(e).Expire)) {
                w.RemoveEntity(e);
            }
        });

        w.AddSystem([typeof(CombatRewardCollectedMessage)], (w, e) => {
            List<int> rewardEnts = w.GetMatchingEntities([typeof(CombatReward)]); 
            rewardEnts.ForEach(ent => {
                w.RemoveEntity(ent);
            }); 
            w.RemoveEntity(e); 
        });
    }
}

public static class TempArmorInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<TempArmor>(w, (w, e, interactorEntity) => {
            TempArmor tArmor = w.GetComponent<TempArmor>(e); 
            Armor playerArmor = w.GetComponent<Armor>(interactorEntity); 
            playerArmor.AddTempDefense(tArmor.Defense); 
        });
    }
}

public static class HealthPotionInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<HealthPotion>(w, (w, e, interactorEntity) => {
            Health playerHealth = w.GetComponent<Health>(interactorEntity); 
            HealthPotion potion = w.GetComponent<HealthPotion>(e);
            playerHealth.TempHP += potion.HP; 
        });
    }
}

public static class LootInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<Loot>(w, (w, e, interactorEntity) => {
            Loot l = w.GetComponent<Loot>(e); 
            Inventory inv = w.GetComponent<Inventory>(e);
            inv.Add(l.ItemID, l.Count);
        });
    }
}

public static class DamagePotionInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<DamagePotion, IShootPattern>(w, (w, dmgPot, sp, _) => {
            ShooterWrap.UpgradeDamage(sp, dmgPot.DMG);
        });
    }
}

public static class MaxAmmoInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<MaxAmmo, Shooter>(w, (w, _, s, _) => {
            ShooterWrap.UpgradeMaxAmmo(s);
        });
    }
}

public static class BulletSizeIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<BulletSizeIncrease, IShootPattern>(w, (w, _, sp, _) => {
            ShooterWrap.UpgradeBulletSize(sp);
        });
    }
}

public static class BulletSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<BulletSpeedIncrease, IShootPattern>(w, (w, _, sp, _) => {
            ShooterWrap.UpgradeBulletSpeed(sp);
        });
    }
}

public static class UnloadSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<UnloadSpeedIncrease, Shooter>(w, (w, _, shooter, _) => {
            ShooterWrap.UpgradeUnloadSpeed(shooter);
        });
    }
}

public static class ReloadSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<ReloadSpeedIncrease, Shooter>(w, (w, _, shooter, _) => {
            ShooterWrap.UpgradeReloadSpeed(shooter);
        });
    }
}

public static class AddExplosionInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<AddExplosion, IShootPattern>(w, (w, _, sp, _) => {
            ShooterWrap.AddExplosion(sp);
        });
    }
}

public static class AddHomingInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<AddHoming, IShootPattern>(w, (w, _, sp, _) => {
            ShooterWrap.AddHoming(sp);
        });
    }
}

public static class AddShieldInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<AddShield, Parrier>(w, (w, _, parrier, _) => {
            Health h = parrier.GetHealth();
            h.TempMaxHP += Constants.ShieldIncrement; 
            h.AddHP(Constants.ShieldIncrement);
        });
    }
}

public static class AddKnockbackInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<AddKnockback, IShootPattern>(w, (w, _, sp, _) => {
            ShooterWrap.AddKnockback(sp);
        });
    }
}

public static class RewardSpawnSystem {
    
    private static LootDistribution lootDist = new LootDistribution(
        new Dictionary<string, int>(){
            [ItemID.Credit] = 50,
            [ItemID.Cobalt] = 50
        },
        new Dictionary<string, int>(){
            [ItemID.Credit] = 100,
            [ItemID.Cobalt] = 100
        }
    );

    private static Action<World, CombatRewardSpawner, int> setLoot = (w, spawn, e) => {
        (string itemID, int count) = lootDist.GetRandom(); 
        count *= spawn.LootMultiplier;
        w.SetComponent<TextBox>(e, new TextBox($"+{count} {itemID}"));
        w.SetComponent<Loot>(e, new Loot(itemID, count));
        w.SetComponent<Inventory>(e, LootWrap.GetDestination(w));
    };

    private static Action<World, CombatRewardSpawner, int> setMaxAmmo = (w, spawn, e) => {
        w.SetComponent<MaxAmmo>(e, new MaxAmmo());
        w.SetComponent<TextBox>(e, new TextBox("+Max Ammo"));
    };

    private static Action<World, CombatRewardSpawner, int> setHealthPotion = (w, spawn, e) => {
        w.SetComponent<HealthPotion>(e, new HealthPotion(1)); 
        w.SetComponent<TextBox>(e, new TextBox("+1 HP"));
    };

    private static Action<World, CombatRewardSpawner, int> setDamagePotion = (w, spawn, e) => {
        w.SetComponent<DamagePotion>(e, new DamagePotion(1)); 
        w.SetComponent<TextBox>(e, new TextBox("+1 DMG"));
    };

    private static Action<World, CombatRewardSpawner, int> setBulletSizeIncrease = (w, spawn, e) => {
        w.SetComponent<BulletSizeIncrease>(e, new BulletSizeIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Bullet Size")); 
    };

    private static Action<World, CombatRewardSpawner, int> setBulletSpeedIncrease = (w, spawn, e) => {
        w.SetComponent<BulletSpeedIncrease>(e, new BulletSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Bullet Speed"));
    };

    private static Action<World, CombatRewardSpawner, int> setUnloadSpeedIncrease = (w, spawn, e) => {
        w.SetComponent<UnloadSpeedIncrease>(e, new UnloadSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Unload Speed"));
    };

    private static Action<World, CombatRewardSpawner, int> setReloadSpeedIncrease = (w, spawn, e) => {
        w.SetComponent<ReloadSpeedIncrease>(e, new ReloadSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Reload Speed"));
    };

    private static Action<World, CombatRewardSpawner, int> setAddExplosion = (w, spawn, e) => {
        w.SetComponent<AddExplosion>(e, new AddExplosion()); 
        w.SetComponent<TextBox>(e, new TextBox("Add Explosion"));
    };

    private static Action<World, CombatRewardSpawner, int> setAddHoming = (w, spawn, e) => {
        w.SetComponent<AddHoming>(e, new AddHoming()); 
        w.SetComponent<TextBox>(e, new TextBox("Add Homing"));
    };

    private static Action<World, CombatRewardSpawner, int> setAddShield = (w, spawn, e) => {
        w.SetComponent<AddShield>(e, new AddShield()); 
        w.SetComponent<TextBox>(e, new TextBox($"+{Constants.ShieldIncrement} Shield"));
    };

    private static Action<World, CombatRewardSpawner, int> setAddKnockback = (w, spawn, e) => {
        w.SetComponent<AddKnockback>(e, new AddKnockback()); 
        w.SetComponent<TextBox>(e, new TextBox($"+1 Knockback"));
    };

    private static Distribution<Type, Action<World, CombatRewardSpawner, int>> rewardDist = 
    new Distribution<Type, Action<World, CombatRewardSpawner, int>>(
        new Dictionary<Type, int>(){
            [typeof(HealthPotion)] = 17,
            [typeof(DamagePotion)] = 17,
            [typeof(ReloadSpeedIncrease)] = 15,
            [typeof(AddExplosion)] = 10,
            [typeof(MaxAmmo)] = 10,
            [typeof(AddShield)] = 8,
            [typeof(UnloadSpeedIncrease)] = 8,
            [typeof(BulletSpeedIncrease)] = 4,
            [typeof(BulletSizeIncrease)] = 4,
            [typeof(AddKnockback)] = 4,
            [typeof(AddHoming)] = 3
        },
        new Dictionary<Type, Action<World, CombatRewardSpawner, int>>(){
            [typeof(MaxAmmo)] = setMaxAmmo,
            [typeof(HealthPotion)] = setHealthPotion,
            [typeof(DamagePotion)] = setDamagePotion,
            [typeof(BulletSizeIncrease)] = setBulletSizeIncrease,
            [typeof(BulletSpeedIncrease)] = setBulletSpeedIncrease,
            [typeof(UnloadSpeedIncrease)] = setUnloadSpeedIncrease,
            [typeof(ReloadSpeedIncrease)] = setReloadSpeedIncrease,
            [typeof(AddExplosion)] = setAddExplosion,
            [typeof(AddHoming)] = setAddHoming,
            [typeof(AddShield)] = setAddShield,
            [typeof(AddKnockback)] = setAddKnockback
        }
    );

    private static Type setReward(World w, CombatRewardSpawner spawn, int e) {
        (Type t, Action<World, CombatRewardSpawner, int> setter) = rewardDist.GetRandom(); 
        setter(w, spawn, e); 
        return t; 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(CombatRewardSpawner), typeof(Active)], (w, e) => {
            List<int> killedEnemyEnts = 
            w.GetMatchingEntities([typeof(Enemy), typeof(Health), typeof(Expired), typeof(Frame), typeof(Active)]);
            CombatRewardSpawner spawn = w.GetComponent<CombatRewardSpawner>(e);

            foreach (int enemyEnt in killedEnemyEnts) {
                EnemyType type = w.GetComponent<Enemy>(enemyEnt).Type; 
                int diff = EnemyID.Enemies[type].Difficulty; 
                spawn.XP += diff + spawn.ExtraXPPerKill; 
            }

            if (spawn.XP >= spawn.XPToNextLevel) {
                //TODO: Refine this growth function
                spawn.XP -= spawn.XPToNextLevel;
                spawn.XPToNextLevel += 5; 
                
                int[] es = {-1, -1};
                List<Type> ts = new();
                Vector2 pos = w.GetComponent<Frame>(killedEnemyEnts[0]).Position;

                for (int i = 0; i < 2; i++) {
                    Vector2 curPos = pos + new Vector2(i * 2 * Constants.TileWidth, 0f);
                    int rewardEnt = EntityFactory.AddUI(w, curPos, Constants.TileWidth, Constants.TileWidth, 
                    setOutline: true, setInteractable: true);
                    w.SetComponent<CombatReward>(rewardEnt, new CombatReward(w.Time)); 
                    es[i] = rewardEnt;
                    ts.Add(setReward(w, spawn, rewardEnt)); 
                }

                if (ts[0] == ts[1]) {
                    if (ts[0] != typeof(Loot)) {
                        setLoot(w, spawn, es[0]); 
                    } else {
                        setMaxAmmo(w, spawn, es[0]);
                    }
                }
            }
        });
    }
}