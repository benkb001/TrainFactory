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
using TrainGame.Constants; 

//TODO: Test
public static class OpenMapSystem {
    private static Action<World> update = (w) => {
        if (VirtualKeyboard.IsClicked(KeyBinds.OpenMap) && SceneSystem.CanExitScene(w)) {
            int dm = EntityFactory.Add(w); 
            w.SetComponent<DrawMapMessage>(dm, DrawMapMessage.Get());
        }
        
    }; 
    
    public static void Register(World w) {
        w.AddSystem(update); 
    }
}