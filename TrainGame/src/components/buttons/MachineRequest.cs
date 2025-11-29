namespace TrainGame.Components; 

public class MachineRequestButton {
    private Machine machine; 
    private int stepperEntity; 
    
    public MachineRequestButton(Machine machine, int entity) {
        this.machine = machine; 
        this.stepperEntity = entity; 
    }

    public Machine GetMachine() {
        return this.machine; 
    }

    public int GetStepperEntity() {
        return stepperEntity; 
    }
}