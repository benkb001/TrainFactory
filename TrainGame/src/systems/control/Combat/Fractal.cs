namespace TrainGame.Systems;

using System.Collections.Generic;
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class FractalSplitSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Fractal), typeof(Active), typeof(Bullet)], (w, e) => {
            float c = w.GetComponent<Fractal>(e).SplitChancePerFrame; 
            float rand = Util.NextFloat();
            if (rand < c) {
                w.SetComponent<FractalSplitFlag>(e, new FractalSplitFlag());
            }
        });
    }

    public static void RegisterRemoveFlag(World w) {
        w.AddSystem([typeof(FractalSplitFlag), typeof(Active)], (w, e) => {
            w.RemoveComponent<FractalSplitFlag>(e);
        });
    }
}