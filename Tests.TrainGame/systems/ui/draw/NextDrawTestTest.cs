using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class NextDrawTestSystemTest {
    [Fact]
    public void NextDrawTest_ShouldMoveButtonWhenClickedFirst() {
        VirtualMouse.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        
        ButtonSystem.RegisterClick(w); 
        NextDrawTestButtonSystem.Register(w); 
        NextDrawTestUISystem.Register(w); 
        ButtonSystem.RegisterUnclick(w); 

        int e = EntityFactory.Add(w); 

        w.SetComponent<Button>(e, new Button(true)); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 100, 100)); 
        w.SetComponent<NextDrawTestButton>(e, new NextDrawTestButton());

        Assert.True(w.GetComponent<Frame>(0).GetX() == 0); 
        w.Update(); 

        VirtualMouse.LeftRelease(); 

        Assert.False(w.GetComponent<Frame>(2).GetX() == 0); 
    }
}