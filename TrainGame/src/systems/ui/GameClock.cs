namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

//TODO: TEST, idrk how tho because time passage relies on irl so fine for now
public class GameClockViewSystem() {
    private static Type[] types = [typeof(TextBox), typeof(Frame), typeof(GameClockView)]; 
    private static Action<World, int> transformer = (w, e) => {
        double secondsPassed = w.GetSecondsPassed(); 
        w.GetComponent<TextBox>(e).Text = ((int)(secondsPassed / 10)).ToString(); 
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}