namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public static class DrawSystem {
    public static void Register<T>(World w, Action<World, int> tf) {
        w.AddSystem([typeof(T)], (w, e) => {
            tf(w, e); 
            w.RemoveEntity(e);
        });
    }
}

public static class DrawInterfaceSystem {
    public static void Register<T>(World w, Action<World, int> tf) where T : IInterfaceData {
        DrawSystem.Register<DrawInterfaceMessage<T>>(w, (w, e) => {
            T data = w.GetComponent<DrawInterfaceMessage<T>>(e).Data; 
            SceneType s = data.GetSceneType(); 
            SceneSystem.EnterScene(w, s);
            Menu menu = data.GetMenu(); 
            int menuEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(menuEnt, menu);
            tf(w, e); 
        });
    }
}