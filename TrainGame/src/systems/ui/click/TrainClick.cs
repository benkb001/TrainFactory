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
// trainClick -> push -> drawTrainDetail 
public class TrainClickSystem() {
    public static void Register(World world) {
        ClickSystem.Register<TrainUI>(world, (w, e) => {
            //TODO:
            //if moving draw a view detailing the train's inventory, fuel, etc 
            //actually should probably make a different component for that

            Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
            if (!t.IsTraveling()) {
                int dm = EntityFactory.Add(w, setScene: false); 
                w.SetComponent<DrawTrainInterfaceMessage>(dm, new DrawTrainInterfaceMessage(t)); 
            }
        });
    }
}