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

    //TODO: test bubbling
    public static void RegisterClick(World world) {
        Action<World> update = (w) => {
            if (!VirtualMouse.LeftClicked()) {
                return; 
            }

            Vector2 mousePoint = world.GetWorldMouseCoordinates(); 

            List<int> buttonEntities = w.GetMatchingEntities([typeof(Button), typeof(Frame), typeof(Active)]);
            List<(Frame, Button)> frameButtons = buttonEntities.Select(e => {
                Frame f = w.GetComponent<Frame>(e); 
                Button b = w.GetComponent<Button>(e); 
                return (f, b); 
            }).OrderBy(pair => pair.Item2.Depth).ToList(); 

            int i = 0; 
            bool clicked = false; 

            while(i < frameButtons.Count && !clicked) {
                if (frameButtons[i].Item1.Contains(mousePoint)) {
                    frameButtons[i].Item2.Clicked = true; 
                    frameButtons[i].Item2.OnClick(); 
                    clicked = true; 
                }
                i++; 
            }
        }; 

        world.AddSystem(update); 
    }

    public static void RegisterUnclick(World world) {
        Type[] types = [typeof(Button), typeof(Active)]; 
        Action<World, int> transformer = (w, e) => {
            w.GetComponent<Button>(e).Clicked = false; 
        };

        world.AddSystem(types, transformer);
    }
}