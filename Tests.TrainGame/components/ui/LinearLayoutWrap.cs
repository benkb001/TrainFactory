using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

[Collection("Sequential")]
public class LinearLayoutWrapTest {
    [Fact]
    public void LinearLayoutWrap_ClearShouldRemoveAllNestedEntitiesFromWorld() {
        World w = WorldFactory.Build(); 

        LinearLayout llParent = new LinearLayout("Horizontal", "alignlow"); 
        LinearLayout llChild = new LinearLayout("Horizontal", "alignlow"); 
        
        int parentEnt = EntityFactory.Add(w); 
        int childEnt = EntityFactory.Add(w); 

        w.SetComponent<LinearLayout>(parentEnt, llParent); 
        w.SetComponent<LinearLayout>(childEnt, llChild); 

        w.SetComponent<Frame>(parentEnt, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Frame>(childEnt, new Frame(0, 0, 50, 50)); 

        LinearLayoutWrap.AddChild(childEnt, parentEnt, llParent, w); 

        int grandChildEnt = EntityFactory.Add(w); 
        w.SetComponent<Frame>(grandChildEnt, new Frame(0, 0, 25, 25)); 
        LinearLayoutWrap.AddChild(grandChildEnt, childEnt, llChild, w); 

        Assert.True(w.EntityExists(parentEnt)); 
        Assert.True(w.EntityExists(childEnt)); 
        Assert.True(w.EntityExists(grandChildEnt)); 

        LinearLayoutWrap.Clear(parentEnt, w); 

        Assert.False(w.EntityExists(childEnt)); 
        Assert.False(w.EntityExists(grandChildEnt)); 
    }

    [Fact]
    public void LinearLayoutWrap_AddChildShouldAddAnLLChlidToChildEntityWithCorrectParentEntity() {
        World w = WorldFactory.Build(); 

        LinearLayout llParent = new LinearLayout("Horizontal", "alignlow"); 
        
        int parentEnt = EntityFactory.Add(w); 
        int childEnt = EntityFactory.Add(w); 

        w.SetComponent<LinearLayout>(parentEnt, llParent); 

        LinearLayoutWrap.AddChild(childEnt, parentEnt, llParent, w); 
        Assert.Equal(parentEnt, w.GetComponent<LLChild>(childEnt).ParentEntity);
    }

    [Fact]
    public void LinearLayoutWrap_GetDepthShouldReturnNumberOfParentLLsAnEntityHas() {
        World w = WorldFactory.Build(); 

        LinearLayout llParent = new LinearLayout("Horizontal", "alignlow"); 
        LinearLayout llChild = new LinearLayout("Horizontal", "alignlow"); 
        
        int parentEnt = EntityFactory.Add(w); 
        int childEnt = EntityFactory.Add(w); 

        w.SetComponent<LinearLayout>(parentEnt, llParent); 
        w.SetComponent<LinearLayout>(childEnt, llChild); 

        LinearLayoutWrap.AddChild(childEnt, parentEnt, llParent, w); 

        int grandChildEnt = EntityFactory.Add(w); 

        LinearLayoutWrap.AddChild(grandChildEnt, childEnt, llChild, w); 

        Assert.Equal(2, w.GetComponent<LLChild>(grandChildEnt).Depth); 
        Assert.Equal(1, w.GetComponent<LLChild>(childEnt).Depth); 
        Assert.False(w.ComponentContainsEntity<LLChild>(parentEnt)); 
    }

    [Fact]
    public void LinearLayoutWrap_ResizeChildrenShouldSizeCorrectly() {
        World w = WorldFactory.Build(); 
        int llEntity = EntityFactory.Add(w); 
        LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
        w.SetComponent<LinearLayout>(llEntity, ll); 
        w.SetComponent<Frame>(llEntity, new Frame(0, 0, 100, 100));
        ll.Padding = 5f; 
        
        int c1 = EntityFactory.Add(w); 
        int c2 = EntityFactory.Add(w); 
        ll.AddChild(c1); 
        ll.AddChild(c2); 
        LinearLayoutWrap.ResizeChildren(llEntity, w);

        Frame f1 = w.GetComponent<Frame>(c1); 
        Frame f2 = w.GetComponent<Frame>(c2); 
        Assert.Equal(90f, f1.GetHeight()); 
        Assert.Equal(42.5f, f2.GetWidth()); 
        Assert.Equal(f1.GetHeight(), f2.GetHeight()); 
        Assert.Equal(f1.GetWidth(), f2.GetWidth()); 
    }

    [Fact]
    public void LinearLayoutWrap_ScrollingShouldPage() {
        VirtualMouse.Reset(); 

        World w = WorldFactory.Build(); 
        LinearLayoutContainer llc = LinearLayoutWrap.Add(w, Vector2.Zero, 100f, 100f, usePaging: true, childrenPerPage: 1);
        int e1 = EntityFactory.AddUI(w, Vector2.Zero, 10, 10); 
        int e2 = EntityFactory.AddUI(w, Vector2.Zero, 10, 10);
        llc.AddChild(e1, w); 
        llc.AddChild(e2, w); 
        Assert.Equal(e1, llc.GetChildren()[0]);

        Frame llFrame = w.GetComponent<Frame>(llc.LLEnt);
        VirtualMouse.SetCoordinates(llFrame.Position + new Vector2(10, 10)); 
        VirtualMouse.ScrollDown(); 
        w.Update(); 

        Assert.Equal(e2, llc.GetChildren()[0]);
        VirtualMouse.Reset(); 
    }

    [Fact]
    public void LinearLayoutWrap_AddChildShouldIncreaseTheDepthOfGrandChildren() {
        World w = WorldFactory.Build(); 
        LinearLayoutContainer llc = LinearLayoutWrap.Add(w, Vector2.Zero, 100f, 100f);
        int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10); 
        llc.AddChild(e, w); 
        int prevDepth = LinearLayoutWrap.GetDepth(e, w); 
        LinearLayoutContainer outer = LinearLayoutWrap.Add(w, Vector2.Zero, 100f, 100f); 
        outer.AddChild(llc.GetParentEntity(), w); 
        Assert.Equal(prevDepth * 2, LinearLayoutWrap.GetDepth(e, w));
    }
}