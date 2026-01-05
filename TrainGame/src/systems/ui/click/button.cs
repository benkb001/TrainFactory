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

public static class ButtonSystem {

    public static void RegisterClick(World world) {
        Action<World> update = (w) => {

            Vector2 mousePoint = w.GetWorldMouseCoordinates(); 

            List<int> es = w.GetMatchingEntities([typeof(Button), typeof(Frame), typeof(Active)]).OrderBy(
                e => w.GetComponent<Button>(e).Depth).Where(e => w.GetComponent<Frame>(e).Contains(mousePoint)).ToList(); 
            
            if (es.Count > 0) {
                int e = es[0]; 
                Button b = w.GetComponent<Button>(e); 
                b.Hovered = true; 

                if (VirtualMouse.LeftPressed()) {
                    b.TicksHeld++;
                }

                if (VirtualMouse.RightPressed()) {
                    b.RightTicksHeld++; 
                }
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
                kvp => kvp.Value.TicksHeld > 0 || kvp.Value.RightTicksHeld > 0).FirstOrDefault(); 
            if (!held.Equals(default(KeyValuePair<int, Button>))) {
                int heldEnt = held.Key; 
                Button heldBtn = held.Value; 

                (Frame f, bool success) = w.GetComponentSafe<Frame>(heldEnt);
                if (success) {

                    if (!f.Contains(mousePoint)) {
                        heldBtn.TicksHeld = 0; 
                        heldBtn.RightTicksHeld = 0;
                    } else {
                        Click type = Click.None; 

                        if (heldBtn.RightTicksHeld > 0 && !VirtualMouse.RightPressed()) {
                            type = Click.Right; 
                            heldBtn.RightTicksHeld = 0; 
                        }

                        if (heldBtn.TicksHeld > 0 && !VirtualMouse.LeftPressed()) {
                            if (VirtualKeyboard.IsPressed(Keys.LeftShift)) {
                                type = Click.Shift; 
                            } else {
                                type = Click.Left; 
                                
                            }
                            heldBtn.TicksHeld = 0; 
                        }

                        heldBtn.ClickType = type; 
                    } 
                }
            }
        }); 
    }

    public static void RegisterClearHovered(World w) {
        w.AddSystem([typeof(Button), typeof(Active)], (w, e) => {
            w.GetComponent<Button>(e).Hovered = false; 
        });
    }

    public static void RegisterHighlight(World w) {
        w.AddSystem([typeof(Button), typeof(Active), typeof(Outline)], (w, e) => {
            Outline o =  w.GetComponent<Outline>(e);
            Button b = w.GetComponent<Button>(e);
            
            if (b.TicksHeld > 0 || b.RightTicksHeld > 0) {
                o.SetThickness(Constants.ButtonHeldOutlineThickness);
            } else if (b.Hovered) {
               o.SetThickness(Constants.ButtonHoveredOutlineThickness); 
            } else {
                o.SetThickness(Constants.ButtonOutlineThickness); 
            }
        }); 
    }
}