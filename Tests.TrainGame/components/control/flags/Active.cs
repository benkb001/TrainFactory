using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class ActiveTest {
    [Fact]
    public void Active_ShouldReturnAnInstance() {
        Assert.True(Active.Get() is Active);
    }

    [Fact]
    public void Active_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(Active.Get(), Active.Get()); 
    }
}