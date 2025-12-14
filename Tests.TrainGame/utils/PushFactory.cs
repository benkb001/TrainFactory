
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class PushFactoryTest {
    [Fact]
    public void PushFactory_ShouldCreateAPushMessage() {
        World w = new World(); 
        RegisterComponents.All(w); 
        //to bypass error on not registering any systems
        CardinalMovementSystem.Register(w); 
        PushFactory.Build(w); 
        Assert.Single(w.GetMatchingEntities([typeof(PushSceneMessage)])); 
    }
}