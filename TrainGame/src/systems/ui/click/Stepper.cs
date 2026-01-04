namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

public class StepperButtonSystem() {
    private static Type[] ts = [typeof(StepperButton), typeof(Button), typeof(Active)]; 
    public static int NumTicksBeforeFast = 30; 
    public static int NumTicksPerFastUpdate = 2; 
    
    public static void Register(World world) {
        Action<World, int> tf = (w, e) => {
            Button b = w.GetComponent<Button>(e); 
            if (b.Clicked || (b.TicksHeld >= NumTicksBeforeFast && b.TicksHeld % NumTicksPerFastUpdate == 0)) {
                StepperButton sb = w.GetComponent<StepperButton>(e); 
                int message = EntityFactory.Add(w); 
                w.SetComponent<StepperMessage>(message, new StepperMessage(sb.Entity, sb.Delta)); 
            }
        };

        world.AddSystem(ts, tf); 
    }
}