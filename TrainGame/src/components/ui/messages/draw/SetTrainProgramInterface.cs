namespace TrainGame.Components; 

public class DrawSetTrainProgramInterfaceMessage {
    private Train train; 
    private int trainEntity;
    public Train GetTrain() => train; 
    public int TrainEntity => trainEntity;

    public DrawSetTrainProgramInterfaceMessage(Train train, int trainEntity) {
        this.train = train; 
        this.trainEntity = trainEntity;
    }
}