using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class DrawMapMessageTest {
    [Fact]
    public void DrawMapMessage_ShouldReturnAnInstance() {
        Assert.True(DrawMapMessage.Get() is DrawMapMessage);
    }

    [Fact]
    public void DrawMapMessage_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(DrawMapMessage.Get(), DrawMapMessage.Get()); 
    }
}