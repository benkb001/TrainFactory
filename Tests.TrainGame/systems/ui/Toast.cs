using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class ToastSystemTest {
    [Fact]
    public void Toast_ShouldRemoveOldToasts() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        Toast t = new Toast(); 
        int e = w.AddEntity(); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Outline>(e, new Outline()); 
        w.SetComponent<TextBox>(e, new TextBox("")); 
        w.SetComponent<Toast>(e, t); 

        int inf_check = 10000; 
        while (t.RemainingDuration >= 0f && inf_check >= 0) {
            w.Update(); 
            inf_check--; 
        }
        Assert.False(w.EntityExists(e)); 
    }

}