using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 

public class RegisterComponentsTest {
    //sanity check idrk what else to write
    [Fact]
    public void RegisterComponentsAll_ShouldRegisterMoreThanZeroComponents() {
        World w = new World(); 
        Assert.Equal(0, w.GetComponentTypeCount()); 
        RegisterComponents.All(w); 
        Assert.True(w.GetComponentTypeCount() > 0); 
    }
}