namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 
using TrainGame.Components; 

public class DrawCallbackSystem {
    private static Type[] ts = [typeof(DrawCallback)]; 
    private static Action<World, int> tf = (w, e) => {
        DrawCallback cb = w.GetComponent<DrawCallback>(e); 
        cb.Run();
        w.RemoveEntity(e); 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}