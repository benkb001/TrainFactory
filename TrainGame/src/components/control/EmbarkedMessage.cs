namespace TrainGame.Components;

public class EmbarkedMessage {
    private Train t; 
    public Train GetTrain() => t; 

    public EmbarkedMessage(Train t) {
        this.t = t; 
    }
}