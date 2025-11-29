using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class DrawInventoryMessageTest {
    [Fact] 
    public void DrawInventoryMessage_ShouldRespectConstructorArguments() {
        DrawInventoryMessage m = new DrawInventoryMessage(100, 200, Vector2.Zero, new Inventory("Test", 1, 1), 0, 0f, false); 
        Assert.Equal(100, m.Width); 
        Assert.Equal(200, m.Height); 
        Assert.Equal(Vector2.Zero, m.Position); 
        Assert.Equal("Test", m.Inv.GetId()); 
        Assert.Equal(0, m.Entity); 
        Assert.Equal(0f, m.Padding); 
        Assert.False(m.DrawLabel); 
    }
}