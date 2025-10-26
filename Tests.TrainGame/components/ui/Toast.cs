
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.Utils; 

public class ToastTest {
    [Fact]
    public void Toast_ShouldDefaultToOneRemainingDuration() {
        Toast t = new Toast(); 
        Assert.True(Util.FloatEqual(t.RemainingDuration, 1f));
    }

    [Fact] 
    public void Toast_ShouldRespectDecrementing() {
        Toast t = new Toast(); 
        t.DecrementDuration(); 
        Assert.True(Util.FloatEqual(t.RemainingDuration, 0.995f)); 
    }
}   