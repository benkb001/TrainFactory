
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

public class BackgroundsTest {
    [Fact]
    public void Backgrounds_ShouldRespectAddingBackground() {
        Backgrounds bgs = new Backgrounds(); 
        Frame f = new Frame(0, 0, 10, 10); 
        Background bg = new Background(Color.White); 
        bgs.Add(bg, f); 
        
        (Background bg1, Frame f1) = bgs.Ls[0]; 
        Assert.Equal(bg, bg1); 
        Assert.Equal(f, f1); 
    }
}