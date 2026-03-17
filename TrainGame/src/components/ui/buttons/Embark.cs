namespace TrainGame.Components; 

public class EmbarkButton {
    private City destination; 
    private Train train; 
    public readonly int TrainEntity;

    public EmbarkButton(City dest, Train t, int TrainEntity) {
        this.destination = dest; 
        this.train = t; 
        this.TrainEntity = TrainEntity;
    }

    public Train GetTrain() {
        return train; 
    }
    
    public City GetDestination() {
        return destination; 
    }
}