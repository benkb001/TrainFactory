using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

//sequential because global state (mouse)
[Collection("Sequential")]
public class NextDrawTestSystemTest {
    [Fact]
    public void NextDrawTest_ShouldMoveButtonWhenClickedFirst() {
        World w = new World(); 
        RegisterComponents.All(w); 
        
        ButtonSystem.RegisterClick(w); 
        NextDrawTestButtonSystem.Register(w); 
        NextDrawTestUISystem.Register(w); 
        ButtonSystem.RegisterUnclick(w); 

        int e = w.AddEntity(); 

        w.SetComponent<Button>(e, new Button()); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 100, 100)); 
        w.SetComponent<NextDrawTestButton>(e, new NextDrawTestButton());

        VirtualMouse.SetCoordinates(1, 1); 
        VirtualMouse.LeftClick(); 

        Assert.True(w.GetComponent<Frame>(0).GetX() == 0); 
        w.Update(); 

        VirtualMouse.LeftRelease(); 

        //It's two here because clicking the button creates a NextTestControl component attached to a different entity
        //Then that entity gets cleared and the new button is made. 
        Assert.False(w.GetComponent<Frame>(2).GetX() == 0); 
    }
}