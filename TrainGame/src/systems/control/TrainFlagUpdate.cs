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

public class TrainFlagUpdateSystem() {
    public static void Register(World world) {
        world.AddSystem([typeof(Train), typeof(Data)], (w, e) => {
            Train t = w.GetComponent<Train>(e); 
            t.EndEmbarking(); 
        }); 
    }
}