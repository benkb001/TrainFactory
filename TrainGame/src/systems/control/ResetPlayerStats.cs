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