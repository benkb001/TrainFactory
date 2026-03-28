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

public static class DefaultShootSystem {
    public static void Register<U>(World w) 
    where U : IFlag<U> {
        ShootSystem.Register<DefaultShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            float offset = (float)(sp.Inaccuracy * Util.NextDouble()); 
            targetPosition += new Vector2(offset, offset); 
            ShooterWrap.Add<U>(w, f.Position, targetPosition, sp.Bullet);
            return 1; 
        });
    }
}

public static class MeleeShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<MeleeShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            float width = sp.Bullet.Width;
            float shooterWidth = f.GetWidth();
            Vector2 position = f.Position; 

            float delta = (width - shooterWidth) / 2f; 
            Vector2 bulletPos = new Vector2(position.X - delta, position.Y - delta);
            ShooterWrap.Add<U>(w, bulletPos, bulletPos, sp.Bullet);
            return 1; 
        });
    }
}

public static class RadialShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<RadialShootPattern, U>(w, (w, sp, f, _, e) => {
            int bullets = sp.BulletsPerShot; 
            float speed = sp.Bullet.Speed;
            Vector2 pos = f.Position;

            double radiansPerShot = 2*Math.PI / bullets;
            double offset = sp.OffsetRadians;
            
            for (int i = 0; i < bullets; i++) {
                float dx = (float)(Math.Cos((radiansPerShot * i) + offset) * speed);
                float dy = (float)(Math.Sin((radiansPerShot * i) + offset) * speed);
                int bulletEnt = ShooterWrap.Add<U>(w, pos, pos, sp.Bullet); 
                w.SetComponent<Velocity>(bulletEnt, new Velocity(new Vector2(dx, dy)));
            }

            return bullets; 
        });
    }
}

public static class ShootSystem {
    //tf should return the number of bullets shot
    public static void Register<T, U>(World w, Func<World, T, Frame, Vector2, int, int> tf) 
    where U : IFlag<U> 
    where T : IShootPattern {
        w.AddSystem([typeof(T), typeof(U), typeof(ShotMessage), typeof(Shooter), 
                     typeof(Frame), typeof(Active)], (w, e) => {

                    
            Frame f = w.GetComponent<Frame>(e); 
            T t = w.GetComponent<T>(e);
            Vector2 targetPosition = w.GetComponent<ShotMessage>(e).TargetPosition; 

            int bulletsShot = tf(w, t, f, targetPosition, e);

            Shooter shooter = w.GetComponent<Shooter>(e); 
            shooter.Update(w.Time, bulletsShot);
        });
    }
}

public static class RemoveShotMessageSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ShotMessage), typeof(Active)], (w, e) => {
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