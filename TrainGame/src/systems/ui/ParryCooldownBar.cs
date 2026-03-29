namespace TrainGame.Systems;

using System.Collections.Generic; 

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class ParryHPBarSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ParryHPBar), typeof(ProgressBar), typeof(Active)], (w, e) => {
            ParryHPBar cb = w.GetComponent<ParryHPBar>(e); 
            ProgressBar pb = w.GetComponent<ProgressBar>(e); 
            Parrier p = cb.GetParrier();
            pb.Completion = p.HP / (float)p.MaxHP; 
        });
    }
}