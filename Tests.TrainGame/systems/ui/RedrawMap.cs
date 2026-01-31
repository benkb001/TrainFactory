
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 

public class RedrawMapSystemTest {
    private (World, Train, City, int) init() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RedrawMapSystem.Register(w); 

        Train t = TrainWrap.GetTestTrain();
        int e = TrainWrap.Add(w, t);
        int tuiEnt = EntityFactory.Add(w); 
        w.SetComponent<TrainUI>(tuiEnt, new TrainUI(t)); 
        w.SetComponent<MapUIFlag>(tuiEnt, MapUIFlag.Get());
        City dest = CityWrap.GetTest(); 
        t.ComingFrom.AddConnection(dest);
        return (w, t, dest, tuiEnt); 
    }

    [Fact]
    public void RedrawMapSystem_ShouldRedrawWhenTrainsEmbark() {
        (World w, Train t, City dest, int tuiEnt) = init(); 
        t.Embark(dest, w.Time); 
        w.Update(); 
        Assert.Single(w.GetComponentArray<DrawMapMessage>());
    }
}