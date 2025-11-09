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
public class InteractSystemTest {
    [Fact]
    public void InteractSystem_ShouldSetInteractedToTrueIfTouchingAndInteractKeyPressed() {
        VirtualKeyboard.Reset(); 

        World w = new World(); 
        RegisterComponents.All(w); 
        InteractSystem.RegisterInteract(w); 

        int interactableEntity = EntityFactory.Add(w);
        w.SetComponent<Frame>(interactableEntity, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Interactable>(interactableEntity, new Interactable()); 

        int interactorEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(interactorEntity, new Frame(10, 0, 10, 10)); 
        w.SetComponent<Interactor>(interactorEntity, Interactor.Get()); 

        VirtualKeyboard.Press(KeyBinds.Interact); 
        
        w.Update(); 

        Assert.True(w.GetComponent<Interactable>(interactableEntity).Interacted); 
        VirtualKeyboard.Reset(); 
    }

    [Fact]
    public void InteractSystem_ShouldSetInteractedToFalseAfterUpdate() {
        VirtualKeyboard.Reset(); 

        World w = new World(); 
        RegisterComponents.All(w); 
        InteractSystem.RegisterInteract(w); 
        InteractSystem.RegisterUninteract(w); 

        int interactableEntity = EntityFactory.Add(w);
        w.SetComponent<Interactable>(interactableEntity, new Interactable(true)); 

        Assert.True(w.GetComponent<Interactable>(interactableEntity).Interacted); 
        w.Update(); 
        Assert.False(w.GetComponent<Interactable>(interactableEntity).Interacted); 
    }
}