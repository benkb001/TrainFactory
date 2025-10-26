
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

public class TextBoxTest {

    [Fact]
    public void TextBox_ShouldRespectConstructor() {
        TextBox t = new TextBox("test"); 
        Assert.Equal("test", t.Text); 
    }
}