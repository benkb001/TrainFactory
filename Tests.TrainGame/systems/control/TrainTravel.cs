
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class TrainTravelSystemTest() {
    [Fact]
    public void TrainTravelSystem_ShouldMakeTrainsArriveAfterTheCorrectAmountOfTime() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 1, 1); 
        City c_start = new City("Start", inv, realX: 0f); 
        City c_end = new City("End", inv, realX: 100f); 

        EntityFactory.AddData<City>(w, c_start);
        EntityFactory.AddData<City>(w, c_end);

        c_start.AddConnection(c_end);
        Train t = new Train(inv, c_start.RealPosition, new Dictionary<CartType, Inventory>(), "TestTrain", milesPerHour: 10f);
        int trainEntity = EntityFactory.AddData<Train>(w, t);

        t.Embark(c_end.RealPosition, w.Time); 
        w.SetComponent<GoingToCity>(trainEntity, new GoingToCity(c_end));

        w.PassTime(new WorldTime(hours: 9, minutes: 59)); 
        w.Update(); 
        Assert.True(t.IsTraveling());
        Assert.Equal(c_start, w.GetComponent<ComingFromCity>(trainEntity)); 
        
        //TODO: maybe bug because its no longer passing if you set minutes to just 1
        w.PassTime(new WorldTime(minutes: 20));
        w.Update(); 
        Assert.False(t.IsTraveling()); 
        Assert.Equal(c_end, w.GetComponent<ComingFromCity>(trainEntity)); 
    }
}