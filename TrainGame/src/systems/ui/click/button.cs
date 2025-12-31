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
            Click type = Click.None; 

            if (VirtualMouse.RightClicked()) {
                type = Click.Right; 
            }

            if (VirtualMouse.LeftClicked()) {
                if (VirtualKeyboard.IsPressed(Keys.LeftShift)) {
                    type = Click.Shift; 
                } else {
                    type = Click.Left; 
                }
            }

            if (type == Click.None) {
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
                (Frame f, Button b) = frameButtons[i]; 

                if (f.Contains(mousePoint)) {
                    b.ClickType = type; 
                    b.TicksHeld++;
                    b.OnClick(); 
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
            Button b = w.GetComponent<Button>(e);
            b.ClickType = Click.None; 
        };

        world.AddSystem(types, transformer);
    }

    public static void RegisterHold(World world) {
        world.AddSystem((w) => {
            Vector2 mousePoint = w.GetWorldMouseCoordinates(); 
            KeyValuePair<int, Button> held = w.GetComponentArray<Button>().Where(
                kvp => kvp.Value.TicksHeld > 0).FirstOrDefault(); 
            if (!held.Equals(default(KeyValuePair<int, Button>))) {
                int heldEnt = held.Key; 
                Button heldBtn = held.Value; 

                (Frame f, bool success) = w.GetComponentSafe<Frame>(heldEnt);
                if (success && f.Contains(mousePoint) && VirtualMouse.LeftPressed()) {
                    heldBtn.TicksHeld++; 
                } else {
                    heldBtn.TicksHeld = 0;
                }
            }
        }); 
    }
}