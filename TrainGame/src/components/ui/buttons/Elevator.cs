namespace TrainGame.Components;

public class ElevatorButton {
    private Stepper step; 
    public int Floor => step.Value;

    public ElevatorButton(Stepper step) {
        this.step = step; 
    }
}