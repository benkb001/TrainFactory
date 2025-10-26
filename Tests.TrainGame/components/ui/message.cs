using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class MessageTest {
    [Fact] 
    public void Message_ShouldRespectConstructor() {
        Message m = new Message("Hey"); 
        Assert.Equal("Hey", m.message);

        m.message = "Yall"; 
        Assert.Equal("Yall", m.message); 
    }
}