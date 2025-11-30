namespace TrainGame.Components; 

public class EmbarkButton {
    private City destination; 
    private Train train; 

    public EmbarkButton(City dest, Train t) {
        this.destination = dest; 
        this.train = t; 
    }

    public Train GetTrain() {
        return train; 
    }
    
    public City GetDestination() {
        return destination; 
    }
}