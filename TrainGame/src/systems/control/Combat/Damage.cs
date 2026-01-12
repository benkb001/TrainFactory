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
    public static void Register<T, U>(World w) {
        w.AddSystem([typeof(T), typeof(Health), typeof(Frame), typeof(Active)], (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            Health health = w.GetComponent<Health>(e); 

            List<int> collidingBulletEnts = 
                w.GetMatchingEntities([typeof(U), typeof(Bullet), typeof(Frame), typeof(Active)]).Where(
                ent => w.GetComponent<Frame>(ent).IntersectsWith(f)).ToList();
            
            foreach (int bulletEnt in collidingBulletEnts) {
                Bullet bullet = w.GetComponent<Bullet>(bulletEnt); 
                health.ReceiveDamage(bullet.Damage); 
                w.RemoveEntity(bulletEnt); 
            }

            if (collidingBulletEnts.Count > 0) {
                Console.WriteLine(health.HP);
            }
        });
    }
}