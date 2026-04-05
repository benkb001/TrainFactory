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
            LootWrap.Transfer(w, l, inv);
        });
    }
}

public static class DamagePotionInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<DamagePotion>(w, (w, e, interactorEntity) => {
            (IShootPattern p, bool hasSP) = w.GetComponentSafe<IShootPattern>(interactorEntity);
            if (hasSP) {
                int dmg = w.GetComponent<DamagePotion>(e).DMG;
            
                foreach (BulletContainer bc in p.GetBulletContainers()) {
                    bc.AddDamage(dmg); 
                }
            }
        });
    }
}

public static class MaxAmmoInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<MaxAmmo>(w, (w, e, interactorEntity) => {
            (Shooter s, bool hasShooter) = w.GetComponentSafe<Shooter>(interactorEntity); 
            if (hasShooter) {
                s.MaxAmmo += s.BaseMaxAmmo;
            }
        });
    }
}

public static class BulletSizeIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<BulletSizeIncrease, IShootPattern>(w, (w, _, sp, _) => {
            foreach (BulletContainer bc in sp.GetBulletContainers()) {
                bc.Width += Constants.BulletSizeIncrease;
                bc.Height += Constants.BulletSizeIncrease;
            }
        });
    }
}

public static class BulletSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<BulletSpeedIncrease, IShootPattern>(w, (w, _, sp, _) => {
            foreach (BulletContainer bc in sp.GetBulletContainers()) {
                bc.Speed += Constants.BulletSpeedIncrease;
            }
        });
    }
}

public static class UnloadSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<UnloadSpeedIncrease, Shooter>(w, (w, _, shooter, _) => {
            shooter.TimeBetweenShots -= Constants.TicksBetweenShotDecrement;
            if (shooter.TimeBetweenShots.InTicks() < 1) {
                shooter.TimeBetweenShots = new WorldTime(ticks: 1);
            }
        });
    }
}

public static class ReloadSpeedIncreaseInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<ReloadSpeedIncrease, Shooter>(w, (w, _, shooter, _) => {
            shooter.ReloadTime -= Constants.ReloadTicksDecrement;
            if (shooter.ReloadTime.InTicks() < 1) {
                shooter.ReloadTime = new WorldTime(ticks: 1);
            }
        });
    }
}

