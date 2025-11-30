
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 

public class RedrawMapSystemTest {
    [Fact]
    public void RedrawMapSystem_ShouldRedrawWhenReturningToMapScene() {
        World w = new World(); 
        RegisterComponents.All(w); 

        RedrawMapSystem.Register(w);
        SceneSystem.RegisterPop(w); 
        SceneSystem.RegisterPush(w); 

        int e = EntityFactory.Add(w); 
        w.SetComponent<MapUIFlag>(e, MapUIFlag.Get()); 

        int pm = EntityFactory.Add(w); 
        w.SetComponent<PushSceneMessage>(pm, PushSceneMessage.Get()); 

        w.Update(); 

        int popMsg = EntityFactory.Add(w); 
        w.SetComponent<PopSceneMessage>(popMsg, PopSceneMessage.Get()); 

        w.Update(); 

        Assert.Single(w.GetComponentArray<DrawMapMessage>()); 
    }
}