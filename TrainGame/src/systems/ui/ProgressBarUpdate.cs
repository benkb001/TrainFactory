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

//todo: test
public class ProgressBarUpdateSystem() {
    private static Type[] ts = [typeof(ProgressBar), typeof(Backgrounds), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        ProgressBar pb = w.GetComponent<ProgressBar>(e); 
        Backgrounds bs = w.GetComponent<Backgrounds>(e);
        (Background b, Frame f) = bs.Ls[1]; 
        bs.Ls[1] = (b, new Frame(f.Position, pb.Completion * pb.MaxWidth, f.GetHeight())); 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }

    public static void RegisterPosition(World w) {
        w.AddSystem([typeof(ProgressBar), typeof(Frame), typeof(Backgrounds), typeof(Active)], (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            foreach ((Background _, Frame bgFrame) in w.GetComponent<Backgrounds>(e).Ls) {
                bgFrame.SetCoordinates(f.Position); 
            }
        });
    }
}