using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class LinearLayoutSystemTest {
    [Fact]
    public void LinearLayout_ShouldPositionChildrenCorrectly() {
        World w = new World(); 
        RegisterComponents.All(w); 

        RegisterSystems.All(w); 
        LinearLayout ll1 = new LinearLayout("horizontal", "alignLow"); 
        ll1.Padding = 5f; 
        int e = EntityFactory.Add(w); 

        Frame ll1_frame = new Frame(0, 0, 600, 150);
        w.SetComponent<Frame>(e, ll1_frame); 
        w.SetComponent<LinearLayout>(e, ll1);

        int c1 = EntityFactory.Add(w); 
        Frame c1_frame = new Frame(0, 0, 100, 100);
        w.SetComponent<Frame>(c1, c1_frame); 

        int c2 = EntityFactory.Add(w); 
        Frame c2_frame = new Frame(0, 0, 100, 100); 
        w.SetComponent<Frame>(c2, c2_frame); 
        ll1.AddChild(c1); 
        ll1.AddChild(c2); 

        w.Update(); 
        Assert.True(Util.FloatEqual(c1_frame.GetX(), ll1_frame.GetX() + ll1.Padding));
        Assert.True(Util.FloatEqual(c2_frame.GetX(), ll1_frame.GetX() + c1_frame.GetWidth() + (ll1.Padding * 2))); 
    }

    [Fact]
    public void LinearLayout_ShouldPositionOuterLLsBeforeChildrenLLs() {
        World w = WorldFactory.Build(); 

        int grandChildEnt = EntityFactory.Add(w); 
        int childEnt = EntityFactory.Add(w); 
        int parentEnt = EntityFactory.Add(w); 

        LinearLayout llChild = new LinearLayout("horizontal", "alignlow"); 
        
        w.SetComponent<LinearLayout>(childEnt, llChild); 
        w.SetComponent<Frame>(childEnt, new Frame(500, 500, 100, 100)); 

        LinearLayout llParent = new LinearLayout("horizontal", "alignlow"); 
        
        w.SetComponent<LinearLayout>(parentEnt, llParent); 
        w.SetComponent<Frame>(parentEnt, new Frame(0, 0, 100, 100)); 

        LinearLayoutWrap.AddChild(childEnt, parentEnt, llParent, w); 
        LinearLayoutWrap.AddChild(grandChildEnt, childEnt, llChild, w); 

        w.SetComponent<Frame>(grandChildEnt, new Frame(1000, 100, 100, 100));

        w.Update(); 

        Assert.Equal(0, w.GetComponent<Frame>(grandChildEnt).Position.X); 
        Assert.Equal(0, w.GetComponent<Frame>(grandChildEnt).Position.Y); 
    }
}