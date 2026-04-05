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

public static class HomingSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Homing), typeof(Velocity), typeof(Frame), typeof(Active)], (w, e) => {
            
            Homing h = w.GetComponent<Homing>(e); 
            int otherEnt = h.TrackedEntity; 
            (Frame otherFrame, bool success) = w.GetComponentSafe<Frame>(otherEnt); 
            if (success) {
                Frame f = w.GetComponent<Frame>(e);
                Vector2 dv = w.GetComponent<Velocity>(e);

                Vector2 targetVelocity = otherFrame.Position - f.Position;
                Vector2 newVelocity = Vector2.Normalize(dv + (targetVelocity * 0.005f)) * h.Speed;
                w.SetComponent<Velocity>(e, new Velocity(newVelocity));
            } else {
                w.RemoveComponent<Homing>(e);
            }
        }); 
    }
}