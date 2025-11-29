
using TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawEmbarkMessageTest {
    [Fact]
    public void DrawEmbarkMessage_ShouldRespectConstructorArguments() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv);
        Train train = new Train(Inv: inv, origin: c);
        DrawEmbarkMessage msg = new DrawEmbarkMessage(
            train, 
            new Vector2(100, 100),
            150f, 
            250f, 
            5f
        ); 
        Assert.Equal(train, msg.GetTrain());
        Assert.Equal(c, msg.GetCity()); 
        Assert.Equal(new Vector2(100, 100), msg.Position); 
        Assert.Equal(150f, msg.Width); 
        Assert.Equal(250f, msg.Height); 
        Assert.Equal(5f, msg.Padding);
    }
}