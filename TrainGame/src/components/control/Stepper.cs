namespace TrainGame.Components;

public class StepperMessage {
    public int Entity; 
    public int Delta; 

    public StepperMessage(int e, int d) {
        Entity = e; 
        Delta = d; 
    }
}

public class Stepper {
    public int Value = 0;

    public Stepper(int v) {
        Value = v; 
    }
}