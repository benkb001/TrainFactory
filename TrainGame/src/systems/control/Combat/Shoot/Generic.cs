namespace TrainGame.Systems; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks;

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

            tf(w, t, f, targetPosition, e);

            Shooter shooter = w.GetComponent<Shooter>(e); 
            shooter.Update(w.Time);
        });
    }

    public static void RegisterReload(World w) {
        w.AddSystem([typeof(Shooter), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e); 
            if (shooter.Reloading && w.Time.IsAfterOrAt(shooter.CanShoot)) {
                shooter.Reloading = false; 
                shooter.Ammo = shooter.MaxAmmo;
            }
        });
    }

    public static void RegisterDrawReload(World w) {
        w.AddSystem([typeof(Shooter), typeof(Player), typeof(Health), typeof(Body), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            if (shooter.Reloading && w.Time.IsAt(shooter.LastShot)) {
                int labelEnt = w.GetComponent<Body>(e).LabelEntity; 
                (LinearLayout ll, bool hasLL) = w.GetComponentSafe<LinearLayout>(labelEnt); 
                if (hasLL) {
                    int progressBarEnt = DrawProgressBarCallback.Draw(w, Vector2.Zero, Constants.PlayerWidth, Constants.PlayerWidth / 4f);
                    w.SetComponent<ReloadBar>(progressBarEnt, new ReloadBar(shooter));
                    ll.AddChild(progressBarEnt);
                }
            }
        });
    }

    public static void RegisterDrawReloadCompletion(World w) {
        w.AddSystem([typeof(ProgressBar), typeof(ReloadBar), typeof(Active)], (w, e) => {
            w.GetComponent<ProgressBar>(e).Completion = w.GetComponent<ReloadBar>(e).GetCompletion(w.Time);
        });
    } 

    public static void RegisterEraseReloadCompletionBar(World w) {
        w.AddSystem([typeof(Player), typeof(Shooter), typeof(Body), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            if (shooter.Reloading && w.Time.IsAfterOrAt(shooter.CanShoot)) {
                int labelEnt = w.GetComponent<Body>(e).LabelEntity; 
                (LinearLayout ll, bool hasLL) = w.GetComponentSafe<LinearLayout>(labelEnt);
                List<int> toRemove = new();
                if (hasLL) {
                    foreach (int cEnt in ll.GetChildren()) {
                        if (w.ComponentContainsEntity<ReloadBar>(cEnt)) {
                            toRemove.Add(cEnt);
                        }
                    }

                    foreach (int cEnt in toRemove) {
                        w.RemoveEntity(cEnt); 
                        ll.RemoveChild(cEnt);
                    }
                }
            }
        });
    }
}

public class ReloadBar {
    private Shooter shooter;
    public float GetCompletion(WorldTime now) => shooter.GetReloadCompletion(now);
    public ReloadBar(Shooter s) {
        this.shooter = s;
    }
}