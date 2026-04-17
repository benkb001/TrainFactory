namespace TrainGame.Components; 

public class DrawInterfaceMessage<T> where T : IInterfaceData {
    public readonly T Data; 
    public DrawInterfaceMessage(T Data) {
        this.Data = Data; 
    }
}