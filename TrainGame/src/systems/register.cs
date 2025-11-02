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
        InteractSystem.RegisterInteract(w); 
        
        PauseButtonSystem.Register(w); 
        UnpauseButtonSystem.Register(w); 

        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);

        ChestInteractSystem.Register(w); 

        SceneSystem.RegisterPush(w); 
        SceneSystem.RegisterPop(w); 
        
        InventoryUISystem.RegisterBuild(w); 

        StepperButtonSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        InventoryControlSystem.RegisterUpdate(w); 
        
        LinearLayoutSystem.Register(w); 

        DragSystem.Register(w); 
        InventoryPickUpUISystem.Register(w); 
        InventoryDropUISystem.Register(w); 
        InventoryDragSystem.Register(w); 
        InventoryControlSystem.RegisterOrganize(w); 
        
        ButtonSystem.RegisterUnclick(w);
        InteractSystem.RegisterUninteract(w); 
    }
}