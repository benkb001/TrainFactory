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
        World w = new World();
        RegisterComponents.All(w);
        TrainMapPositionSystem.Register(w);

        Train train = TrainWrap.GetTest();
        train.SetPosition(0f, 0f);
        int trainDataEntity = EntityFactory.AddData<Train>(w, train);
        
        train.Embark(new Vector2(100, 0), new WorldTime()); 

        int trainUIEntity = EntityFactory.Add(w); 
        City start = new City("start", new Inventory("test",1,1));
        City end = new City("end", new Inventory("test",1,1), 100f, 100f, 100f, 100f);
        TrainUI tUI = new TrainUI(train, trainDataEntity, start, end); 

        w.SetComponent<TrainUI>(trainUIEntity, tUI); 
        w.SetComponent<MapUIFlag>(trainUIEntity, MapUIFlag.Get());
        w.SetComponent<Frame>(trainUIEntity, new Frame(0, 0, 10, 10)); 
        w.SetComponent<ComingFromCity>(trainUIEntity, new ComingFromCity(start));
        w.SetComponent<GoingToCity>(trainUIEntity, new GoingToCity(end));
        train.Move(new WorldTime(minutes: 1));
        w.Update(); 

        //train is moving to right so the ui component should've moved to the right
        Assert.True(w.GetComponent<Frame>(trainUIEntity).Position.X > 0);
    }
}