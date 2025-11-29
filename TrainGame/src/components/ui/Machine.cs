namespace TrainGame.Components; 

public class MachineUI {

    private Machine machine;

    public MachineUI(Machine machine) {
        this.machine = machine; 
    }
    
    public Machine GetMachine() {
        return machine; 
    }
}