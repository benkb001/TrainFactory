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
using TrainGame.Constants; 

public static class LinearLayoutSystem {
    private static Action<World, int> tf = (w, e) => {
        LinearLayout layout = w.GetComponent<LinearLayout>(e); 
        Frame layout_frame = w.GetComponent<Frame>(e); 
        List<int> children = layout.GetChildren();
        if (layout.IsSpaceEven()) {
            
        } else if (layout.IsHorizontal()) {
            float layout_y = layout_frame.GetY(); 

            float layout_height = layout_frame.GetHeight();
            float used_width = 0f;  
            if (layout.IsAlignLow()) {
                float layout_x = layout_frame.GetX() + layout.Padding; 
                foreach (int c in children) {
                    Frame child_frame = w.GetComponent<Frame>(c); 
                    float child_height = child_frame.GetHeight(); 
                    float child_y = layout_y + ((layout_height - child_height) / 2); 
                    float child_x = layout_x + used_width; 
                    child_frame.SetCoordinates(child_x, child_y); 
                    used_width += child_frame.GetWidth() + layout.Padding; 
                } 
            } else {
                float layout_x = layout_frame.GetX() + layout_frame.GetWidth() - layout.Padding; 
                foreach (int c in children) {
                    Frame child_frame = w.GetComponent<Frame>(c); 
                    float child_height = child_frame.GetHeight(); 
                    float child_width = child_frame.GetWidth(); 
                    float child_y = layout_y + ((layout_height - child_height) / 2); 
                    float child_x = layout_x - (child_frame.GetWidth() + used_width); 
                    child_frame.SetCoordinates(child_x, child_y); 
                    used_width += child_frame.GetWidth() + layout.Padding; 
                } 
            }
        } else if (layout.IsVertical()) {
            float layout_x = layout_frame.GetX(); 
            float layout_width = layout_frame.GetWidth(); 
            float used_height = 0f; 
            if (layout.IsAlignLow()) {
                float layout_y = layout_frame.GetY() + layout.Padding;
                foreach (int c in children) {
                    Frame child_frame = w.GetComponent<Frame>(c); 
                    float child_height = child_frame.GetHeight(); 
                    float child_width = child_frame.GetWidth(); 
                    float child_x = layout_x + ((layout_width - child_width) / 2); 
                    float child_y = layout_y + used_height; 
                    child_frame.SetCoordinates(child_x, child_y); 
                    used_height += child_height + layout.Padding; 
                }
            } else {
                float layout_y = layout_frame.GetY() + layout_frame.GetHeight() - layout.Padding; 
                foreach(int c in children) {
                    Frame child_frame = w.GetComponent<Frame>(c); 
                    float child_height = child_frame.GetHeight(); 
                    float child_width = child_frame.GetWidth(); 
                    float child_x = layout_x + ((layout_width - child_width) / 2); 
                    float child_y = layout_y - (child_height + used_height); 
                    child_frame.SetCoordinates(child_x, child_y); 
                    used_height += child_height + layout.Padding; 
                }
            }
        }
    }; 

    private static Type[] ts = [typeof(LinearLayout), typeof(Active), typeof(Frame)]; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}