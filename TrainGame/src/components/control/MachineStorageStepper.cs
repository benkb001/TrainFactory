namespace TrainGame.Components;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils;
using TrainGame.Constants;

public class MachineStorageStepper {
    private Machine machine;
    private Stepper step; 

    public Machine GetMachine() => machine; 
    public Stepper GetStepper() => step; 
    public MachineStorageStepper(Machine machine, Stepper step) {
        this.machine = machine; 
        this.step = step; 
    }
}