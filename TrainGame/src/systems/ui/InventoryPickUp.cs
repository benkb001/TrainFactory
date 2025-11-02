namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

//TODO: test
public class InventoryPickUpUISystem() {
    public const float Threshold = 30f; 
    private static Type[] types = [
        typeof(Frame), 
        typeof(Outline), 
        typeof(TextBox), 
        typeof(Background),
        typeof(Draggable), 
        typeof(Inventory.Item), 
        typeof(Active)
    ]; 

    private static Action<World, int> transformer = (w, e) => {
        Draggable d = w.GetComponent<Draggable>(e); 
        if (d.IsBeingPickedUp()) {
            w.GetComponent<TextBox>(e).Depth = Constants.InventoryHeldTextBoxDepth; 
            w.GetComponent<Background>(e).Depth = Constants.InventoryHeldBackgroundDepth; 
            w.GetComponent<Outline>(e).Depth = Constants.InventoryHeldOutlineDepth; 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(types, transformer); 
    }
}