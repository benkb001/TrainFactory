
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class TrainTravelSystemTest() {
    [Fact]
    public void TrainTravelSystem_ShouldMakeTrainsArriveAfterTheCorrectAmountOfTime() {
        World w = WorldFactory.Build(); 
        int trainEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 1, 1); 
        City c_start = new City("Start", inv, realX: 0f); 
        City c_end = new City("End", inv, realX: 100f); 
        Train t = new Train(inv, c_start, Id: "Bug", milesPerHour: 10f);
        t.Embark(c_end, w.Time); 
        w.SetComponent<Train>(trainEntity, t); 
        w.SetComponent<Data>(trainEntity, Data.Get()); 
        w.PassTime(new WorldTime(hours: 9, minutes: 59)); 
        w.Update(); 
        Assert.True(t.IsTraveling());
        Assert.Equal(c_start, t.ComingFrom); 
        w.PassTime(new WorldTime(minutes: 1));
        w.Update(); 
        Assert.False(t.IsTraveling()); 
        Assert.Equal(c_end, t.ComingFrom); 
    }
}