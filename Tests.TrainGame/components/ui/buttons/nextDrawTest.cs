using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class NextDrawTestButtonTest {
    [Fact] 
    public void NextDrawTestButton_ShouldSayCorrectNextTest() {
        NextDrawTestButton b1 = new NextDrawTestButton(); 
        NextDrawTestButton b2 = new NextDrawTestButton(1); 

        Assert.Equal(0, b1.GetCurTest()); 
        Assert.Equal(1, b2.GetCurTest()); 
    }
}