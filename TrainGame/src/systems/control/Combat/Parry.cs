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
                typeof(CardinalMovement), typeof(Parrier), typeof(Background), typeof(Active)]);

                foreach(int e in es) {
                    Parrier p = w.GetComponent<Parrier>(e); 
                    if (p.HP > 0) {
                        p.Parrying = true;
                        w.GetComponent<CardinalMovement>(e).Speed = Constants.ParryingSpeed;

                        //ICKY: This should be in a separate ui system
                        w.GetComponent<Background>(e).BackgroundColor = Color.Yellow; 

                        Frame f = w.GetComponent<Frame>(e); 
                        float pbWidth = f.GetWidth(); 
                        float pbHeight = pbWidth / 3f; 
                        
                        int pbEnt = DrawProgressBarCallback.Draw(w, Vector2.Zero, pbWidth, pbHeight);
                        w.SetComponent<Label>(pbEnt, new Label(e)); 
                        w.SetComponent<ParryHPBar>(pbEnt, new ParryHPBar(p)); 
                    }
                }
            }
        });
    }

    public static void RegisterEndParry(World w) {
        w.AddSystem([typeof(Player), typeof(Parrier), 
        typeof(CardinalMovement), typeof(Background), typeof(Active)], (w, e) => {
            Parrier p = w.GetComponent<Parrier>(e);
            if (p.HP < 1 || !VirtualMouse.RightPressed()) {
                p.Parrying = false; 
                w.GetComponent<CardinalMovement>(e).Speed = Constants.PlayerSpeed;
                //ICKY: Should be in separate ui system
                w.GetComponent<Background>(e).BackgroundColor = Color.White;
                foreach (int barEnt in w.GetMatchingEntities([typeof(ParryHPBar), typeof(Active)])
                .Where(ent => w.GetComponent<Label>(ent).BodyEntity == e)) {
                    w.RemoveEntity(barEnt);
                }
            }
        });
    }
}