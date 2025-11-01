namespace TrainGame.Components;

using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class PushSceneMessageTest {
    [Fact]
    public void PushSceneMessage_ShouldReturnAnInstance() {
        Assert.True(PushSceneMessage.Get() is PushSceneMessage);
    }

    [Fact]
    public void PushSceneMessage_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(PushSceneMessage.Get(), PushSceneMessage.Get()); 
    }
}

public class PopSceneMessageTest {
    [Fact]
    public void PopSceneMessage_ShouldReturnAnInstance() {
        Assert.True(PopSceneMessage.Get() is PopSceneMessage);
    }

    [Fact]
    public void PopSceneMessage_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(PopSceneMessage.Get(), PopSceneMessage.Get()); 
    }
}