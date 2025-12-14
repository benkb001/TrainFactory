
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
using TrainGame.Utils; 
using TrainGame.Callbacks; 

public class DrawProgressBarCallbackTest {
    [Fact]
    public void DrawProgressBarCallback_ShouldDrawProgressBar() {
        World w = WorldFactory.Build(); 

        DrawProgressBarCallback.Create(w, Vector2.Zero, 10, 10); 
        
        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(ProgressBar), typeof(Backgrounds), typeof(Active)])); 
    }
}