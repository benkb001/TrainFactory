namespace TrainGame.Systems;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 
using TrainGame.Components; 

public static class RegisterSystems {
    public static void All(World w) {
        CameraLockSystem.Register(w); 
        SceneSystem.RegisterPopLate(w); 

        CardinalMovementSystem.Register(w); 
        MovementSystem.Register(w); 
        TALExecutionSystem.Register(w); //***** 
        TrainTravelSystem.Register(w); 
        w.AddSystem(MachineUpdateSystem.Ts, MachineUpdateSystem.Tf); 
        AssemblerSystem.Register<TrainAssembler, Train>(w); 
        AssemblerSystem.Register<CartAssembler, Cart>(w); 

        ButtonSystem.RegisterHold(w); 
        ButtonSystem.RegisterClick(w);
        InteractSystem.RegisterInteract(w); 
        
        PauseButtonSystem.Register(w);
        UnpauseButtonSystem.Register(w); 
        SpeedTimeClickSystem.Register(w); 
        SlowTimeClickSystem.Register(w); 
        
        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);
        CloseMenuSystem.Register(w); 
        ClearLLSystem.Register(w); 

        ChestInteractSystem.Register(w); 
        TrainInteractSystem.Register(w); 
        MachineInteractSystem.Register(w); 
        CityClickSystem.Register(w); 
        TrainClickSystem.Register(w); 
        EmbarkClickSystem.Register(w); 
        MachineUIClickSystem.Register(w); 
        MachineRequestClickSystem.Register(w); 
        PlayerAccessTrainClickSystem.Register(w); 
        AddCartClickSystem.Register(w); 
        AddCartInterfaceClickSystem.Register(w); 
        UpgradeTrainClickSystem.Register(w); 
        InventoryIndexSystem.Register<Train>(w); 

        OpenMapSystem.Register(w); 

        RedrawMapSystem.Register(w); //**** 

        SceneSystem.RegisterPop(w); 
        SceneSystem.RegisterPush(w);

        DrawBackgroundSystem.Register(w); 
        DrawCityDetailsSystem.Register(w); 
        DrawEmbarkSystem.Register(w); 
        DrawMapSystem.Register(w); 
        TrainMapPositionSystem.Register(w);
        DrawCitySystem.Register(w); 
        DrawMachineRequestSystem.Register(w);
        DrawMachinesViewSystem.Register(w); 
        DrawTrainsViewSystem.Register(w);  
        DrawCallbackSystem.Register(w); 
        HeldItemDrawSystem.Register(w); 
        DrawButtonSystem.Register<AddCartInterfaceButton>(w); 
        DrawButtonSystem.Register<UpgradeTrainButton>(w); 
        w.AddSystem(DrawAddCartInterfaceSystem.Ts, DrawAddCartInterfaceSystem.Tf); 
        DrawInventoryContainerSystem.Register<Train>(w); 
        DrawTrainInterfaceSystem.Register(w); 
        DrawTravelingInterfaceSystem.Register(w); 
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
        InventorySplitSystem.Register(w); 
        InventoryFastTransferSystem.Register(w); 

        CraftProgressBarUpdateSystem.Register(w); 
        ProgressBarUpdateSystem.Register(w); 
        
        ButtonSystem.RegisterUnclick(w);
        InteractSystem.RegisterUninteract(w); 
        TrainFlagUpdateSystem.Register(w); 
    }
}