
using TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawTrainsViewMessageTest {
    [Fact]
    public void DrawTrainsViewMessage_ShouldRespectConstructors() {
        List<Train> ts = new(); 
        DrawTrainsViewMessage dm = new DrawTrainsViewMessage(
            ts, 
            100f, 
            200f, 
            new Vector2(10, 20)
        ); 
        Assert.Equal(ts, dm.Trains); 
        Assert.Equal(100f, dm.Width); 
        Assert.Equal(200f, dm.Height); 
        Assert.Equal(10, dm.Position.X); 
        Assert.Equal(20, dm.Position.Y); 
    }
}