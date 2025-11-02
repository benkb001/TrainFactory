
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

public class LinesTest {
    [Fact]
    public void Lines_ShouldBeEmptyInitially() {
        Lines ls = new Lines(); 
        Assert.Empty(ls.Ls);
    }

    [Fact]
    public void Lines_AddShouldAddSpecifiedLine() {
        Lines ls = new Lines(); 
        ls.AddLine(new Vector2(0, 0), new Vector2(10, 10), Color.Blue);

        Assert.Single(ls.Ls); 
        Assert.Equal((new Vector2(0, 0), new Vector2(10, 10), Color.Blue), ls.Ls[0]); 
    }
}