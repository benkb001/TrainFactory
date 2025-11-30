
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class StepperButtonSystemTest {
    [Fact]
    public void StepperButtonSystem_ShouldMakeAMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        StepperButtonSystem.Register(w); 

        int sb = EntityFactory.Add(w); 
        int step = EntityFactory.Add(w);  

        w.SetComponent<Button>(sb, new Button(true)); 
        w.SetComponent<StepperButton>(sb, new StepperButton(step, 10)); 

        w.Update(); 

        Assert.Single(w.GetComponentArray<StepperMessage>()); 

        StepperMessage generated = w.GetComponentArray<StepperMessage>().ToList()[0].Value; 
        Assert.Equal(step, generated.Entity); 
        Assert.Equal(10, generated.Delta); 
    }
}