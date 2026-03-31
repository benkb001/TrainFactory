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

public static class DecayBulletSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Bullet), typeof(Active)], (w, e) => {
            Bullet b = w.GetComponent<Bullet>(e); 
            b.Decay(); 
            if (b.ShouldRemove) {
                w.SetComponent<Expired>(e, new Expired()); 
            }
        });
    }
}

public static class RemoveExpiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Expired), typeof(Active)], (w, e) => {
            w.RemoveEntity(e);
        });
    }
}

public static class RemoveOnHitSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Active), typeof(HitMessage), typeof(RemoveOnCollision)], (w, e) => {
            w.RemoveEntity(e);
        });
    }
}