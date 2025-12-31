
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

    //todo: write
}