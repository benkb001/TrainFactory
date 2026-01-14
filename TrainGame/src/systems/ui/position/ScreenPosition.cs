namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.ECS;

public class ScreenAnchorSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ScreenAnchor), typeof(Frame), typeof(Active)], (w, e) => {
            Vector2 screenPosition = w.GetComponent<ScreenAnchor>(e).Position; 
            Vector2 pos = w.GetCameraTopLeft() + screenPosition;
            Console.WriteLine($"Screen anchor postiion: {pos}");
            w.GetComponent<Frame>(e).SetCoordinates(pos);
        });
    }
}