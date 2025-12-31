namespace TrainGame.Systems; 

using System;
using TrainGame.Components; 
using TrainGame.ECS; 

public class TALExecutionSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(TALBody), typeof(Data)], (w, e) => { 
            TALBody body = w.GetComponent<TALBody>(e); 
            body.Execute(w); 
        });
    }
}
