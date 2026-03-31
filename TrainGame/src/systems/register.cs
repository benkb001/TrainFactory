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
        RegisterBulletTraits.All();
        RegisterMovementTypes.All();
        RegisterShootPatterns.All();
        RegisterEnemyTraits.All();

        CardinalMovementSystem.Register(w); 
        ParametricMovementSystem.Register(w);
        DefaultEnemyMovementSystem.Register(w); 
        ChaseMovementSystem.Register(w);
        CyclicalMoveSystem.Register(w);
        
        MovementSystem.RegisterPartition(w);
        MovementSystem.RegisterCollision(w);
        MovementSystem.Register(w);

        ParrySystem.RegisterStartParry(w); 
        ParrySystem.RegisterEndParry(w); 
        DamageSystem.RegisterShoot<Player, Enemy>(w); 
        DamageSystem.RegisterShoot<Enemy, Player>(w); 
        VampireDamageSystem.Register(w);
        DamageSystem.RegisterArmor(w); 
        DamageSystem.RegisterParry(w); 
        DamageSystem.RegisterAddInvincibleMessage(w);
        DamageSystem.RegisterReceive(w); 
        DamageSystem.RegisterHealShield(w);
        DamageSystem.RegisterSetInvincible(w);
        DamageSystem.RegisterDecayInvincibility(w);
        LootSystem.Register(w); 
        PlayerDeathSystem.Register(w); 
        DeathSystem.Register(w); 
        EnemySpawnSystem.Register(w); 
        CollideBulletSystem.Register(w);
        ApplyVampiredSystem.Register(w);
        RemoveVampiredSystem.Register(w);
        DecayBulletSystem.Register(w); 
        
        SplitBulletSystem.Register(w);
        PlayerShootSystem.Register(w);
        EnemyShootSystem.Register(w); 
        DefaultShootSystem.Register<Enemy>(w);
        DefaultShootSystem.Register<Player>(w);
        MeleeShootSystem.Register<Enemy>(w);
        MeleeShootSystem.Register<Player>(w);
        RadialShootSystem.Register<Enemy>(w);
        RadialShootSystem.Register<Player>(w);
        ShotgunShootSystem.Register<Player>(w);
        ShotgunShootSystem.Register<Enemy>(w);
        GridShootSystem.Register<Enemy>(w); 
        GridShootSystem.Register<Player>(w);
        RandomShotgunShootSystem.Register<Player>(w);
        RandomShotgunShootSystem.Register<Enemy>(w);
        BulletWarningShootSystem.Register(w); 
        HomingSystem.Register(w);

        RemoveExpiredSystem.Register(w);
        RemoveOnCollisionSystem.Register(w);
        RemoveOnHitSystem.Register(w);
        
        TALExecutionSystem.Register(w);
        TrainTravelSystem.RegisterMove(w);
        TrainTravelSystem.Register(w); 
        MachineUpdateSystem.Register(w); 
        MachineUpdateSystem.RegisterConsumeTimeCrystals(w); 
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

        TrainInteractSystem.Register(w); 
        CityClickSystem.Register(w); 
        TrainClickSystem.Register(w); 
        EmbarkClickSystem.Register(w); 
        AddCartClickSystem.Register(w); 
        AddCartInterfaceClickSystem.Register(w); 
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
        EnterInterfaceClickSystem.Register<EquipmentInterfaceData>(w); 
        EnterInterfaceClickSystem.Register<ViewProgramInterfaceData>(w); 
        EnterInterfaceClickSystem.Register<WriteProgramInterfaceData>(w); 
        EnterInterfaceClickSystem.Register<UpgradeTrainInterfaceData>(w);
        EnterInterfaceClickSystem.Register<MachineInterfaceData>(w);
        EnterInterfaceInteractSystem.Register<ElevatorInterfaceData>(w); 
        PurchaseClickSystem.Register(w); 
        PurchaseClickSystem.RegisterResetHP(w);
        UpgradeDepotClickSystem.Register(w); 
        ConnectCitiesClickSystem.Register(w);
        UpgradeTrainClickSystem.Register<UpgradeTrainPowerButton>(w);
        UpgradeTrainClickSystem.Register<UpgradeFuelConsumptionButton>(w);
        UpgradeInventoryExponentialClickSystem.Register(w);
        UpgradeMachineSpeedClickSystem.Register(w);
        UpgradeMachineProductCountClickSystem.Register(w);
        CloseMenuClickSystem.Register(w); 
        PauseTrainProgramButtonClickSystem.Register(w); 
        ElevatorSystem.Register(w); 

        EquipSystem.Register<Armor>(w); 
        
        TempArmorInteractSystem.Register(w); 
        HealthPotionInteractSystem.Register(w); 
        LootInteractSystem.Register(w); 
        DamagePotionInteractSystem.Register(w); 
        LadderInteractSystem.Register(w);
        
        VendorInteractSystem.Register(w);
        ToolSystem.Register(w);
        
        RewardInteractSystem.RegisterRemove(w); 

        CloseMenuSystem.Register(w); 

        RedrawMapSystem.Register(w);

        DrawVampiredSystem.Register(w);
        DrawBackgroundSystem.Register(w); 
        DrawHPSystem.Register(w); 
        DrawEmbarkSystem.Register(w); 
        DrawMapSystem.Register(w); 
        TrainMapPositionSystem.Register(w);
        DrawCitySystem.Register(w); 
        DrawMachineRequestSystem.Register(w);
        DrawCallbackSystem.Register(w); 
        HeldItemDrawSystem.Register(w); 
        w.AddSystem(DrawAddCartInterfaceSystem.Ts, DrawAddCartInterfaceSystem.Tf); 
        DrawInventoryContainerSystem.Register<Train>(w); 
        DrawTrainInterfaceSystem.Register(w); 
        DrawTravelingInterfaceSystem.Register(w); 
        DrawMachineInterfaceSystem.Register(w); 
        DrawSetTrainProgramInterfaceSystem.Register(w); 
        DrawCityInterfaceSystem.Register(w); 
        DrawWriteProgramInterfaceSystem.Register(w); 
        DrawViewProgramInterfaceSystem.Register(w); 
        DrawVendorInterfaceSystem.Register(w); 
        DrawEquipmentInterfaceSystem.Register(w); 
        DrawUpgradeTrainInterfaceSystem.Register(w);
        SetMachineHeaderSystem.Register(w); 
        DrawLadderSystem.Register(w);
        DrawElevatorInterfaceSystem.Register(w); 
        ToastSystem.RegisterDraw(w);
        StopDrawingVampiredSystem.Register(w);

        StepperButtonSystem.Register(w); 
        SetMachinePrioritySystem.Register(w); 
        SetMachineStorageSystem.Register(w); 
        StepperUISystem.Register(w); 
        
        ToastSystem.Register(w); 

        ExitExpiredTrainMenuSystem.Register(w); 

        InventoryControlSystem.RegisterUpdate(w); 
        
        PlayerHUDPositionSystem.Register(w); 
        DrawAmmoHUDSystem.Register(w); 
        
        ScreenAnchorSystem.Register(w);
        LinearLayoutSystem.Register(w);
        LabelPositionSystem.Register(w); 

        ProgressBarUpdateSystem.RegisterPosition(w); 

        DragSystem.Register(w); 
        InventoryPickUpUISystem.Register(w); 
        InventoryDropUISystem.Register(w); 
        InventoryDragSystem.Register(w); 
        InventoryControlSystem.RegisterOrganize(w); 
        InventorySplitSystem.Register(w); 
        InventoryFastTransferSystem.Register(w); 

        CraftProgressBarUpdateSystem.Register(w); 
        ParryHPBarSystem.Register(w); 
        ManualCraftUpdateSystem.Register(w); 
        ProgressBarUpdateSystem.Register(w); 
        
        ButtonSystem.RegisterUnclick(w);
        ButtonSystem.RegisterClearHovered(w); 
        InteractSystem.RegisterUninteract(w); 
        TrainFlagUpdateSystem.Register(w); 
        MachineUpdateSystem.RegisterEndFrame(w); 
        RemoveInventoryUpdatedFlagSystem.Register(w);
        RemoveShotMessageSystem.Register(w);
        RemoveEndVampiredSystem.Register(w);
    }
}