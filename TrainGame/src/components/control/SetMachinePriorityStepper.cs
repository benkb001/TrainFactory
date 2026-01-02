namespace TrainGame.Components;

public class MachinePriorityStepper {
    private Machine machine;
    private Stepper step; 

    public Machine GetMachine() => machine; 
    public Stepper GetStepper() => step; 
    public MachinePriorityStepper(Machine machine, Stepper step) {
        this.machine = machine; 
        this.step = step; 
    }
}