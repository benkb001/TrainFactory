namespace TrainGame.Components; 

public class TrainUI {
    private Train train; 
    public static float Width = 50f; 
    public static float Height = 50f; 
    public readonly int TrainEntity;
    public readonly City ComingFrom; 
    public readonly City GoingTo;

    public TrainUI(Train train, int TrainEntity, City comingFrom, City goingTo) {
        this.train = train; 
        this.TrainEntity = TrainEntity;
        this.ComingFrom = comingFrom;
        this.GoingTo = goingTo;
    }

    public Train GetTrain() {
        return train; 
    }
}