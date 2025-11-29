namespace TrainGame.Components;

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

public class LinearLayoutWrap {
    public static void Clear(LinearLayout ll, int e, World w) {
        List<int> children = ll.GetChildren(); 
        for (int i = children.Count - 1; i >= 0; i--) {
            int c = children[i]; 
            if (w.ComponentContainsEntity<LinearLayout>(c)) {
                LinearLayout child_ll = w.GetComponent<LinearLayout>(c);
                Clear(child_ll, c, w);
            }
            w.RemoveEntity(c);
            ll.RemoveChild(c);
        }
        w.RemoveComponent<LinearLayout>(e); 
    }

    //TODO: Test
    public static void ResizeChildren(int llEntity, World w) {
        LinearLayout ll = w.GetComponent<LinearLayout>(llEntity); 
        List<int> cs = ll.GetChildren(); 
        int numChildren = cs.Count; 
        Frame llFrame = w.GetComponent<Frame>(llEntity);

        float width = 0f; 
        float height = 0f; 

        if (ll.IsVertical()) {
            width = llFrame.GetWidth() - 2 * ll.Padding; 
            height = (llFrame.GetHeight() - ((1 + numChildren) * ll.Padding)) / numChildren;
        } else {
            height = llFrame.GetHeight() - 2 * ll.Padding; 
            width = (llFrame.GetWidth() - ((1 + numChildren) * ll.Padding)) / numChildren; 
        }

        foreach (int c in ll.GetChildren()) {
            w.SetComponent<Frame>(c, new Frame(0, 0, width, height));
        }
    }
}