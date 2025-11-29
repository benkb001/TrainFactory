namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class MachineRequestClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(MachineRequestButton), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                int popEntity = EntityFactory.Add(w); 
                w.SetComponent<PopSceneMessage>(popEntity, PopSceneMessage.Get()); 
                MachineRequestButton mb = w.GetComponent<MachineRequestButton>(e); 
                Machine m = mb.GetMachine();
                int count = w.GetComponent<Stepper>(mb.GetStepperEntity()).Value;
                m.Request(count); 
            }
        };

        world.AddSystem(ts, tf); 
    }

}