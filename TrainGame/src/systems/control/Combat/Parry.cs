namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public class Parrier {
    private WorldTime canParry; 
    private WorldTime cooldown; 
    private WorldTime duration; 
    private WorldTime parryDecrease; 
    private WorldTime endParry; 
    private WorldTime startedParry; 
    private bool parrying = false; 
    public bool Parrying => parrying; 

    public Parrier() {
        canParry = new WorldTime(); 
        endParry = new WorldTime(); 
        startedParry = new WorldTime(); 
        cooldown = new WorldTime(minutes: 3); 
        duration = new WorldTime(ticks: 30); 
        parryDecrease = new WorldTime(ticks: 30); 
    }

    public bool CanParry(WorldTime now) {
        return now.IsAfterOrAt(canParry); 
    }

    public void StartParry(WorldTime now) {
        startedParry = now.Clone(); 
        canParry = now + cooldown; 
        endParry = now + duration;
        parrying = true; 
    }

    public void Parry() {
        canParry = canParry - parryDecrease; 
    }

    public bool ParryEnded(WorldTime now) {
        if (parrying && now.IsAfterOrAt(endParry)) {
            parrying = false; 
            return true; 
        }
        return false; 
    }

    public float PercentCooldownComplete(WorldTime now) {
        WorldTime waitTime = canParry - startedParry;
        return ((now - startedParry) / waitTime); 
    }
}

public class ParryCooldownBar {
    private Parrier parrier; 

    public ParryCooldownBar(Parrier p) {
        parrier = p; 
    }

    public float Completion(WorldTime now) {
        return parrier.PercentCooldownComplete(now); 
    }
}

public static class ParryCooldownBarSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ParryCooldownBar), typeof(ProgressBar), typeof(Active)], (w, e) => {
            ParryCooldownBar cb = w.GetComponent<ParryCooldownBar>(e); 
            ProgressBar pb = w.GetComponent<ProgressBar>(e); 
            pb.Completion = cb.Completion(w.Time); 
            if (pb.Completion >= 1f) {
                w.RemoveEntity(e); 
            }
        });
    }
}

public static class ParrySystem {
    public static void RegisterStartParry(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.RightPressed()) {

                foreach(int e in w.GetMatchingEntities([typeof(Parrier), typeof(Active)])) {

                    Parrier p = w.GetComponent<Parrier>(e); 
                    if (p.CanParry(w.Time)) {
                        p.StartParry(w.Time); 

                        w.GetComponent<Background>(e).BackgroundColor = Color.Yellow; 

                        Frame f = w.GetComponent<Frame>(e); 
                        float pbWidth = f.GetWidth(); 
                        float pbHeight = pbWidth / 3f; 
                        
                        int pbEnt = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight);
                        w.SetComponent<Label>(pbEnt, new Label(e)); 
                        w.SetComponent<ParryCooldownBar>(pbEnt, new ParryCooldownBar(p)); 
                    }
                }
            }
        });
    }

    public static void RegisterEndParry(World w) {
        w.AddSystem([typeof(Parrier), typeof(Background), typeof(Active)], (w, e) => {
            Parrier p = w.GetComponent<Parrier>(e); 
            if (p.ParryEnded(w.Time)) {
                w.GetComponent<Background>(e).BackgroundColor = Color.White;
            }
        });
    }
}