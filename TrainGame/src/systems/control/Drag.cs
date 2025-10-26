namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class DragSystem() {
    private static Type[] types = [typeof(Draggable), typeof(Frame), typeof(Button), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        Button b = w.GetComponent<Button>(e); 
        Draggable draggable = w.GetComponent<Draggable>(e); 
        Frame f = w.GetComponent<Frame>(e); 

        if (b.Clicked) {
            draggable.PickUp(); 
            draggable.SnapPosition = f.Position; 
            draggable.RelativeClickPosition = w.GetWorldMouseCoordinates() - f.Position; 
        }

        if (draggable.IsHeld()) {
            f.SetCoordinates(w.GetWorldMouseCoordinates() - draggable.RelativeClickPosition); 
        }

        if (!VirtualMouse.LeftPressed() && draggable.IsHeld()) {
            draggable.Release(); 
            f.SetCoordinates(draggable.SnapPosition); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}