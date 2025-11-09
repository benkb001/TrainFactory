
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
    public void GameClockViewSystem_ShouldSetTimeToSecondsPassedDividedByTen() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 
        int gcvEntity = EntityFactory.Add(w);
        w.SetComponent<Frame>(gcvEntity, new Frame(0, 0, 100, 100));
        w.SetComponent<TextBox>(gcvEntity, new TextBox("")); 
        w.SetComponent<GameClockView>(gcvEntity, GameClockView.Get());

        w.Update(); 

        Assert.Equal("0", w.GetComponent<TextBox>(gcvEntity).Text); 

        w.PassTime(seconds: 10); 
        w.Update(); 

        Assert.Equal("1", w.GetComponent<TextBox>(gcvEntity).Text); 
    }
}