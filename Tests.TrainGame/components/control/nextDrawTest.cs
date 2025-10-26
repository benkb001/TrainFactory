using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class NextDrawTestControlTest {
    [Fact] 
    public void NextDrawTestControl_ShouldSayCorrectNextTest() {
        NextDrawTestButton c1 = new NextDrawTestButton(1); 

        Assert.Equal(1, c1.GetCurTest()); 
    }
}