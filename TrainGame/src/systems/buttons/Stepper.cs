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

    public static void Register(World world) {
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                StepperButton sb = w.GetComponent<StepperButton>(e); 
                int message = w.AddEntity(); 
                w.SetComponent<StepperMessage>(message, new StepperMessage(sb.Entity, sb.Delta)); 
            }
        };

        world.AddSystem(ts, tf); 
    }
}