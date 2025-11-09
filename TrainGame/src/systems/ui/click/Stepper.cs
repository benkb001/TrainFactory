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

public static class StepperUISystem {
    private static Type[] ts = [typeof(StepperMessage)]; 

    public static void Register(World world) {
        Action<World, int> tf = (w, e) => {
            StepperMessage sm = w.GetComponent<StepperMessage>(e); 
            Stepper s = w.GetComponent<Stepper>(sm.Entity); 
            s.Value += sm.Delta; 
            w.SetComponent<TextBox>(sm.Entity, new TextBox(s.Value.ToString())); 
            w.RemoveEntity(e); 
        };

        world.AddSystem(ts, tf); 
    }
}