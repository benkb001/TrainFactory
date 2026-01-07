
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class WorldTimeButtonSystemsTest {
    [Fact]
    public void PauseButton_ClickShouldStopWorldTimePassage() {
        World w = WorldFactory.Build(); 

        int pbEnt = EntityFactory.Add(w); 
        w.SetComponent<Button>(pbEnt, new Button(true)); 
        w.SetComponent<PauseButton>(pbEnt, PauseButton.Get()); 
        w.Update(); 
        Assert.Equal(0, w.MiliticksPerUpdate);
    }

    [Fact]
    public void SpeedButton_ClickShouldChangewWorldTimePassageSpeed() {
        World w = WorldFactory.Build(); 
        int slowEnt = EntityFactory.Add(w); 
        w.SetComponent<Button>(slowEnt, new Button(true)); 
        w.SetComponent<SlowTimeButton>(slowEnt, SlowTimeButton.Get()); 
        w.Update(); 
        int slowTime = w.MiliticksPerUpdate; 

        int fastEnt = EntityFactory.Add(w); 
        w.SetComponent<Button>(fastEnt, new Button(true)); 
        w.SetComponent<SpeedTimeButton>(fastEnt, SpeedTimeButton.Get()); 
        w.Update();
        Assert.True(w.MiliticksPerUpdate > slowTime); 
    }
}