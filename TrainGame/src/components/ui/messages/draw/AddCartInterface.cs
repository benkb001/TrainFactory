namespace TrainGame.Components; 

public class DrawAddCartInterfaceMessage {
    public Train CartDest; 
    public City CartSource; 
    public readonly int TrainEntity;

    public DrawAddCartInterfaceMessage(Train CartDest, City CartSource, int TrainEntity) {
        this.CartDest = CartDest; 
        this.CartSource = CartSource; 
        this.TrainEntity = TrainEntity;
    }
}