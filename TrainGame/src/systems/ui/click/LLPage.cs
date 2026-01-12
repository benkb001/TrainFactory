namespace TrainGame.Systems;

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public static class LLPageSystem {
    public static void Register(World w) {
        ClickSystem.Register<LLPageButton>(w, (w, e) => {
            LLPageButton pb = w.GetComponent<LLPageButton>(e);
            LinearLayout ll = pb.LL; 
            ll.Page(pb.Delta);
        }); 
    }

    public static void RegisterScroll(World w) {
        w.AddSystem((w) => {
            int direction = 0; 

            if (VirtualMouse.IsScrollingDown()) {
                direction = 1; 
            } else if (VirtualMouse.IsScrollingUp()) {
                direction = -1; 
            }

            if (direction != 0) {
                Vector2 mousePoint = w.GetWorldMouseCoordinates(); 
                List<LinearLayout> lls = w.GetMatchingEntities([typeof(LinearLayout), typeof(Active), typeof(Frame)]).Where(
                    e => {
                        return w.GetComponent<LinearLayout>(e).UsePaging && 
                            w.GetComponent<Frame>(e).Contains(mousePoint); 
                }).Select(e => w.GetComponent<LinearLayout>(e)).ToList(); 

                foreach (LinearLayout ll in lls) {
                    ll.Page(direction); 
                }
            }
        });
    }
}