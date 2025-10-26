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

            //TODO: Just register the systems this test depends on for decoupling?  
            //Or is it better to include in case a new added system fucks this up? 
            RegisterSystems.All(w); 
            LinearLayout ll1 = new LinearLayout("horizontal", "alignLow"); 
            ll1.Padding = 5f; 
            int e = w.AddEntity(); 

            Frame ll1_frame = new Frame(0, 0, 600, 150);
            w.SetComponent<Frame>(e, ll1_frame); 
            w.SetComponent<LinearLayout>(e, ll1);

            int c1 = w.AddEntity(); 
            Frame c1_frame = new Frame(0, 0, 100, 100);
            w.SetComponent<Frame>(c1, c1_frame); 

            int c2 = w.AddEntity(); 
            Frame c2_frame = new Frame(0, 0, 100, 100); 
            w.SetComponent<Frame>(c2, c2_frame); 
            ll1.AddChild(c1); 
            ll1.AddChild(c2); 

            w.Update(); 
            Assert.True(Util.FloatEqual(c1_frame.GetX(), ll1_frame.GetX() + ll1.Padding));
            Assert.True(Util.FloatEqual(c2_frame.GetX(), ll1_frame.GetX() + c1_frame.GetWidth() + (ll1.Padding * 2))); 

            /*
            TODO: Test y axis alongside above assertions, and
            Test other directions/alignments and spaceeven once implemeneted
            
            LinearLayout ll2 = new LinearLayout("horizontal", "alignHigh"); 
            ll2.Padding = 5f; 
            int e2 = w.AddEntity(); 
            w.SetComponent<Frame>(e2, new Frame(0, 160, 600, 150)); 
            w.SetComponent<LinearLayout>(e2, ll2);

            int c3 = w.AddEntity(); 
            w.SetComponent<Frame>(c3, new Frame(0, 0, 100, 100)); 


            int c4 = w.AddEntity(); 
            w.SetComponent<Frame>(c4, new Frame(0, 0, 100, 100)); 

            ll2.AddChild(c3); 
            ll2.AddChild(c4); 

            LinearLayout ll3 = new LinearLayout("vertical", "alignLow"); 
            ll3.Padding = 5f; 
            int e3 = w.AddEntity(); 
            w.SetComponent<Frame>(e3, new Frame(610, 10, 100, 400)); 
            w.SetComponent<LinearLayout>(e3, ll3);

            int c5 = w.AddEntity(); 
            w.SetComponent<Frame>(c5, new Frame(0, 0, 100, 100)); 

            int c6 = w.AddEntity(); 
            w.SetComponent<Frame>(c6, new Frame(0, 0, 100, 100)); 

            ll3.AddChild(c5); 
            ll3.AddChild(c6); 

            LinearLayout ll4 = new LinearLayout("vertical", "alignHigh"); 
            ll4.Padding = 5f; 
            int e4 = w.AddEntity(); 
            w.SetComponent<Frame>(e4, new Frame(720, 10, 100, 400)); 
            w.SetComponent<LinearLayout>(e4, ll4);

            int c7 = w.AddEntity(); 
            w.SetComponent<Frame>(c7, new Frame(0, 0, 100, 100)); 

            int c8 = w.AddEntity(); 
            w.SetComponent<Frame>(c8, new Frame(0, 0, 100, 100)); 

            ll4.AddChild(c7); 
            ll4.AddChild(c8); 
            */
    }
}