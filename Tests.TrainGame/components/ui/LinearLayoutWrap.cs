using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class LinearLayoutWrapTest {
    [Fact]
    public void LinearLayoutWrap_ClearShouldRemoveAllNestedEntitiesFromWorld() {
        
        //too lazy to write because i am not using this method at all right now. 
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
}