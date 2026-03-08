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
            List<int> collidingEnts = MovementSystem.GetIntersectingEntities(w, e)
            .Where(ent => !w.ComponentContainsEntity<Player>(ent) && !w.ComponentContainsEntity<Shooter>(ent))
            .ToList();

            if (collidingEnts.Count > 0) {
                int otherEnt = collidingEnts[0];
                Frame otherFrame = w.GetComponent<Frame>(otherEnt); 
                Bullet b = w.GetComponent<Bullet>(e);

                switch (b.GetOnCollideEffect()) {
                    case OnCollideEffect.Bounce: 
                        //need to get which side of the wall it collided with. 
                        //
                        

                        break;
                    case OnCollideEffect.None:
                        break;
                    default: 
                        throw new InvalidOperationException("Unimplemented OnCollideEffect");
                }

                if (b.IsRemovedOnCollision) {
                    w.RemoveEntity(e); 
                }
            }
        });
    }
}