

using System.Collections.Generic;
using System; 

using TrainGame.Components; 

public class InteractableTest {
    [Fact]
    public void Interactable_ShouldDefaultToNotClicked() {
        Interactable i = new Interactable(); 
        Assert.False(i.Interacted); 
    }

    [Fact]
    public void Interactable_ShouldRespectConstructorArguments() {
        Interactable i = new Interactable(false); 
        Assert.False(i.Interacted); 
        i = new Interactable(true); 
        Assert.True(i.Interacted); 
    }
}