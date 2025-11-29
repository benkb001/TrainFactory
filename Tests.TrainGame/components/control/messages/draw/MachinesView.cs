
using TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawMachinesViewMessageTest {
    [Fact]
    public void DrawMachinesViewMessage_ShouldRespectConstructors() {
        List<Machine> ms = new(); 
        DrawMachinesViewMessage dm = new DrawMachinesViewMessage(
            ms, 
            100f, 
            200f, 
            new Vector2(10, 20)
        ); 
        Assert.Equal(ms, dm.Machines); 
        Assert.Equal(100f, dm.Width); 
        Assert.Equal(200f, dm.Height); 
        Assert.Equal(10, dm.Position.X); 
        Assert.Equal(20, dm.Position.Y); 
    }
}