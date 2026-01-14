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
        w.AddSystem([typeof(Bullet)], (w, e) => {
            if (w.Time.IsAfterOrAt(w.GetComponent<Bullet>(e).TimeShot + new WorldTime(minutes: 10))) {
                w.RemoveEntity(e); 
            }
        });
    }
}

public static class CollideBulletSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Bullet), typeof(Active)], (w, e) => {
            Frame bulletFrame = w.GetComponent<Frame>(e); 

            List<Frame> collidingFrames = w
            .GetMatchingEntities([typeof(Frame), typeof(Collidable), typeof(Active)])
            .Where(ent => !w.ComponentContainsEntity<Shooter>(ent))
            .Select(collidableEnt => w.GetComponent<Frame>(collidableEnt))
            .Where(f => bulletFrame.IntersectsWith(f))
            .ToList();

            if (collidingFrames.Count > 0) {
                w.RemoveEntity(e); 
            }
            
        });
    }
}