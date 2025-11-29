
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class DrawMachineRequestSystemTest {
    [Fact]
    public void DrawMachineRequestSystem_ShouldMakeAMachineRequestButtonAStepperAndTwoStepperButtons() {
        World w = WorldFactory.Build(); 
        Machine m = new Machine(null, null, "", 0, 0);
        DrawMachineRequestMessage dm = new DrawMachineRequestMessage(m, 15f, 20f, new Vector2(5, 20), 1f);
        int dmEntity = EntityFactory.Add(w);
        w.SetComponent<DrawMachineRequestMessage>(dmEntity, dm); 

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(Stepper)])); 
        Assert.Equal(2, w.GetMatchingEntities([typeof(StepperButton)]).Count); 
        Assert.Single(w.GetMatchingEntities([typeof(MachineRequestButton)])); 
    }

    [Fact]
    public void DrawMachineRequestSystem_ShouldRemoveMessageEntity() {
        World w = WorldFactory.Build(); 
        Machine m = new Machine(null, null, "", 0, 0);
        DrawMachineRequestMessage dm = new DrawMachineRequestMessage(m, 15f, 20f, new Vector2(5, 20), 1f);
        int dmEntity = EntityFactory.Add(w);
        w.SetComponent<DrawMachineRequestMessage>(dmEntity, dm); 

        w.Update(); 

        Assert.False(w.EntityExists(dmEntity)); 
    }

    //todo: test that the steppers have the correct values (productCount on machine) 
}