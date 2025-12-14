namespace TrainGame.Utils;

using System;
using TrainGame.Components; 
using TrainGame.ECS; 

public class PopFactory {
    public static void Build(World w, int scene = 0, bool late = false, int delay = 0) {
        int e = EntityFactory.Add(w, setScene: false); 
        if (scene != 0 || late) {
            w.SetComponent<PopLateMessage>(e, new PopLateMessage(scene, delay)); 
        } else {
            w.SetComponent<PopSceneMessage>(e, PopSceneMessage.Get()); 
        }
    }
}