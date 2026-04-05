namespace TrainGame.Systems;

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class CollideBulletSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Bullet), typeof(Frame), typeof(Active)], (w, e) => {
            List<int> collidingEnts = MovementSystem.GetIntersectingEntities(w, e)
            .Where(ent => !w.ComponentContainsEntity<Player>(ent) && !w.ComponentContainsEntity<Shooter>(ent))
            .ToList();

            if (collidingEnts.Count > 0) {
                int otherEnt = collidingEnts[0];
                Frame otherFrame = w.GetComponent<Frame>(otherEnt); 
                Bullet b = w.GetComponent<Bullet>(e);
                w.SetComponent<Collided>(e, new Collided(otherEnt, otherFrame));
            }
        });
    }
}

public static class RemoveOnCollisionSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Collided), typeof(RemoveOnCollision), typeof(Active)], (w, e) => {
            w.RemoveEntity(e);
        });
    }
}