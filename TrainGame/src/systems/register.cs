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
        //CameraLockSystem.Register(w); 

        CardinalMovementSystem.Register(w); 
        MovementSystem.RegisterCollision(w); 
        MovementSystem.Register(w); 
        TALExecutionSystem.Register(w);
        TrainTravelSystem.Register(w); 
        MachineUpdateSystem.Register(w); 
        AssemblerSystem.Register<TrainAssembler, Train>(w); 
        AssemblerSystem.Register<CartAssembler, Cart>(w); 

        ButtonSystem.RegisterClick(w); 
        ButtonSystem.RegisterHold(w);
        ButtonSystem.RegisterHighlight(w); 
        InteractSystem.RegisterInteract(w); 
        
        PauseButtonSystem.Register(w);
        UnpauseButtonSystem.Register(w); 
        SpeedTimeClickSystem.Register(w); 
        SlowTimeClickSystem.Register(w); 
        
        GameClockViewSystem.Register(w); 

        NextDrawTestButtonSystem.Register(w);
        NextDrawTestUISystem.Register(w);
        
        ClearLLSystem.Register(w); 

        ChestInteractSystem.Register(w); 
        TrainInteractSystem.Register(w); 
        MachineInteractSystem.Register(w); 
        CityClickSystem.Register(w); 
        TrainClickSystem.Register(w); 
        EmbarkClickSystem.Register(w); 
        MachineUIClickSystem.Register(w); 
        PlayerAccessTrainClickSystem.Register(w); 
        AddCartClickSystem.Register(w); 
        AddCartInterfaceClickSystem.Register(w); 
        UpgradeTrainClickSystem.Register(w); 
        InventoryIndexSystem.Register<Train>(w); 
        SetPlayerProgramClickSystem.Register(w); 
        SetTrainProgramClickSystem.Register(w);
        SetTrainProgramInterfaceClickSystem.Register(w); 
        LLPageSystem.Register(w); 
        LLPageSystem.RegisterScroll(w); 
        TextInputSystem.RegisterDeactivate(w); 
        TextInputSystem.RegisterActivate(w); 
        TextInputSystem.RegisterType(w); 
        TextInputSystem.RegisterFormat(w); 
        TextInputSystem.RegisterCopy(w); 
        SaveClickSystem.Register(w); 
        LoadClickSystem.Register(w); 
        UpgradeMachineClickSystem.Register(w); 
        EnterInterfaceClickSystem.Register<ViewProgramInterfaceData>(w); 
        EnterInterfaceClickSystem.Register<WriteProgramInterfaceData>(w); 
        ShootSystem.Register(w);

        CloseMenuSystem.Register(w); 
        
        OpenMapSystem.Register(w); 

        RedrawMapSystem.Register(w);

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
        DrawMachineInterfaceSystem.Register(w); 
        DrawSetTrainProgramInterfaceSystem.Register(w); 
        DrawCityInterfaceSystem.Register(w); 
        DrawWriteProgramInterfaceSystem.Register(w); 
        DrawViewProgramInterfaceSystem.Register(w); 
        SetMachineHeaderSystem.Register(w); 
        //CameraReturnSystem.Register(w); 

        StepperButtonSystem.Register(w); 
        SetMachinePrioritySystem.Register(w); 
        SetMachineStorageSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        InventoryControlSystem.RegisterUpdate(w); 
        
        PlayerInventoryPositionSystem.Register(w); 
        
        ScreenAnchorSystem.Register(w); 
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
        ManualCraftUpdateSystem.Register(w); 
        ProgressBarUpdateSystem.Register(w); 
        
        ButtonSystem.RegisterUnclick(w);
        ButtonSystem.RegisterClearHovered(w); 
        InteractSystem.RegisterUninteract(w); 
        TrainFlagUpdateSystem.Register(w); 
        MachineUpdateSystem.RegisterEndFrame(w); 
    }
}