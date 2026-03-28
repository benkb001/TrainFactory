namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;

using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.Constants; 

public static class DrawVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampired), typeof(Frame), typeof(Active)], (w, e) => {
            int vampiredBy = w.GetComponent<Vampired>(e).VampiredByEntity; 
            (Frame f, bool hasFrame) = w.GetComponentSafe<Frame>(vampiredBy); 
            if (hasFrame) {
                Vector2 vampirePos = f.Position; 
                Vector2 pos = w.GetComponent<Frame>(e).Position;

                Lines ls = new Lines(); 
                
                ls.AddLine(pos, vampirePos, Colors.Vampiric);
                w.SetComponent<Lines>(e, ls);
            }
        });
    }
}

public static class StopDrawingVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(EndVampired), typeof(Lines), typeof(Active)], (w, e) => {
            w.RemoveComponent<Lines>(e);
        });
    }
}
