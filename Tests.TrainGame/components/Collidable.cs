using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class CollidableTest {
    [Fact]
    public void Collidable_ShouldReturnAnInstance() {
        Assert.True(Collidable.Get() is Collidable);
    }

    [Fact]
    public void Collidable_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(Collidable.Get(), Collidable.Get()); 
    }
}