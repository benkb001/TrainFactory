
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class PopFactoryTest {
    [Fact]
    public void PopFactory_ShouldCreateAPopMessage() {
        World w = new World(); 
        
        RegisterComponents.All(w); 
        //to bypass error on not registering any systems
        CardinalMovementSystem.Register(w); 
        PopFactory.Build(w); 
        Assert.Single(w.GetMatchingEntities([typeof(PopSceneMessage)])); 
    }
    //TODO: Test other branch poplate
}