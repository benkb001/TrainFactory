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

public static class ClickSystem {
    public static void Register<T>(World w, Action<World, int> onClick) {
        w.AddSystem(
            [typeof(Button), typeof(Active), typeof(T)], 
            (w, e) => {
                if (w.GetComponent<Button>(e).Clicked) {
                    onClick(w, e); 
                }
            }
        ); 
    }
}