using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class TrainInteractSystemTest {
    [Fact]
    public void TrainInteractSystem_ShouldCreateAPushMessageAndADrawMapMessage() {
        World w = new World(); 
        RegisterComponents.All(w); 
        TrainInteractSystem.Register(w); 

        int tEntity = EntityFactory.Add(w);
        Inventory inv = new Inventory("Test", 1, 1); 
        w.SetComponent<Train>(tEntity, new Train(inv, new City("start", inv, 100f, 100f))); 
        w.SetComponent<Interactable>(tEntity, new Interactable(true)); //notice interacted is set to true
        
        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(PushSceneMessage)])); 
        Assert.Single(w.GetMatchingEntities([typeof(DrawMapMessage)])); 
    }

}