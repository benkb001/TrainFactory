
using TrainGame.Components; 
using TrainGame.ECS; 

public class DrawCallbackSystemTest {
    [Fact]
    public void DrawCallbackSystem_ShouldPerformSpecifiedCallback() {
        World w = WorldFactory.Build(); 
        DrawCallback cb = new DrawCallback(() => {
            int e = EntityFactory.Add(w); 
            w.SetComponent<Frame>(e, new Frame(0, 0, 1, 1)); 
        }); 

        int dmEntity = EntityFactory.Add(w); 
        w.SetComponent<DrawCallback>(dmEntity, cb); 
        w.Update(); 
        Assert.Single(w.GetMatchingEntities([typeof(Frame)])); 
    }

    [Fact]
    public void DrawCallbackSystem_ShouldDestroyEntity() {
        World w = WorldFactory.Build(); 
        DrawCallback cb = new DrawCallback(() => {
            int e = EntityFactory.Add(w); 
            w.SetComponent<Frame>(e, new Frame(0, 0, 1, 1)); 
        }); 

        int dmEntity = EntityFactory.Add(w); 
        w.SetComponent<DrawCallback>(dmEntity, cb); 
        w.Update(); 

        Assert.False(w.EntityExists(dmEntity)); 
    }
}