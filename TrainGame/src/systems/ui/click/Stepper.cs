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
    
    
    public static void Register(World w) {
        Action<World, int, StepperButton> tf = (w, e, sb) => {
            int message = EntityFactory.Add(w); 
            w.SetComponent<StepperMessage>(message, new StepperMessage(sb.Entity, sb.Delta)); 
        };

        ClickAndHoldSystem.Register<StepperButton>(w, tf);
    }
}