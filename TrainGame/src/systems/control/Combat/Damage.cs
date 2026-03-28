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

public class HitMessage {
    public int Entity; 
    public HitMessage(int e) {
        this.Entity = e; 
    }
}

public static class RemoveOnHitSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Active), typeof(HitMessage), typeof(RemoveOnCollision)], (w, e) => {
            w.RemoveEntity(e);
        });
    }
}

public class Vampired {
    public readonly int VampiredByEntity; 
    public readonly int Damage;

    public Vampired(int e, int damage) {
        this.VampiredByEntity = e; 
        this.Damage = damage;
    }
}

public static class DrawVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampired), typeof(Frame), typeof(Active)], (w, e) => {
            int vampiredBy = w.GetComponent<Vampired>(e).VampiredByEntity; 
            (Frame f, bool hasFrame) = w.GetComponentSafe<Frame>(vampiredBy); 
            if (hasFrame) {
                Vector2 vampirePos = f.Position; 
                Vector2 pos = w.GetComponent<Frame>(e).Position;

                Lines ls = new Lines(); 
                
                ls.AddLine(pos, vampirePos, Colors.Vampiric);
                w.SetComponent<Lines>(e, ls);
            }
        });
    }
}

public static class ApplyVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampiric), typeof(HitMessage), typeof(ShotBy), typeof(Active)], (w, e) => {
            int hitEntity = w.GetComponent<HitMessage>(e).Entity; 
            int shooterEnt = w.GetComponent<ShotBy>(e).Entity;
            int dmg = w.GetComponent<Vampiric>(e).Damage;
            w.SetComponent<Vampired>(hitEntity, new Vampired(shooterEnt, dmg));
        });
    }
}

public static class RemoveVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampired), typeof(Active)], (w, e) => {
            int vampiredByEnt = w.GetComponent<Vampired>(e).VampiredByEntity;
            if (!w.EntityExists(vampiredByEnt)) {
                w.RemoveComponent<Vampired>(e);
            }
        });
    }
}

public static class VampireDamageSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (w.Time.Ticks == 0) {
                List<int> vampiredEnts = w.GetMatchingEntities([typeof(Vampired), typeof(Active)]);
                vampiredEnts.ForEach(e => {
                    int dmg = w.GetComponent<Vampired>(e).Damage;
                    w.SetComponent<ReceiveDamageMessage>(e, new ReceiveDamageMessage(dmg));
                });
            }
        });
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