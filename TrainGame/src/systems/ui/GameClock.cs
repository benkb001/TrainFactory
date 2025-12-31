namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class GameClockViewSystem() {
    private static Type[] types = [typeof(TextBox), typeof(GameClockView), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        w.GetComponent<TextBox>(e).Text = w.Time.ToString(); 
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}