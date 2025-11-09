using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 


//sequential because global state (keyboard)
[Collection("Sequential")]
public class CloseMenuSystemTest {
    
    [Fact]
    public void CloseMenuSystem_ShouldCreateOnePopSceneMessageIfInteractClickedWhileAnActiveEntityHasAMenuComponent() {
        VirtualKeyboard.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        CloseMenuSystem.Register(w); 

        int e = w.AddEntity(); 
        w.SetComponent<Menu>(e, Menu.Get()); 
        w.SetComponent<Active>(e, Active.Get()); 

        VirtualKeyboard.Press(KeyBinds.Interact); 

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(PopSceneMessage)])); 

        VirtualKeyboard.Reset(); 
    }
}
