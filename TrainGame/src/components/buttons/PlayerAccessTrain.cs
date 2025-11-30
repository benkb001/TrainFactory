
namespace TrainGame.Components; 

public class PlayerAccessTrainButton {
    private Train train; 
    public PlayerAccessTrainButton(Train t) {
        this.train = t; 
    }
    public Train GetTrain() {
        return train; 
    }
    public string GetMessage() {
        if (train.HasPlayer) {
            return "Click to enter train"; 
        }
        return "Click to exit train"; 
    }
}