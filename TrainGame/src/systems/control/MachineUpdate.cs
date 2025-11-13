namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class MachineUpdateSystem() {
    private static Type[] ts = [typeof(Machine), typeof(Data)]; 
    private static Action<World, int> tf = (w, e) => {
        w.GetComponent<Machine>(e).Update(); 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}