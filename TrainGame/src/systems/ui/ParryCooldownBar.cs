namespace TrainGame.Systems;

using System.Collections.Generic; 

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class ParryCooldownBarSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ParryCooldownBar), typeof(ProgressBar), typeof(Active)], (w, e) => {
            ParryCooldownBar cb = w.GetComponent<ParryCooldownBar>(e); 
            ProgressBar pb = w.GetComponent<ProgressBar>(e); 
            pb.Completion = cb.Completion(w.Time); 
            if (pb.Completion >= 1f) {
                w.RemoveEntity(e); 
            }
        });
    }
}