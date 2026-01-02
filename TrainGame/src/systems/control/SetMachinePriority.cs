namespace TrainGame.Systems;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils;
using TrainGame.Constants;

public static class SetMachinePrioritySystem {
    public static void Register(World w) {
        w.AddSystem([typeof(MachinePriorityStepper), typeof(Button), typeof(Active)], (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                MachinePriorityStepper prioStep = w.GetComponent<MachinePriorityStepper>(e); 
                Stepper step = prioStep.GetStepper(); 
                Machine m = prioStep.GetMachine(); 
                int priority = step.Value;
                m.SetPriority(priority); 
            }
        });
    }
}

