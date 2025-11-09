using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class InteractorTest {
    [Fact]
    public void Interactor_ShouldReturnAnInstance() {
        Assert.True(Interactor.Get() is Interactor);
    }

    [Fact]
    public void Interactor_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(Interactor.Get(), Interactor.Get()); 
    }
}