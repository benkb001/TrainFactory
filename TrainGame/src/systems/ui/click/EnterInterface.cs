namespace TrainGame.Systems; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 

public static class EnterInterfaceClickSystem {
    public static void Register<T>(World w) {
        ClickSystem.Register<EnterInterfaceButton<T>>(w, (w, e) => {
            T data = w.GetComponent<EnterInterfaceButton<T>>(e).Data; 
            DrawInterfaceMessage<T> dm = new DrawInterfaceMessage<T>(data); 
            MakeMessage.Add<DrawInterfaceMessage<T>>(w, dm); 
        });
    }
}