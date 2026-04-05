
using Microsoft.Xna.Framework;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 

public class RedrawMapSystemTest {
    private (World, Train, int) init() {
        World w = new World(); 
        City start = CityWrap.GetTest();
        City end = CityWrap.GetTest();
        RegisterComponents.All(w); 
        RedrawMapSystem.Register(w); 

        Train t = TrainWrap.GetTestTrain();
        int e = EntityFactory.AddData<Train>(w, t);
        int tuiEnt = EntityFactory.Add(w); 
        w.SetComponent<TrainUI>(tuiEnt, new TrainUI(t, e, start, end)); 
        w.SetComponent<MapUIFlag>(tuiEnt, MapUIFlag.Get());

        return (w, t, tuiEnt); 
    }

    [Fact]
    public void RedrawMapSystem_ShouldRedrawWhenTrainsEmbark() {
        (World w, Train t, int tuiEnt) = init(); 
        t.Embark(new Vector2(10, 10), w.Time); 
        w.Update(); 
        Assert.Single(w.GetComponentArray<DrawMapMessage>());
    }
}