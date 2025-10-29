using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class StepperButtonTest {
    [Fact] 
    public void StepperButton_ShouldRespectConstructorArguments() {
        StepperButton sb = new StepperButton(0, 1);
        Assert.Equal(0, sb.Entity); 
        Assert.Equal(1, sb.Delta); 
    }
}