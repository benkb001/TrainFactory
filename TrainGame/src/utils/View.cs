using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color; 

using _Color = System.Drawing.Color; 
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Components; 
using TrainGame.Utils; 

public class View {
    public static int num_menus = 1; 
    public static void EnterMenu(World w) {
        int cameraReturnEntity = EntityFactory.Add(w); 
        w.SetComponent<CameraReturn>(cameraReturnEntity, 
            new CameraReturn(w.GetCameraPosition(), w.GetCameraZoom())); 
        w.SetCameraZoom(1f); 

        Vector2 cameraPosition = new Vector2(w.ScreenWidth * num_menus, w.ScreenHeight * num_menus); 
        w.SetCameraPosition(cameraPosition); 
        num_menus++; 
    }
}