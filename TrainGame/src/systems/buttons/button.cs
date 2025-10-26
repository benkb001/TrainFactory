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

public class ButtonSystem() {
    private static Type[] types = [typeof(Button), typeof(Frame), typeof(Active)]; 

    //TODO: test bubbling
    public static void RegisterClick(World world) {
        Action<World> update = (w) => {
            Vector2 mousePoint = world.GetWorldMouseCoordinates(); 
            MouseState curMouse = VirtualMouse.GetState(); 

            if (!VirtualMouse.LeftClicked()) {
                return; 
            }

            //TODO: Convert to while loop/make more readable
            foreach (KeyValuePair<int, Button> entry in 
                w.GetComponentArray<Button>().OrderBy(pair => pair.Value.Depth)) {
                Frame f = w.GetComponent<Frame>(entry.Key);
                if (f.Contains(mousePoint)) {
                    entry.Value.Clicked = true; 
                    return; 
                }
            }
        }; 

        world.AddSystem(types, update); 
    }

    public static void RegisterUnclick(World world) {
        Action<World, int> transformer = (w, e) => {
            w.GetComponent<Button>(e).Clicked = false; 
        };

        world.AddSystem(types, transformer);
    }
}