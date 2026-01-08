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

public class LinearLayoutContainer {
    private int llEnt; 
    private LinearLayout ll;
    private float llWidth; 
    private float llHeight; 
    
    public float LLWidth => llWidth; 
    public float LLHeight => llHeight; 
    public int LLEnt => llEnt; 

    public LinearLayoutContainer(int llEnt, LinearLayout ll, float llWidth, float llHeight) {
        this.llEnt = llEnt; 
        this.ll = ll; 
        this.llWidth = llWidth; 
        this.llHeight = llHeight; 
    }

    public void AddChild(int e, World w) {
        LinearLayoutWrap.AddChild(e, llEnt, ll, w);
    }

    public void ResizeChildren(World w) {
        LinearLayoutWrap.ResizeChildren(llEnt, w); 
    }
}

public class LinearLayoutWrap {
    public static LinearLayoutContainer Add(World w, Vector2 position, float width, float height, 
        bool usePaging = false, int childrenPerPage = 10, string direction = "horizontal", 
        string align = "alignlow", float padding = 5f, string label = "", bool outline = true) {
        
        //add the outer linear layout, has two children: header row and main ll
        int outerLLEnt = EntityFactory.Add(w); 
        w.SetComponent<Frame>(outerLLEnt, new Frame(position, width, height)); 
        LinearLayout outerLL = new LinearLayout("vertical", "alignlow"); 
        w.SetComponent<LinearLayout>(outerLLEnt, outerLL); 
        outerLL.Padding = padding; 

        //add header row ll
        LinearLayout headerRowLL = new LinearLayout("horizontal", "alignlow"); 
        int headerRowLLEnt = EntityFactory.Add(w); 
        w.SetComponent<LinearLayout>(headerRowLLEnt, headerRowLL); 
        float headerHeight = height / 8f;
        float headerWidth = Math.Max(20, width - (2 * padding)); 
        w.SetComponent<Frame>(headerRowLLEnt, new Frame(Vector2.Zero, headerWidth, headerHeight)); 
        headerRowLL.Padding = padding; 

        bool useHeader = false; 

        //labelWidth and labelHeight are used for both the label and the dimensions 
        //of the pager buttons, if either are included
        float labelWidth = (headerWidth / 2f) - (3 * padding); 
        float labelHeight = Math.Max(5f, headerHeight - (2 * padding));

        //add the main ll, wait to add till after header
        int mainLLEnt = EntityFactory.Add(w); 
        LinearLayout mainLL = new LinearLayout(direction, align, usePaging, childrenPerPage); 
        w.SetComponent<LinearLayout>(mainLLEnt, mainLL); 
        float mainLLWidth = width - 2 * padding; 
        float mainLLHeight = height - headerHeight - 3 * padding; 
        w.SetComponent<Frame>(mainLLEnt, new Frame(Vector2.Zero, mainLLWidth, mainLLHeight)); 
        mainLL.Padding = padding; 

        if (outline) {
            w.SetComponent<Outline>(outerLLEnt, new Outline()); 
            w.SetComponent<Outline>(headerRowLLEnt, new Outline()); 
            w.SetComponent<Outline>(mainLLEnt, new Outline()); 
        }

        if (label != "") {
            useHeader = true;
            int labelEnt = EntityFactory.Add(w); 
            
            w.SetComponent<Frame>(labelEnt, new Frame(Vector2.Zero, labelWidth, labelHeight)); 
            w.SetComponent<TextBox>(labelEnt, new TextBox(label)); 
            if (outline) {
                w.SetComponent<Outline>(labelEnt, new Outline()); 
            }
            AddChild(labelEnt, headerRowLLEnt, headerRowLL, w);
        } 

        if (usePaging) {
            useHeader = true; 
            int pageLLEnt = EntityFactory.Add(w); 
            LinearLayout pageLL = new LinearLayout("horizontal", "alignlow"); 
            w.SetComponent<LinearLayout>(pageLLEnt, pageLL); 
            pageLL.Padding = padding; 

            AddChild(pageLLEnt, headerRowLLEnt, headerRowLL, w); 

            float pageHeight = Math.Max(5f, labelHeight - (2 * padding)); 
            float pageWidth = pageHeight; 
            float pageLLWidth = (pageWidth * 2) + (3 * padding); 

            w.SetComponent<Frame>(pageLLEnt, new Frame(Vector2.Zero, pageLLWidth, labelHeight)); 

            int[] ds = [-1, 1]; 

            foreach (int d in ds) {
                int curEnt = EntityFactory.Add(w);
                AddChild(curEnt, pageLLEnt, pageLL, w); 
                List<Vector2> points = [
                    new Vector2(0, 0),
                    new Vector2(0, pageHeight),
                    new Vector2(pageWidth * d, pageHeight / 2f),
                ]; 

                w.SetComponent<Frame>(curEnt, new Frame(points));
                w.SetComponent<Button>(curEnt, new Button()); 
                w.SetComponent<LLPageButton>(curEnt, new LLPageButton(mainLL, d));
                w.SetComponent<Outline>(curEnt, new Outline()); 
            }
            

            if (outline) {
                w.SetComponent<Outline>(pageLLEnt, new Outline()); 
            }
        }

        if (useHeader) {
            AddChild(headerRowLLEnt, outerLLEnt, outerLL, w); 
        } else {
            w.RemoveEntity(headerRowLLEnt); 
        }

        AddChild(mainLLEnt, outerLLEnt, outerLL, w); 

        return new LinearLayoutContainer(mainLLEnt, mainLL, mainLLWidth, mainLLHeight); 
    }

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
        List<int> pagedChildren = ll.GetPagedChildren(); 
        int numChildren = ll.UsePaging ? ll.ChildrenPerPage : cs.Count; 
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

        foreach (int c in ll.UsePaging ? pagedChildren : cs) {
            w.SetComponent<Frame>(c, new Frame(0, 0, width, height));
            if (recurse && w.ComponentContainsEntity<LinearLayout>(c)) {
                ResizeChildren(c, w, recurse: true); 
            }
        }
    }

    public static void AddChild(int childEntity, int linearLayoutEntity, LinearLayout ll, World w) {
        ll.AddChild(childEntity); 
        LLChild c = new LLChild(linearLayoutEntity);
        w.SetComponent<LLChild>(childEntity, c);
        c.Depth = GetDepth(childEntity, w);  
    }

    public static int GetDepth(int childEntity, World w, int depth = 0) {
        if (w.ComponentContainsEntity<LLChild>(childEntity)) {
            int parentEntity = w.GetComponent<LLChild>(childEntity).ParentEntity;
            if (parentEntity == childEntity) {
                throw new InvalidOperationException($"Entity {parentEntity} thinks it is its own parent linear layout");
            }
            return GetDepth(parentEntity, w, depth + 1);
        }
        return depth; 
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