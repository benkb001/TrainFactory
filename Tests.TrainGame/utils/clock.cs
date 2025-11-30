
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class GameClockTest {
    [Fact]
    public void GameClock_ShouldAllowVirtualTimePassage() {
        GameClock gc = new GameClock(); 
        double prevMili = gc.TotalMilliseconds; 
        gc.PassTime(seconds: 100, milliseconds: 100); 
        Assert.True(gc.TotalSeconds >= 100);
    }
}