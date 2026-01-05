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
using TrainGame.Systems; 

public class LLPageButton {
    public readonly LinearLayout LL; 
    private int delta; 
    public int Delta => delta; 
    public LLPageButton(LinearLayout ll, int delta) {
        this.delta = delta; 
        this.LL = ll; 
    }
}

public static class LLPageSystem {
    public static void Register(World w) {
        ClickSystem.Register<LLPageButton>(w, (w, e) => {
            LLPageButton pb = w.GetComponent<LLPageButton>(e);
            LinearLayout ll = pb.LL; 
            ll.Page(pb.Delta);
        }); 
    }
}

public class LinearLayoutWrap {
    public static void Clear(int e, World w, LinearLayout ll = null) {
        if (ll == null) {
            if (w.ComponentContainsEntity<LinearLayout>(e)) {
                ll = w.GetComponent<LinearLayout>(e); 
            } else {
                return; 
            }
        }

        List<int> children = ll.GetChildren(); 
        for (int i = children.Count - 1; i >= 0; i--) {
            int cEntity = children[i]; 
            if (w.ComponentContainsEntity<LinearLayout>(cEntity)) {
                Clear(cEntity, w);
            }
            w.RemoveEntity(cEntity);
            ll.RemoveChild(cEntity);
        }

        if (w.ComponentContainsEntity<LinearLayout>(e)) {
            w.RemoveComponent<LinearLayout>(e); 
        }
    }

    //TODO: Test
    public static void ResizeChildren(int llEntity, World w, bool recurse = false) {
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
            if (recurse && w.ComponentContainsEntity<LinearLayout>(c)) {
                ResizeChildren(c, w); 
            }
        }
    }

    public static void AddChild(int childEntity, int linearLayoutEntity, LinearLayout ll, World w) {
        ll.AddChild(childEntity); 
        LLChild c = new LLChild(linearLayoutEntity);
        w.SetComponent<LLChild>(childEntity, c);
        c.Depth = GetDepth(childEntity, w);  
    }

    public static int GetDepth(int childEntity, World w) {
        if (w.ComponentContainsEntity<LLChild>(childEntity)) {
            return 1 + GetDepth(w.GetComponent<LLChild>(childEntity).ParentEntity, w);
        }
        return 0; 
    }

    public static int GetParent(int childEntity, World w) {
        if (w.ComponentContainsEntity<LLChild>(childEntity)) {
            return w.GetComponent<LLChild>(childEntity).ParentEntity; 
        }
        return -1;
    }

    public static int ClearParent(int childEntity, World w) {
        (LLChild llChild, bool success) = w.GetComponentSafe<LLChild>(childEntity); 
        
        if (success) {
            int parentEnt = llChild.ParentEntity;
            LinearLayoutWrap.Clear(parentEnt, w); 
            return parentEnt; 
        }

        return -1; 
    }
}