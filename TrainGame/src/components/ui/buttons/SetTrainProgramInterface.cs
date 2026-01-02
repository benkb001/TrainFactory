namespace TrainGame.Components; 

public class SetTrainProgramInterfaceButton {
    private Train train; 
    public Train GetTrain() => train;

    public SetTrainProgramInterfaceButton(Train train) {
        this.train = train; 
    }
}