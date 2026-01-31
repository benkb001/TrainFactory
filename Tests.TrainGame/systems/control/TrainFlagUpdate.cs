using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class TrainFlagUpdateSystemTest {
    [Fact]
    public void TrainFlagUpdateSystemShouldClearIsEmbarkingFlag() {
        World w = WorldFactory.Build(); 
        Train t = TrainWrap.GetTestTrain(); 
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Train>(e, t); 
        City c = CityWrap.GetTest();
        t.ComingFrom.AddConnection(c);
        t.Embark(c, w.Time);
        Assert.True(t.IsEmbarking); 
        w.Update(); 
        Assert.False(t.IsEmbarking); 
    }
}