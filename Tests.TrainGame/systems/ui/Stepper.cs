
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class StepperUISystemTest {
    [Fact]
    public void StepperUISystem_ShouldIncrementStepper() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int stepperEntity = w.AddEntity(); 

        w.SetComponent<Stepper>(stepperEntity, new Stepper(10)); 
        
        int msg = w.AddEntity(); 
        w.SetComponent<StepperMessage>(msg, new StepperMessage(stepperEntity, 10));

        w.Update(); 
        Assert.Equal(20, w.GetComponent<Stepper>(stepperEntity).Value); 
        Assert.Equal("20", w.GetComponent<TextBox>(stepperEntity).Text); 
    }

    [Fact]
    public void StepperUISystem_ShouldRemoveStepperMessage() {
                World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int stepperEntity = w.AddEntity(); 

        w.SetComponent<Stepper>(stepperEntity, new Stepper(10)); 
        
        int msg = w.AddEntity(); 
        w.SetComponent<StepperMessage>(msg, new StepperMessage(stepperEntity, 10));

        w.Update(); 
        Assert.False(w.EntityExists(msg)); 
    }
}