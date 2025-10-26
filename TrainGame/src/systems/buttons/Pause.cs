namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

public class PauseButtonSystem() {
    private static Type[] ts = [typeof(PauseButton), typeof(Button), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        if (w.GetComponent<Button>(e).Clicked) {
            int msg = w.AddEntity(); 
            w.SetComponent<PushSceneMessage>(msg, PushSceneMessage.Get()); 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}

public class UnpauseButtonSystem() {
    private static Type[] ts = [typeof(UnpauseButton), typeof(Button), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        if (w.GetComponent<Button>(e).Clicked) {
            int msg = w.AddEntity(); 
            w.SetComponent<PopSceneMessage>(msg, PopSceneMessage.Get()); 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}