public static class AddExplosionInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<AddExplosion, IShootPattern>(w, (w, _, sp, _) => {
            foreach (BulletContainer bc in sp.GetBulletContainers()) {
                bool hasExplosion = false; 
                foreach (IBulletTrait bt in bc.GetTraits()) {
                    if (bt is Split split && split.Pattern is MeleeShootPattern msp) {
                        foreach (BulletContainer innerBC in msp.GetBulletContainers()) {
                            innerBC.AddDamage(1);
                        }
                        hasExplosion = true;
                        break;
                    }
                }
                
                if (!hasExplosion) {
                    bc.AddTrait(
                        new Split(
                            new MeleeShootPattern(
                                new BulletContainer(
                                    new Bullet(3, maxFramesActive: 10),
                                    new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2)
                                )
                            )
                        )
                    );
                }
            }
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

    private static Action<World, int> setLoot = (w, e) => {
        (string itemID, int count) = lootDist.GetRandom(); 
        w.SetComponent<TextBox>(e, new TextBox($"+{count} {itemID}"));
        w.SetComponent<Loot>(e, new Loot(itemID, count));
        w.SetComponent<Inventory>(e, LootWrap.GetDestination(w));
    };

    private static Action<World, int> setMaxAmmo = (w, e) => {
        w.SetComponent<MaxAmmo>(e, new MaxAmmo());
        w.SetComponent<TextBox>(e, new TextBox("+Max Ammo"));
    };

    private static Action<World, int> setHealthPotion = (w, e) => {
        w.SetComponent<HealthPotion>(e, new HealthPotion(1)); 
        w.SetComponent<TextBox>(e, new TextBox("+1 HP"));
    };

    private static Action<World, int> setDamagePotion = (w, e) => {
        w.SetComponent<DamagePotion>(e, new DamagePotion(1)); 
        w.SetComponent<TextBox>(e, new TextBox("+1 DMG"));
    };

    private static Action<World, int> setBulletSizeIncrease = (w, e) => {
        w.SetComponent<BulletSizeIncrease>(e, new BulletSizeIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Bullet Size")); 
    };

    private static Action<World, int> setBulletSpeedIncrease = (w, e) => {
        w.SetComponent<BulletSpeedIncrease>(e, new BulletSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Bullet Speed"));
    };

    private static Action<World, int> setUnloadSpeedIncrease = (w, e) => {
        w.SetComponent<UnloadSpeedIncrease>(e, new UnloadSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Unload Speed"));
    };

    private static Action<World, int> setReloadSpeedIncrease = (w, e) => {
        w.SetComponent<ReloadSpeedIncrease>(e, new ReloadSpeedIncrease()); 
        w.SetComponent<TextBox>(e, new TextBox("+1 Reload Speed"));
    };

    private static Action<World, int> setAddExplosion = (w, e) => {
        w.SetComponent<AddExplosion>(e, new AddExplosion()); 
        w.SetComponent<TextBox>(e, new TextBox("Add Explosion"));
    };

    private static Distribution<Type, Action<World, int>> rewardDist = new Distribution<Type, Action<World, int>>(
        new Dictionary<Type, int>(){
            [typeof(Loot)] = 10,
            [typeof(MaxAmmo)] = 10,
            [typeof(HealthPotion)] = 10,
            [typeof(DamagePotion)] = 10,
            [typeof(BulletSizeIncrease)] = 10,
            [typeof(BulletSpeedIncrease)] = 10,
            [typeof(UnloadSpeedIncrease)] = 10,
            [typeof(ReloadSpeedIncrease)] = 10,
            [typeof(AddExplosion)] = 10
        },
        new Dictionary<Type, Action<World, int>>(){
            [typeof(Loot)] = setLoot,
            [typeof(MaxAmmo)] = setMaxAmmo,
            [typeof(HealthPotion)] = setHealthPotion,
            [typeof(DamagePotion)] = setDamagePotion,
            [typeof(BulletSizeIncrease)] = setBulletSizeIncrease,
            [typeof(BulletSpeedIncrease)] = setBulletSpeedIncrease,
            [typeof(UnloadSpeedIncrease)] = setUnloadSpeedIncrease,
            [typeof(ReloadSpeedIncrease)] = setReloadSpeedIncrease,
            [typeof(AddExplosion)] = setAddExplosion
        }
    );

    private static Type setReward(World w, int e) {
        (Type t, Action<World, int> setter) = rewardDist.GetRandom(); 
        setter(w, e); 
        return t; 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(Enemy), typeof(Health), typeof(Expired), typeof(Frame), typeof(Active)], (w, e) => {
            bool rewardOnGround = w.GetMatchingEntities([typeof(CombatReward), typeof(Active)]).Count > 0; 
            if (!rewardOnGround && Util.NextFloat() < Constants.RewardChance) {
                int[] es = {-1, -1};
                List<Type> ts = new();
                Vector2 pos = w.GetComponent<Frame>(e).Position;

                for (int i = 0; i < 2; i++) {
                    Vector2 curPos = pos + new Vector2(i * 2 * Constants.TileWidth, 0f);
                    int rewardEnt = EntityFactory.AddUI(w, curPos, Constants.TileWidth, Constants.TileWidth, 
                    setOutline: true, setInteractable: true);
                    w.SetComponent<CombatReward>(rewardEnt, new CombatReward(w.Time)); 
                    es[i] = rewardEnt;
                    ts.Add(setReward(w, rewardEnt)); 
                }

                if (ts[0] == ts[1]) {
                    if (ts[0] != typeof(Loot)) {
                        setLoot(w, es[0]); 
                    } else {
                        setMaxAmmo(w, es[0]);
                    }
                }
            }
        });
    }
}