
using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class PauseButtonSystemTest {
    private void RegisterDependencies(World w) {
        CardinalMovementSystem.Register(w); 
        MovementSystem.Register(w); 

        ButtonSystem.RegisterClick(w);
        
        PauseButtonSystem.Register(w); 
        UnpauseButtonSystem.Register(w); 
        //SceneSystem.RegisterPush(w); 
        //SceneSystem.RegisterPop(w); 

        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);

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
    public void PauseButtonSystem_ShouldGenerateMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 

        int pauseBtnEntity = EntityFactory.Add(w); 
        w.SetComponent<Button>(pauseBtnEntity, new Button(true)); 
        w.SetComponent<PauseButton>(pauseBtnEntity, PauseButton.Get()); 

        w.Update(); 

        Assert.Single(w.GetComponentArray<PushSceneMessage>()); 
    }

    [Fact]
    public void UnpauseButtonSystem_ShouldGenerateMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 

        int unpauseButton = EntityFactory.Add(w);  
        w.SetComponent<Button>(unpauseButton, new Button(true)); 
        w.SetComponent<UnpauseButton>(unpauseButton, UnpauseButton.Get()); 

        w.Update(); 

        Assert.Single(w.GetComponentArray<PopSceneMessage>()); 
    }
}