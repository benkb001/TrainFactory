namespace TrainGame.Components; 

public class TrainUI {
    private Train train; 
    public static float Width = 50f; 
    public static float Height = 50f; 
    public readonly int TrainEntity;

    public TrainUI(Train train, int TrainEntity) {
        this.train = train; 
        this.TrainEntity = TrainEntity;
    }

    public Train GetTrain() {
        return train; 
    }
}