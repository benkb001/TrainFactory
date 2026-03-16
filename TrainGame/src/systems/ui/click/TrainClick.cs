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
using TrainGame.Callbacks; 

//required order: 
// trainClick -> drawTrainDetail 
public static class TrainClickSystem {
    public static void Register(World world) {
        ClickSystem.Register<TrainUI>(world, (w, e, btn) => {

            Train t = btn.GetTrain(); 
            if (!t.IsTraveling()) {
                int dm = EntityFactory.Add(w, setScene: false); 
                w.SetComponent<DrawTrainInterfaceMessage>(dm, new DrawTrainInterfaceMessage(t, btn.TrainEntity)); 
            } else {
                (TALBody<Train, City> exe, bool _) = w.GetComponentSafe<TALBody<Train, City>>(btn.TrainEntity);
                DrawTravelingInterfaceSystem.AddMessage(w, t, btn.TrainEntity); 
            }
        });
    }
}