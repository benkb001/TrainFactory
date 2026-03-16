namespace TrainGame.Components; 

public class DrawInterfaceMessage<T> {
    public readonly T Data; 
    public DrawInterfaceMessage(T Data) {
        this.Data = Data; 
    }
}