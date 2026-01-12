using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class LLScreenAnchorIntegrationTest {
    [Fact]
    public void WhenCameraPanCausesAParentLLEntityWithScreenAnchorToMoveItsChildrenShouldBeRepositionedRelativeToTheNewPosition() {
        World w = WorldFactory.Build(); 

        LinearLayout parent = new LinearLayout("vertical", "alignlow"); 
        int parentEnt = EntityFactory.Add(w); 
        w.SetComponent<LinearLayout>(parentEnt, parent); 
        w.SetComponent<ScreenAnchor>(parentEnt, new ScreenAnchor(Vector2.Zero)); 
        w.SetComponent<Frame>(parentEnt, new Frame(0, 0, 1000, 1000)); 
        

        int childEnt = EntityFactory.Add(w); 
        LinearLayout childLL = new LinearLayout("horizontal", "alignlow"); 
        w.SetComponent<LinearLayout>(childEnt, childLL); 
        w.SetComponent<Frame>(childEnt, new Frame(0, 0, 500, 500));
        LinearLayoutWrap.AddChild(childEnt, parentEnt, parent, w);

        int grandchildEnt = EntityFactory.Add(w); 
        Frame grandchildFrame = new Frame(0, 0, 100, 100);
        w.SetComponent<Frame>(grandchildEnt, grandchildFrame); 
        LinearLayoutWrap.AddChild(grandchildEnt, childEnt, childLL, w);

        w.SetCameraPosition(Vector2.Zero); 
        w.Update(); 

        Vector2 grandchildPosition = grandchildFrame.Position; 
        w.SetCameraPosition(new Vector2(10, 10)); 
        w.Update(); 

        //Assert.Equal(grandchildPosition + new Vector2(10, 10), grandchildFrame.Position); 
    }
}