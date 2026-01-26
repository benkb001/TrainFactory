namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

class EmbarkedMessage {
    private Train t; 
    public Train GetTrain() => t; 

    public EmbarkedMessage(Train t) {
        this.t = t; 
    }
}

public class EmbarkClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(EmbarkButton), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                EmbarkButton eb = w.GetComponent<EmbarkButton>(e); 
                Train t = eb.GetTrain();
                City c = eb.GetDestination(); 
                TrainWrap.Embark(t, c, w); 
                MakeMessage.Add<DrawMapMessage>(w, new DrawMapMessage());
                MakeMessage.Add<EmbarkedMessage>(w, new EmbarkedMessage(t)); 
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}

public static class ResetPlayerStatsSystem {
    public static void RegisterLeftCity(World w) {
        w.AddSystem([typeof(EmbarkedMessage)], (w, e) => {
            if (w.GetComponent<EmbarkedMessage>(e).GetTrain().HasPlayer) {
                PlayerStats.Reset(w);
            }
            w.RemoveEntity(e);  
        });
    }
}