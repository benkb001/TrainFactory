namespace TrainGame.Components; 

public class DrawAddCartInterfaceMessage {
    public Train CartDest; 
    public City CartSource; 

    public DrawAddCartInterfaceMessage(Train CartDest, City CartSource) {
        this.CartDest = CartDest; 
        this.CartSource = CartSource; 
    }
}