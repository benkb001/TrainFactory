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

public class CraftProgressBarUpdateSystem() {
    private static Type[] ts = [typeof(ProgressBar), typeof(Backgrounds), typeof(Machine), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        ProgressBar pb = w.GetComponent<ProgressBar>(e); 
        Machine m = w.GetComponent<Machine>(e);
        pb.Completion = m.Completion; 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}