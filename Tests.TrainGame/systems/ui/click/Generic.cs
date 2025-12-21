using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

//todo: fix
public class ClickSystemTest {
    [Fact]
    public void ClickSystem_ShouldRunOnClick() {
        World w = WorldFactory.Build(); 
        w.AddComponentType<TestButton>(); 
        ClickSystem.Register<TestButton>(w, (w, e) => {
            w.GetComponent<TestButton>(e).Clicked = true; 
        }); 

        TestButton tb = new TestButton(); 
        Assert.False(tb.Clicked); 
        
        int e = EntityFactory.Add(w); 
        w.SetComponent<TestButton>(e, tb); 
        w.SetComponent<Button>(e, new Button(true)); 

        w.Update(); 

        Assert.True(tb.Clicked); 
    }
}