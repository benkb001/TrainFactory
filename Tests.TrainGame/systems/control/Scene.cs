
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

public class SceneSystemTest {
    [Fact]
    public void SceneSystem_ShouldIncrementSceneWhenPushed() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        Assert.Equal(0, w.GetComponent<Scene>(e).Value);

        int push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 

        w.Update(); 
        Assert.Equal(1, w.GetComponent<Scene>(e).Value); 
    }

    [Fact]
    public void SceneSystem_ShouldDecrementSceneWhenPopped() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        w.SetComponent<Scene>(e, new Scene(1)); 
        
        int pop = w.AddEntity(); 
        w.SetComponent<PopSceneMessage>(pop, PopSceneMessage.Get()); 

        w.Update(); 
        Assert.Equal(0, w.GetComponent<Scene>(e).Value); 
    }

    [Fact]
    public void SceneSystem_ShouldNotSetScenesToNegativeValues() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        Assert.Equal(0, w.GetComponent<Scene>(e).Value); 

        int pop = w.AddEntity(); 
        w.SetComponent<PopSceneMessage>(pop, PopSceneMessage.Get());

        w.Update(); 
        Assert.Equal(0, w.GetComponent<Scene>(e).Value); 
    }

    [Fact]
    public void SceneSystem_ShouldRemoveActiveComponentFromEntitiesWithNonzeroScene() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        Assert.True(w.ComponentContainsEntity<Active>(e)); 

        int push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 
        w.Update(); 

        Assert.False(w.ComponentContainsEntity<Active>(e)); 
    }

    [Fact] 
    public void SceneSystem_ShouldAddActiveComponentToEntitiesWithSceneEqualToZero() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        Assert.True(w.ComponentContainsEntity<Active>(e)); 

        int push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 
        w.Update(); 

        Assert.False(w.ComponentContainsEntity<Active>(e));

        int pop = w.AddEntity(); 
        w.SetComponent<PopSceneMessage>(pop, PopSceneMessage.Get()); 
        w.Update(); 
        
        Assert.True(w.ComponentContainsEntity<Active>(e));
    }

    [Fact] 
    public void SceneSystem_PopShouldNotAddActiveComponentIfSceneGreaterThanZero() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int e = w.AddEntity(); 
        Assert.True(w.ComponentContainsEntity<Active>(e)); 

        int push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 
        w.Update(); 

        Assert.False(w.ComponentContainsEntity<Active>(e));

        push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 
        w.Update(); 
        
        Assert.False(w.ComponentContainsEntity<Active>(e));

        int pop = w.AddEntity(); 
        w.SetComponent<PopSceneMessage>(pop, PopSceneMessage.Get()); 
        w.Update(); 

        Assert.False(w.ComponentContainsEntity<Active>(e));
    }

    [Fact]
    public void SceneSystem_ShouldRemoveAllPushAndPopSceneMessages() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int push = w.AddEntity(); 
        w.SetComponent<PushSceneMessage>(push, PushSceneMessage.Get()); 

        int pop = w.AddEntity(); 
        w.SetComponent<PopSceneMessage>(pop, PopSceneMessage.Get());

        Assert.True(w.EntityExists(push)); 
        Assert.True(w.EntityExists(pop)); 

        w.Update(); 

        Assert.False(w.EntityExists(push)); 
        Assert.False(w.EntityExists(pop)); 
    }
}