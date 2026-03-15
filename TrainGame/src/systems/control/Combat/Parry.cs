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