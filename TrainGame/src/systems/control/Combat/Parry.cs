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

public class Parrier {
    private WorldTime canParry; 
    private WorldTime cooldown; 
    private WorldTime duration; 
    private WorldTime parryDecrease; 
    private WorldTime endParry; 
    private bool parrying = false; 
    public bool Parrying => parrying; 

    public Parrier() {
        canParry = new WorldTime(); 
        endParry = new WorldTime(); 
        cooldown = new WorldTime(minutes: 3); 
        duration = new WorldTime(minutes: 1); 
        parryDecrease = new WorldTime(ticks: 30); 
    }

    public bool CanParry(WorldTime now) {
        return now.IsAfterOrAt(canParry); 
    }

    public void StartParry(WorldTime now) {
        canParry = now + cooldown; 
        endParry = now + duration;
        parrying = true; 
    }

    public void Parry() {
        canParry = canParry - parryDecrease; 
    }

    public void EndParry(WorldTime now) {
        if (now.IsAfterOrAt(endParry)) {
            parrying = false; 
        }
    }
}

public static class ParrySystem {
    public static void RegisterStartParry(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.RightPushed()) {
                foreach(KeyValuePair<int, Parrier> kvp in w.GetComponentArray<Parrier>()) {
                    Parrier p = kvp.Value; 
                    if (p.CanParry(w.Time)) {
                        p.StartParry(w.Time); 
                    }
                }
            }
        });
    }

    public static void RegisterEndParry(World w) {
        w.AddSystem([typeof(Parrier), typeof(Active)], (w, e) => {
            w.GetComponent<Parrier>(e).EndParry(w.Time); 
        });
    }
}