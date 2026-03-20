
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class EmbarkClickSystemTest {
    [Fact]
    public void EmbarkClickSystem_ShouldEmbarkClickedTrain() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 1, 1); 
        City cStart = new City("start", inv, 100f, 100f);
        City cEnd = new City("end", inv, 100f, 100f);
        cStart.AddConnection(cEnd);
        Train t = TrainWrap.GetTest();
        int trainEnt = EntityFactory.AddData<Train>(w, t);
        w.SetComponent<ComingFromCity>(trainEnt, new ComingFromCity(cStart));
        
        Assert.False(t.IsTraveling()); 

        int embarkEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(embarkEntity, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Button>(embarkEntity, new Button(true)); 
        w.SetComponent<EmbarkButton>(embarkEntity, new EmbarkButton(cEnd, t, trainEnt));

        w.Update(); 
        Assert.Equal(cEnd, w.GetComponent<GoingToCity>(trainEnt)); 
        Assert.True(t.IsTraveling()); 
    }
}