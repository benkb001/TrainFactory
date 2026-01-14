namespace TrainGame.Components; 

public class EnterInterfaceInteractable<T> where T : IInterfaceData {
    public readonly T Data; 
    public EnterInterfaceInteractable(T Data) {
        this.Data = Data; 
    }
}