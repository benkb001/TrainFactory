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

public static class EnemyShootSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Enemy), typeof(Shooter), typeof(Health), typeof(Frame), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            if (w.Time.IsAfterOrAt(shooter.CanShoot)) {
                int targetableEnt = w.GetFirstMatchingEntity([typeof(Targetable), typeof(Frame), typeof(Active)]); 
                (Frame targetableFrame, bool s) = w.GetComponentSafe<Frame>(targetableEnt); 

                if (s) {
                    w.SetComponent<ShotMessage>(e, new ShotMessage(targetableFrame.Position));
                }
            }
        });
    }
}