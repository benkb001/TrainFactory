namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class TrainTravelSystem() {
    private static Type[] ts = [typeof(Train)]; 
    private static Action<World, int> tf = (w, e) => {
        Train t = w.GetComponent<Train>(e); 
        t.Update(w.Time); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}