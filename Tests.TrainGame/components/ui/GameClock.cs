using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class GameClockViewTest {
    [Fact]
    public void GameClockView_ShouldReturnAnInstance() {
        Assert.True(GameClockView.Get() is GameClockView);
    }

    [Fact]
    public void GameClockView_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(GameClockView.Get(), GameClockView.Get()); 
    }
}