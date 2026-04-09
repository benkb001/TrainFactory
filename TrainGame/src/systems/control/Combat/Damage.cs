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

public static class DamageSystem {
    public static void RegisterShoot<T, U>(World w) {
        w.AddSystem((w) => {
            List<int> bulletEnts = w.GetMatchingEntities([typeof(U), typeof(Bullet), typeof(Frame), typeof(Active)]);
            List<int> potentialReceivers = 
                w.GetMatchingEntities([typeof(T), typeof(Health), typeof(Active), typeof(Frame)]);
            
            Dictionary<int, int> receivingDamage = potentialReceivers
            .Select(e => new KeyValuePair<int, int>(e, 0))
            .ToDictionary(); 

            HashSet<int> intersectingEnts = new(); 
            
            bulletEnts.ForEach(e => {
                int dmg = w.GetComponent<Bullet>(e).Damage;

                MovementSystem.FillIntersectingEnts(w, e, intersectingEnts);

                foreach (int iEnt in intersectingEnts) {
                    if (potentialReceivers.Contains(iEnt)) {
                        w.SetComponent<HitMessage>(e, new HitMessage(iEnt));
                        //TODO: should enemies be able to be hit multiple times in the same frame? 
                        //for now probably doesn't matter but if we add a gun that has a 
                        //really high number of bullets, maybe 
                        receivingDamage[iEnt] = dmg; 
                        break;
                    }
                }
            });

            potentialReceivers
            .ForEach(e => {
                int dmg = receivingDamage[e]; 
                if (dmg > 0) {
                    w.SetComponent<ReceiveDamageMessage>(e, new ReceiveDamageMessage(receivingDamage[e]));
                }
            });
        });
    }

    public static void RegisterArmor(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Armor), typeof(Active)], (w, e) => {
            int defense = w.GetComponent<Armor>(e).Defense;
            ReceiveDamageMessage dm = w.GetComponent<ReceiveDamageMessage>(e);
            dm.ReduceDamage(defense); 
        });
    }

    public static void RegisterParry(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Parrier), typeof(Active)], (w, e) => {
            Parrier p = w.GetComponent<Parrier>(e);
            ReceiveDamageMessage msg = w.GetComponent<ReceiveDamageMessage>(e);
            if (msg.DMG > 0) {
                if (p.Parrying) {
                    p.GetHealth().ReceiveDamage(msg.DMG); 
                    msg.SetDamage(0); 
                } else {
                    msg.SetDamage(1);
                }
            }
        });
    }

    public static void RegisterAddInvincibleMessage(World w) {
        w.AddSystem([typeof(Health), typeof(Player), typeof(ReceiveDamageMessage), typeof(Active)], (w, e) => {
            ReceiveDamageMessage rm = w.GetComponent<ReceiveDamageMessage>(e);
            if (rm.DMG > 0) {
                rm.SetDamage(rm.FirstSourceDMG);
                w.SetComponent<SetInvincibleMessage>(e, new SetInvincibleMessage());
            }
        }); 
    }

    public static void RegisterSetInvincible(World w) {
        w.AddSystem([typeof(Health), typeof(SetInvincibleMessage)], (w, e) => {
            w.GetComponent<Health>(e).InvincibleFrames = Constants.InvincibilityFrames;
            w.RemoveComponent<SetInvincibleMessage>(e);
        });
    }

    public static void RegisterHealShield(World w) {
        w.AddSystem([typeof(CombatRewardSpawner), typeof(Active)], (w, spawnerEnt) => {
            CombatRewardSpawner sp = w.GetComponent<CombatRewardSpawner>(spawnerEnt);
            foreach (int e in w.GetMatchingEntities([typeof(HitMessage), typeof(ShotBy), typeof(Active), typeof(Player)])) {
                int hitEntity = w.GetComponent<HitMessage>(e).Entity; 
                int shotByEntity = w.GetComponent<ShotBy>(e).Entity; 
                (Health h, bool hasHealth) = w.GetComponentSafe<Health>(hitEntity); 
                (Parrier p, bool hasParrier) = w.GetComponentSafe<Parrier>(shotByEntity); 
                
                if (hasHealth && hasParrier && h.HP < 1) {
                    p.GetHealth().AddHP(sp.ShieldHealAmount);
                }
            }
        });
    }

    public static void RegisterReceive(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Health), typeof(Active)], (w, e) => {
            w.GetComponent<Health>(e).ReceiveDamage(w.GetComponent<ReceiveDamageMessage>(e).DMG); 
            w.RemoveComponent<ReceiveDamageMessage>(e); 
        });
    }

    public static void RegisterDecayInvincibility(World w) {
        w.AddSystem([typeof(Health), typeof(Active)], (w, e) => {
            w.GetComponent<Health>(e).InvincibleFrames--;
        });
    }
}