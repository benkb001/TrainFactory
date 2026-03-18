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

public static class ShootSystem {
    public static void Register<U>(World w) 
    where U : IFlag<U> {
        w.AddSystem([typeof(U), typeof(ShotMessage), typeof(Shooter), typeof(Frame), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            Frame f = w.GetComponent<Frame>(e);
            Vector2 targetPosition = w.GetComponent<ShotMessage>(e).TargetPosition;
            IEnumerable<BulletContainer> bs = shooter.Shoot(w.Time, f.Position, targetPosition);

            foreach (BulletContainer b in bs) {
                Bullet bullet = b.GetBullet();
                int ent = EntityFactory.AddUI(w, b.GetPosition(), b.GetWidth(), b.GetWidth(), 
                    setOutline: true);
                w.SetComponent<Velocity>(ent, new Velocity(b.GetVelocity())); 
                w.SetComponent<Bullet>(ent, bullet);
                w.SetComponent<U>(ent, U.Get());

                if (b.IsWarned) {
                    w.RemoveComponent<Active>(ent); 
                    int warnEnt = EntityFactory.Add(w); 
                    Frame bulletFrame = w.GetComponent<Frame>(ent);
                    w.SetComponent<Frame>(warnEnt, new Frame(bulletFrame));
                    bulletFrame.SetCoordinates(SceneSystem.OffScreenPosition);
                    BulletWarning warn = new BulletWarning(w.Time + b.WarningDuration, ent);
                    w.SetComponent<BulletWarning>(warnEnt, warn); 
                    w.SetComponent<Outline>(warnEnt, new Outline(Colors.Warning));
                    TextBox tb = new TextBox("!"); 
                    tb.TextColor = Colors.Warning;
                    w.SetComponent<TextBox>(warnEnt, tb);
                }
            }

            w.RemoveComponent<ShotMessage>(e);
        });
    }
}

public static class PlayerShootSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftPressed()) {
                //TODO: Add back in after refactor
            }
        });
    }
}