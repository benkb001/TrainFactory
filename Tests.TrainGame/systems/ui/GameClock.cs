
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class GameClockViewSystemTest {
    [Fact]
    public void GameClockViewSystem_ShouldSetTimeToWorldTime() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 
        int gcvEntity = EntityFactory.Add(w);
        w.SetComponent<Frame>(gcvEntity, new Frame(0, 0, 100, 100));
        w.SetComponent<TextBox>(gcvEntity, new TextBox("")); 
        w.SetComponent<GameClockView>(gcvEntity, GameClockView.Get());

        w.Update(); 

        Assert.Equal(new WorldTime().ToString(), w.GetComponent<TextBox>(gcvEntity).Text);
        w.PassTime(new WorldTime(days: 1, hours: 2, minutes: 9, ticks: 59)); 
        w.Update(); 

        Assert.Equal(new WorldTime(days: 1, hours: 2, minutes: 10).ToString(), w.GetComponent<TextBox>(gcvEntity).Text);
    }
}