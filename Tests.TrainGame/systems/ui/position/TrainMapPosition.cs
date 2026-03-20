using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class TrainMapPositionSystemTest {
    [Fact]
    public void TrainMapPositionSystem_ShouldSetTrainsToTheCorrectPositionBasedOnTimePassed() {
        World w = WorldFactory.Build(); 

        Train train = TrainWrap.GetTest();
        int trainDataEntity = EntityFactory.AddData<Train>(w, train);
        
        train.Embark(new Vector2(100, 0), new WorldTime()); 

        int trainUIEntity = EntityFactory.Add(w); 
        TrainUI tUI = new TrainUI(train, trainDataEntity); 
        w.SetComponent<TrainUI>(trainUIEntity, tUI); 
        w.SetComponent<MapUIFlag>(trainUIEntity, MapUIFlag.Get());
        w.SetComponent<Frame>(trainUIEntity, new Frame(0, 0, 10, 10)); 
        w.PassTime(new WorldTime(minutes: 1)); 
        w.Update(); 

        //train is moving to right so the ui component should've moved to the right
        Assert.True(w.GetComponent<Frame>(trainUIEntity).Position.X > 0);
    }
}