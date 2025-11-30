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
        int trainDataEntity = EntityFactory.Add(w, setScene: false); 

        Inventory inv = new Inventory("Test", 1, 1); 
        City c_start = new City("C_START", inv, uiX: 0f, uiY: 0f, realX: 0f, realY: 0f); 
        City c_end = new City("C_END", inv, uiX: 10f, uiY: 0f, realX: 100f, realY: 0f); 

        Train train = new Train(inv, c_start, milesPerHour: 10f); 
        train.Embark(c_end, new WorldTime()); 

        w.SetComponent<Train>(trainDataEntity, train);
        w.SetComponent<Data>(trainDataEntity, Data.Get()); 

        int trainUIEntity = EntityFactory.Add(w); 
        TrainUI tUI = new TrainUI(train); 
        w.SetComponent<TrainUI>(trainUIEntity, tUI); 
        w.SetComponent<MapUIFlag>(trainUIEntity, MapUIFlag.Get());
        w.SetComponent<Frame>(trainUIEntity, new Frame(0, 0, 10, 10)); 
        w.PassTime(new WorldTime(hours: 1)); 
        w.Update(); 

        Assert.Equal(w.GetComponent<Frame>(trainUIEntity).Position, train.GetMapPosition(new WorldTime(hours: 1))); 
    }
}