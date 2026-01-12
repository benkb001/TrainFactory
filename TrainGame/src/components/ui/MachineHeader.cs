namespace TrainGame.Components; 

public class MachineHeader {
    private Machine machine; 
    public Machine GetMachine() => machine;

    public MachineHeader(Machine machine) {
        this.machine = machine; 
    }
}