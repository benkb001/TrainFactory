namespace TrainGame.Systems; 

using System;
using TrainGame.Components; 
using TrainGame.ECS; 

public class TALExecutionSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Train), typeof(Data)], (w, e) => { 
            Train t = w.GetComponent<Train>(e); 
            if (t.Executable != null) {
                t.Executable.Execute(w); 
            }
        });
    }
}
