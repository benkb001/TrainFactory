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
                List<int> es = w.GetMatchingEntities([typeof(Player), 
                typeof(CardinalMovement), typeof(Parrier), typeof(Background), typeof(Body), typeof(Active)]);

                foreach(int e in es) {
                    Parrier p = w.GetComponent<Parrier>(e); 
                    if (p.HP > 0 && !p.Parrying) {
                        p.Parrying = true;
                        w.GetComponent<CardinalMovement>(e).Speed = Constants.ParryingSpeed;

                        //ICKY: This should be in a separate ui system
                        w.GetComponent<Background>(e).BackgroundColor = Color.Yellow; 

                        Frame f = w.GetComponent<Frame>(e); 
                        float pbWidth = f.GetWidth(); 
                        float pbHeight = pbWidth / 3f; 
                        
                        int pbEnt = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight);
                        w.SetComponent<ParryHPBar>(pbEnt, new ParryHPBar(p)); 

                        int labelEnt = w.GetComponent<Body>(e).LabelEntity; 
                        (LinearLayout ll, bool hasLL) = w.GetComponentSafe<LinearLayout>(labelEnt); 
                        if (hasLL) {
                            ll.AddChild(pbEnt);
                        }
                    }
                }
            }
        });
    }

    public static void RegisterEndParry(World w) {
        w.AddSystem([typeof(Player), typeof(Parrier), 
        typeof(CardinalMovement), typeof(Background), typeof(Active), typeof(Body)], (w, e) => {
            Parrier p = w.GetComponent<Parrier>(e);
            if (p.HP < 1 || !VirtualMouse.RightPressed()) {
                p.Parrying = false; 
                w.GetComponent<CardinalMovement>(e).Speed = Constants.PlayerSpeed;
                //ICKY: Should be in separate ui system
                w.GetComponent<Background>(e).BackgroundColor = Color.White;
                int labelEnt = w.GetComponent<Body>(e).LabelEntity; 
                (LinearLayout ll, bool hasLL) = w.GetComponentSafe<LinearLayout>(labelEnt); 
                List<int> childrenToRemove = new();
                if (hasLL) {
                    foreach (int childEnt in ll.GetChildren()) {
                        if (w.ComponentContainsEntity<ParryHPBar>(childEnt)) {
                            childrenToRemove.Add(childEnt);
                        }
                    }

                    foreach (int childEnt in childrenToRemove) {
                        ll.RemoveChild(childEnt); 
                        w.RemoveEntity(childEnt);
                    }
                }
            }
        });
    }
}