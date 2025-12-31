
namespace TrainGame.Components; 

public class DrawTravelingInterfaceMessage {
    private Train train; 

    public DrawTravelingInterfaceMessage(Train t) {
        this.train = t; 
    }

    public Train GetTrain() {
        return train; 
    }
}