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

public static class ExitExpiredTrainMenuSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(TrainEmbarkedMessage)], (w, e) => {
            Train t = w.GetComponent<TrainEmbarkedMessage>(e).GetTrain();
            if (SceneSystem.GetMenuEntities(w).Any(e => t.Equals(w.GetComponent<Menu>(e).GetTrain()))) {
                CloseMenuSystem.AddMessage(w); 
            }
            w.RemoveEntity(e); 
        });
    }
}