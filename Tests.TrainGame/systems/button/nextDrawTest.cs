
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class NextDrawTestButtonSystemTest {

    private void RegisterDependencies(World w) {
        CardinalMovementSystem.Register(w); 
        MovementSystem.Register(w); 

        ButtonSystem.RegisterClick(w);
        
        PauseButtonSystem.Register(w); 
        UnpauseButtonSystem.Register(w); 
        SceneSystem.RegisterPush(w); 
        SceneSystem.RegisterPop(w); 

        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        //NextDrawTestUISystem.Register(w);

        InventoryUISystem.RegisterBuild(w); 

        StepperButtonSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        InventoryControlSystem.RegisterUpdate(w); 
        
        LinearLayoutSystem.Register(w); 

        DragSystem.Register(w); 
        InventoryDragSystem.Register(w); 
        InventoryControlSystem.RegisterOrganize(w); 
        
        ButtonSystem.RegisterUnclick(w);
    }
    
    [Fact]
    public void NextDrawTestButtonSystem_ShouldMakeDrawMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 

        int nextDrawBtnEntity = w.AddEntity(); 
        w.SetComponent<Button>(nextDrawBtnEntity, new Button(true)); 
        w.SetComponent<Frame>(nextDrawBtnEntity, new Frame(0, 0, 10, 10)); 
        w.SetComponent<NextDrawTestButton>(nextDrawBtnEntity, new NextDrawTestButton(3)); 

        w.Update(); 

        Assert.Single(w.GetComponentArray<NextDrawTestControl>()); 
        NextDrawTestControl generated = w.GetComponentArray<NextDrawTestControl>().ToList()[0].Value; 

        Assert.Equal(4, generated.GetCurTest());
    }
}