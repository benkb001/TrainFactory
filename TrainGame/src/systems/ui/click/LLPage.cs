namespace TrainGame.Systems;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public static class LLPageSystem {
    public static void Register(World w) {
        ClickSystem.Register<LLPageButton>(w, (w, e) => {
            LLPageButton pb = w.GetComponent<LLPageButton>(e);
            LinearLayout ll = pb.LL; 
            ll.Page(pb.Delta);
        }); 
    }
}