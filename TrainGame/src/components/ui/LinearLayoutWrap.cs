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
}