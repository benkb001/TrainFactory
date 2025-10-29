using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class PauseButtonTest {
    [Fact]
    public void PauseButton_ShouldReturnAnInstance() {
        Assert.True(PauseButton.Get() is PauseButton);
    }

    [Fact]
    public void PauseButton_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(PauseButton.Get(), PauseButton.Get()); 
    }
}

public class UnPauseButtonTest {
    [Fact]
    public void UnPauseButton_ShouldReturnAnInstance() {
        Assert.True(UnpauseButton.Get() is UnpauseButton);
    }

    [Fact]
    public void UnPauseButton_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(UnpauseButton.Get(), UnpauseButton.Get()); 
    }
}