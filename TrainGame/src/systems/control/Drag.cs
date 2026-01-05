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

public class DragSystem {
    private static Type[] types = [typeof(Draggable), typeof(Frame), typeof(Button), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        Button b = w.GetComponent<Button>(e); 
        Draggable draggable = w.GetComponent<Draggable>(e); 
        Frame f = w.GetComponent<Frame>(e); 
        
        //TODO: test
        if (draggable.IsBeingPickedUp()) {
            draggable.Hold(); 
        }

        if (draggable.IsReleased() && b.TicksHeld == 1 && !w.GetComponentArray<Draggable>().Any(kvp => !kvp.Value.IsReleased())) {
            draggable.PickUp(); 
            draggable.SnapPosition = f.Position; 
            draggable.RelativeClickPosition = w.GetWorldMouseCoordinates() - f.Position; 
        }

        if (draggable.IsHeld()) {
            f.SetCoordinates(w.GetWorldMouseCoordinates() - draggable.RelativeClickPosition); 
        }

        if (draggable.IsBeingDropped()) {
            draggable.Release(); 
            f.SetCoordinates(draggable.SnapPosition); 
        }

        if (!VirtualMouse.LeftPressed() && draggable.IsHeld()) {
            draggable.Drop(); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}