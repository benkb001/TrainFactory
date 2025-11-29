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

//We need to lock before we update the camera, because 
//camera update happens after all system updates
public class CameraLockSystem() {
    private static Action<World> update = (w) => {
        if (w.GetMatchingEntities([typeof(Active), typeof(Menu)]).Count > 0) {
            w.LockCamera();
        } else {
            w.UnlockCamera(); 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(update); 
    }
}