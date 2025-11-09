namespace TrainGame.Components; 

public class TrainUI {
    private Train train; 
    public static float Width = 50f; 
    public static float Height = 50f; 

    public TrainUI(Train train) {
        this.train = train; 
    }

    public Train GetTrain() {
        return train; 
    }
}