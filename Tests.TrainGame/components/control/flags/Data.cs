using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class DataTest {
    [Fact]
    public void Data_ShouldReturnAnInstance() {
        Assert.True(Data.Get() is Data);
    }

    [Fact]
    public void Data_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(Data.Get(), Data.Get()); 
    }
}