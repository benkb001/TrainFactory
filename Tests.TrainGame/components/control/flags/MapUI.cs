
using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class MapUIFlagTest {
    [Fact]
    public void MapUIFlag_ShouldReturnAnInstance() {
        Assert.True(MapUIFlag.Get() is MapUIFlag);
    }

    [Fact]
    public void MapUIFlag_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(MapUIFlag.Get(), MapUIFlag.Get()); 
    }
}