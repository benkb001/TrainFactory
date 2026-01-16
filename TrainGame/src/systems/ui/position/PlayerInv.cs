namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.ECS;

public static class PlayerHUDPositionSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(PlayerHUD), typeof(Frame), typeof(ScreenAnchor), typeof(Active)], (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            bool overlap = w.GetMatchingEntities([typeof(Frame), typeof(Interactor), typeof(Active)]).Where(
                ent => f.IntersectsWith(w.GetComponent<Frame>(ent))).ToList().Count > 0; 
            if (overlap) {
                ScreenAnchor anchor = w.GetComponent<ScreenAnchor>(e); 
                if (anchor.Position != Vector2.Zero) {
                    anchor.Position = Vector2.Zero; 
                } else {
                    anchor.Position = new Vector2(0, w.ScreenHeight - f.GetHeight()); 
                }
            }
        });
    }
}