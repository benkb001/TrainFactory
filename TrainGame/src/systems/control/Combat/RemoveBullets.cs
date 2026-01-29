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

public static class RemoveBulletSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Bullet), typeof(Active)], (w, e) => {
            Bullet b = w.GetComponent<Bullet>(e); 
            b.Decay(); 
            if (b.ShouldRemove) {
                w.RemoveEntity(e); 
            }
        });
    }
}

public static class CollideBulletSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Bullet), typeof(Frame), typeof(Active)], (w, e) => {
            if (MovementSystem.GetIntersectingEntities(w, e)
                .Where(ent => !w.ComponentContainsEntity<Player>(ent) && !w.ComponentContainsEntity<Shooter>(ent))
                .ToList()
                .Count > 0) {

                w.RemoveEntity(e); 
            }
        });
    }
}