namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

//button holds what the current test is
public class NextDrawTestButtonSystem() {
    private static Type[] ts = [typeof(NextDrawTestButton), typeof(Button), typeof(Active)]; 

    public static void Register(World world) {
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                int curTest = w.GetComponent<NextDrawTestButton>(e).GetCurTest(); 
                int entity = w.AddEntity(); 
                w.SetComponent<NextDrawTestControl>(entity, new NextDrawTestControl(++curTest)); 
            }
        };

        world.AddSystem(ts, tf); 
    }
}