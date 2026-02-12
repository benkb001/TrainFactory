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

public class SetInvincibleMessage {}
public class ReceiveDamageMessage {
    private List<int> damageSources = new();

    public int DMG => damageSources.Aggregate(0, (acc, cur) => acc + cur); 
    public int FirstSourceDMG => damageSources[0]; 

    public ReceiveDamageMessage(int dmg) {
        damageSources.Add(dmg);
    }

    public void AddDamage(int dmg) {
        damageSources.Add(Math.Max(0, dmg));
    }

    public void ReduceDamage(int dmg) {
        damageSources.Add(-dmg);
    }

    public void SetDamage(int dmg) {
        damageSources.Clear();
        AddDamage(dmg);
    }
}

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
                bool hit = false; 

                foreach (int iEnt in intersectingEnts) {
                    if (potentialReceivers.Contains(iEnt)) {
                        hit = true; 
                        receivingDamage[iEnt] += dmg;
                    }
                }
                
                if (hit) {
                    w.RemoveEntity(e);
                }
            });

            potentialReceivers
            .ForEach(e => w.SetComponent<ReceiveDamageMessage>(e, new ReceiveDamageMessage(receivingDamage[e])));
        });
    }

    public static void RegisterArmor(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Armor), typeof(Active)], (w, e) => {
            int defense = w.GetComponent<Armor>(e).Defense;
            w.GetComponent<ReceiveDamageMessage>(e).ReduceDamage(defense); 
        });
    }

    public static void RegisterParry(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Parrier), typeof(Active)], (w, e) => {
            if (w.GetComponent<Parrier>(e).Parrying) {
                w.GetComponent<ReceiveDamageMessage>(e).SetDamage(0); 
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