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
        //CameraLockSystem.Register(w); 

        CardinalMovementSystem.Register(w); 
        MovementSystem.Register(w); 
        TrainTravelSystem.Register(w); 
        MachineUpdateSystem.Register(w); 

        ButtonSystem.RegisterClick(w);
        InteractSystem.RegisterInteract(w); 
        
        PauseButtonSystem.Register(w);
        UnpauseButtonSystem.Register(w); 
        
        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);

        ChestInteractSystem.Register(w); 
        TrainInteractSystem.Register(w); 
        CityClickSystem.Register(w); 
        TrainClickSystem.Register(w); 
        EmbarkClickSystem.Register(w); 
        MachineUIClickSystem.Register(w); 
        MachineRequestClickSystem.Register(w); 
        PlayerAccessTrainClickSystem.Register(w); 
        OpenMapSystem.Register(w); 

        CloseMenuSystem.Register(w); 
        RedrawMapSystem.Register(w); 

        SceneSystem.RegisterPop(w); 
        SceneSystem.RegisterPush(w); 

        DrawBackgroundSystem.Register(w); 
        DrawCityDetailsSystem.Register(w); 
        DrawEmbarkSystem.Register(w); 
        InventoryUISystem.RegisterBuild(w); 
        DrawMapSystem.Register(w); 
        TrainMapPositionSystem.Register(w);
        DrawCitySystem.Register(w); 
        DrawMachineRequestSystem.Register(w);
        DrawMachinesViewSystem.Register(w); 
        DrawTrainsViewSystem.Register(w);  

        CameraReturnSystem.Register(w); 

        StepperButtonSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        InventoryControlSystem.RegisterUpdate(w); 
        
        LinearLayoutSystem.Register(w);
        LabelPositionSystem.Register(w); 

        DragSystem.Register(w); 
        InventoryPickUpUISystem.Register(w); 
        InventoryDropUISystem.Register(w); 
        InventoryDragSystem.Register(w); 
        InventoryControlSystem.RegisterOrganize(w); 

        ButtonSystem.RegisterUnclick(w);
        InteractSystem.RegisterUninteract(w); 
    }
}