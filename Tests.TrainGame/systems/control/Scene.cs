
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

[Collection("Sequential")]
public class SceneSystemTest {
    [Fact]
    public void SceneSystem_CurrentSceneShouldBeLastEnteredScene() {
        World w = WorldFactory.Build(); 
        SceneSystem.EnterScene(w, SceneType.Map);
        Assert.Equal(SceneType.Map, SceneSystem.CurrentScene); 

        SceneSystem.EnterScene(w, SceneType.RPG);
        Assert.Equal(SceneType.RPG, SceneSystem.CurrentScene); 
    }

    [Fact]
    public void SceneSystem_EnterShouldRemoveActiveComponentsFromEntitiesInOtherScenes() {
        World w = WorldFactory.Build(); 
        SceneSystem.EnterScene(w, SceneType.Map); 
        int e = EntityFactory.Add(w, type: SceneType.Map); 
        Assert.True(w.ComponentContainsEntity<Active>(e)); 
        SceneSystem.EnterScene(w, SceneType.RPG); 
        Assert.False(w.ComponentContainsEntity<Active>(e)); 
    }
}