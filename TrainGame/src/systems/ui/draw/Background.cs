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
//TODO: Test
//required order: 
// make draw-bg message -> push -> draw bg  
public class DrawBackgroundSystem() {
    public static Type[] ts = [typeof(DrawBackgroundMessage)]; 
    public static Action<World, int> tf = (w, e) => {
        int bgEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(bgEntity, new Frame(Vector2.Zero, w.ScreenWidth, w.ScreenHeight)); 
        w.SetComponent<Background>(bgEntity, new Background(Colors.BG)); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf);
    }
}