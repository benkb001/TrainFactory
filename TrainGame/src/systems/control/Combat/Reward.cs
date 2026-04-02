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

public static class RewardInteractSystem {
    public static void Register<T>(World w, Action<World, int, int> tf) {
        InteractSystem.Register<T>(w, (w, e) => {
            int interactorEntity = w.GetComponent<Interactable>(e).InteractorEntity; 
            tf(w, e, interactorEntity);
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
            playerHealth.AddHP(potion.HP); 
        });
    }
}

public static class LootInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<Loot>(w, (w, e, interactorEntity) => {
            Loot l = w.GetComponent<Loot>(e); 
            l.Transfer();
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
                    bc.AddTempDamage(dmg);
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
                s.TempMaxAmmo += s.BaseMaxAmmo;
            }
        });
    }
}