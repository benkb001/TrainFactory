namespace TrainGame.Systems;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 

public static class RegisterSystems {
    public static void All(World w) {
        CardinalMovementSystem.Register(w); 
        MovementSystem.Register(w); 

        ButtonSystem.RegisterClick(w);
        
        PauseButtonSystem.Register(w); 
        UnpauseButtonSystem.Register(w); 
        SceneSystem.RegisterPush(w); 
        SceneSystem.RegisterPop(w); 

        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);

        InventoryUISystem.RegisterBuild(w); 

        StepperButtonSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        InventoryUISystem.RegisterUpdate(w); 
        LinearLayoutSystem.Register(w); 

        DragSystem.Register(w); 
        InventoryDragSystem.Register(w); 
        
        ButtonSystem.RegisterUnclick(w);
    }
}