namespace TrainGame.Components; 

public class DrawSetTrainProgramInterfaceMessage {
    private Train train; 
    public Train GetTrain() => train; 

    public DrawSetTrainProgramInterfaceMessage(Train train) {
        this.train = train; 
    }
}