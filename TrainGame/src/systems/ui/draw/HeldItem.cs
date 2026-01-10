namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class HeldItemDrawSystem() {
    private static Type[] ts = [typeof(HeldItem), typeof(Frame), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        HeldItem held = w.GetComponent<HeldItem>(e);

        int rowEntity = w.GetComponent<LinearLayout>(held.InventoryEntity).GetChildren()[0]; 
        List<int> cells = w.GetComponent<LinearLayout>(rowEntity).GetChildren(); 

        w.GetComponent<Outline>(cells[held.InvIndex]).SetColor(Colors.InventoryNotHeld); 
        if (VirtualMouse.IsScrollingDown()) {
            int index = held.InvIndex - 1; 
            if (index < 0) {
                index = held.InvSize - 1; 
            }
            held.SetItem(index); 
        }

        if (VirtualMouse.IsScrollingUp()) {
            int index = (held.InvIndex + 1) % held.InvSize; 
            held.SetItem(index);
        }

        if (held.ItemId != "" && held.Count > 0) {
            if (!w.EntityExists(held.LabelEntity)) {
                held.LabelEntity = EntityFactory.Add(w); 
            }

            w.SetComponent<Label>(held.LabelEntity, new Label(e));
            w.SetComponent<TextBox>(held.LabelEntity, new TextBox(held.ItemId));
            w.SetComponent<Outline>(held.LabelEntity, new Outline()); 
            Frame f = w.GetComponent<Frame>(e); 
            w.SetComponent<Frame>(held.LabelEntity, new Frame(0, 0, f.GetWidth() / 2, f.GetHeight() / 2)); 
        } else {
            w.RemoveEntity(held.LabelEntity); 
        }

        w.GetComponent<Outline>(cells[held.InvIndex]).SetColor(Colors.InventoryHeld); 
    };

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}