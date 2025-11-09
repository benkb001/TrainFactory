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

public class OutlineTest {
    public void Outline_ShouldDefaultToOneThicknessWhite() {
        Outline o = new Outline(); 
        Assert.Equal(1, o.GetThickness()); 
        Assert.Equal(Color.White, o.GetColor()); 
    }

    public void Outline_ShouldRespectConstructors() {
        Outline o = new Outline(Color.Red, 2); 
        Assert.Equal(2, o.GetThickness()); 
        Assert.Equal(Color.Red, o.GetColor()); 
    }
}