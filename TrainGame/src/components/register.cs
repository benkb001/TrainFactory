namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 
using TrainGame.Systems;

public static class RegisterComponents {
    public static void All(World w) {
        
        void r<T>() {
            w.AddComponentType<T>(); 
        }

        r<Active>(); 
        r<AddCartButton>(); 
        r<AddCartInterfaceButton>(); 
        r<Background>(); 
        r<Backgrounds>(); 
        r<Body>();
        r<Button>(); 
        r<CameraReturn>();
        r<CardinalMovement>(); 
        r<Cart>(); 
        r<CartAssembler>();
        r<Chest>(); 
        r<City>(); 
        r<CityUI>(); 
        r<ClearLLMessage>(); 
        r<Collidable>(); 
        r<Data>(); 
        r<Draggable>(); 
        r<DrawAddCartInterfaceMessage>(); 
        r<DrawButtonMessage<AddCartInterfaceButton>>(); 
        r<DrawButtonMessage<UpgradeTrainButton>>(); 
        r<DrawBackgroundMessage>(); 
        r<DrawCallback>(); 
        r<DrawCityDetailsMessage>(); 
        r<DrawCityMessage>(); 
        r<DrawEmbarkMessage>();  
        r<DrawInventoryContainerMessage<Train>>(); 
        r<DrawMachineInterfaceMessage>(); 
        r<DrawMachineRequestMessage>(); 
        r<DrawMachinesViewMessage>(); 
        r<DrawMapMessage>(); 
        r<DrawSetTrainProgramInterfaceMessage>();
        r<DrawTrainInterfaceMessage>(); 
        r<DrawTrainsViewMessage>();
        r<DrawTravelingInterfaceMessage>();
        r<EmbarkButton>(); 
        r<Frame>(); 
        r<GameClockView>();
        r<HeldItem>(); 
        r<Interactable>(); 
        r<Interactor>();  
        r<Inventory>(); 
        r<InventoryContainer<Train>>(); 
        r<InventoryIndexer<Train>>(); 
        r<Inventory.Item>();
        r<InventoryOrganizeMessage>();
        r<Machine>(); 
        r<MachinePriorityStepper>();
        r<MachineRequestButton>(); 
        r<MachineStorageStepper>();
        r<MachineUI>(); 
        r<ManualCraftButton>(); 
        r<MapUIFlag>();
        r<Menu>(); 
        r<Message>(); 
        r<Label>(); 
        r<LinearLayout>(); 
        r<Lines>(); 
        r<LLChild>(); 
        r<NextDrawTestButton>();
        r<NextDrawTestControl>(); 
        r<Outline>(); 
        r<PauseButton>(); 
        r<PlayerAccessTrainButton>(); 
        r<PopLateMessage>(); 
        r<PushSceneMessage>(); 
        r<PopSceneMessage>(); 
        r<ProgressBar>(); 
        r<Scene>(); 
        r<SetTrainProgramButton>(); 
        r<SetTrainProgramInterfaceButton>(); 
        r<SlowTimeButton>();
        r<SpeedTimeButton>(); 
        r<Sprite>(); 
        r<Stepper>(); 
        r<StepperButton>(); 
        r<StepperMessage>(); 
        r<TALBody>();
        r<TextBox>(); 
        r<Toast>(); 
        r<Train>(); 
        r<TrainAssembler>(); 
        r<TrainUI>(); 
        r<TrainYard>(); 
        r<UnpauseButton>(); 
        r<UpgradeTrainButton>(); 
        r<Velocity>(); 
    }
}