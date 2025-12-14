namespace TrainGame.Utils; 

using TrainGame.Components; 
using TrainGame.ECS; 

public class PushFactory {
    public static void Build(World w) {
        int e = EntityFactory.Add(w, setScene: false); 
        w.SetComponent<PushSceneMessage>(e, PushSceneMessage.Get()); 
    }
}