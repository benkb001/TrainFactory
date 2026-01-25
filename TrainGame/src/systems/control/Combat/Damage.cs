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

public class ReceiveDamageMessage {
    private int dmg = 0; 
    public int DMG => dmg; 

    public ReceiveDamageMessage(int dmg) {
        this.dmg = dmg; 
    }

    public void AddDamage(int dmg) {
        this.dmg = Math.Max(0, this.dmg + dmg);
    }

    public void SetDamage(int dmg) {
        this.dmg = Math.Max(0, dmg); 
    }
}

public static class DamageSystem {
    public static void RegisterShoot<T, U>(World w) {
        w.AddSystem([typeof(T), typeof(Health), typeof(Frame), typeof(Active)], (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            Health health = w.GetComponent<Health>(e); 

            int receivedDamage = w.GetMatchingEntities([typeof(U), typeof(Bullet), typeof(Frame), typeof(Active)])
            .Where(ent => w.GetComponent<Frame>(ent).IntersectsWith(f))
            .Select(ent => {
                int dmg = w.GetComponent<Bullet>(ent).Damage;
                w.RemoveEntity(ent); 
                return dmg; 
            })
            .Aggregate(0, (acc, cur) => acc + cur); 

            w.SetComponent<ReceiveDamageMessage>(e, new ReceiveDamageMessage(receivedDamage)); 
        });
    }

    public static void RegisterArmor(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Armor), typeof(Active)], (w, e) => {
            int defense = w.GetComponent<Armor>(e).Defense;
            w.GetComponent<ReceiveDamageMessage>(e).AddDamage(-defense); 
        });
    }

    public static void RegisterParry(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Parrier), typeof(Active)], (w, e) => {
            if (w.GetComponent<Parrier>(e).Parrying) {
                w.GetComponent<ReceiveDamageMessage>(e).SetDamage(0); 
            }
        });
    }

    public static void RegisterReceive(World w) {
        w.AddSystem([typeof(ReceiveDamageMessage), typeof(Health), typeof(Active)], (w, e) => {
            w.GetComponent<Health>(e).ReceiveDamage(w.GetComponent<ReceiveDamageMessage>(e).DMG); 
            w.RemoveComponent<ReceiveDamageMessage>(e); 
        });
    }
}