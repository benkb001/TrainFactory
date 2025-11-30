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
    private static Type[] ts = [typeof(Train), typeof(Data)]; 
    private static Action<World, int> tf = (w, e) => {
        Train t = w.GetComponent<Train>(e); 
        if (t.IsArriving(w.Time) && t.HasPlayer) {
            int msg = EntityFactory.Add(w); 
            w.SetComponent<DrawCityMessage>(msg, new DrawCityMessage(t.GoingTo)); 
        }
        t.Update(w.Time); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}