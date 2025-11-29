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
public class OpenMapSystem() {
    private static Action<World> update = (w) => {

    }; 
    public static void Register(World w) {
        w.AddSystem(update); 
    }
}