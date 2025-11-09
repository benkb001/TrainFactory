

using System.Collections.Generic;
using System; 

using TrainGame.Components; 

public class ButtonTest {
    [Fact]
    public void Button_ShouldDefaultToNotClicked() {
        Button b = new Button(); 
        Assert.False(b.Clicked); 
    }

    [Fact]
    public void Button_ShouldRespectSpecifiedClicked() {
        Button b = new Button(false); 
        Assert.False(b.Clicked); 
        b = new Button(true); 
        Assert.True(b.Clicked); 
    }
}