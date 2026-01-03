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
        if (VirtualKeyboard.IsClicked(KeyBinds.OpenMap)) {
            if (w.GetMatchingEntities([typeof(Menu), typeof(Active)]).Count == 0) {
                int dm = EntityFactory.Add(w); 
                w.SetComponent<DrawMapMessage>(dm, DrawMapMessage.Get());
            } 
        }
        
    }; 
    
    public static void Register(World w) {
        w.AddSystem(update); 
    }
}