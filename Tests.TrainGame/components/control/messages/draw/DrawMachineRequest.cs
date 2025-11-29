using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class DrawMachineRequestMessageTest {
    [Fact]
    public void DrawMachineRequestMessage_ShouldRespectConstructors() {
        Machine m = new Machine(null, null, "", 0, 0);
        DrawMachineRequestMessage dm = new DrawMachineRequestMessage(m, 15f, 20f, new Vector2(5, 20), 1f);
        Assert.Equal(m, dm.GetMachine()); 
        Assert.Equal(15f, dm.Width); 
        Assert.Equal(20f, dm.Height); 
        Assert.Equal(new Vector2(5, 20), dm.Position); 
        Assert.Equal(1f, dm.Margin);
    }
}