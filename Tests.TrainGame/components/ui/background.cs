
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

public class BackgroundTest {

    [Fact]
    public void Background_ShouldRespectConstructorArguments() {
        Background b = new Background(Color.Red, 0.5f); 
        Assert.Equal(Color.Red, b.BackgroundColor); 
        Assert.Equal(0.5f, b.Depth); 
    }
}