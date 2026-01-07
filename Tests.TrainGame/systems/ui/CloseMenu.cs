using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 

[Collection("Sequential")]
public class CloseMenuSystemTest {
    [Fact]
    public void CloseMenuSystem_ShouldChangeActiveScene() {
        World w = WorldFactory.Build(); 
        SceneSystem.EnterScene(w, SceneType.Map); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Menu>(e, new Menu()); 
        VirtualKeyboard.Press(KeyBinds.Interact);
        w.Update(); 
        Assert.NotEqual(SceneType.Map, SceneSystem.CurrentScene); 
    }
}
