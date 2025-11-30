
using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class DrawBackgroundMessageTest {
    [Fact]
    public void DrawBackgroundMessage_ShouldReturnAnInstance() {
        Assert.True(DrawBackgroundMessage.Get() is DrawBackgroundMessage);
    }

    [Fact]
    public void DrawBackgroundMessage_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(DrawBackgroundMessage.Get(), DrawBackgroundMessage.Get()); 
    }
}