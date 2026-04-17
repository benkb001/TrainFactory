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

public static class BulletWarningShootSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(BulletWarning), typeof(Frame), typeof(Active)], (w, e) => {
            BulletWarning warn = w.GetComponent<BulletWarning>(e);
            if (w.Time.IsAfterOrAt(warn.WhenToShoot)) {
                int bulletEnt = warn.BulletEnt; 
                w.SetComponentSafe<Active>(bulletEnt, new Active()); 
                (Frame f, bool s) = w.GetComponentSafe<Frame>(bulletEnt); 
                
                if (s) {
                    f.SetCoordinates(w.GetComponent<Frame>(e).Position);
                }

                w.RemoveEntity(e); 
            }
        });
    }
}