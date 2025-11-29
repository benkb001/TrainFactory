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
public class CameraReturnSystem() {
    private static Type[] ts = [typeof(CameraReturn), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        CameraReturn cr = w.GetComponent<CameraReturn>(e);
        w.SetCameraPosition(cr.Position);
        w.SetCameraZoom(cr.Zoom); 
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}