namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 

public static class RegisterComponents {
    public static void All(World w) {
        w.AddComponentType<Active>(); 
        w.AddComponentType<Background>(); 
        w.AddComponentType<Button>(); 
        w.AddComponentType<CameraReturn>();
        w.AddComponentType<CardinalMovement>(); 
        w.AddComponentType<Chest>(); 
        w.AddComponentType<City>(); 
        w.AddComponentType<CityUI>(); 
        w.AddComponentType<Collidable>(); 
        w.AddComponentType<Data>(); 
        w.AddComponentType<Draggable>(); 
        w.AddComponentType<DrawBackgroundMessage>(); 
        w.AddComponentType<DrawCityDetailsMessage>(); 
        w.AddComponentType<DrawCityMessage>(); 
        w.AddComponentType<DrawEmbarkMessage>(); 
        w.AddComponentType<DrawInventoryMessage>(); 
        w.AddComponentType<DrawMachineRequestMessage>(); 
        w.AddComponentType<DrawMachinesViewMessage>(); 
        w.AddComponentType<DrawMapMessage>(); 
        w.AddComponentType<DrawTrainsViewMessage>();
        w.AddComponentType<EmbarkButton>(); 
        w.AddComponentType<Frame>(); 
        w.AddComponentType<GameClockView>();
        w.AddComponentType<Interactable>(); 
        w.AddComponentType<Interactor>();  
        w.AddComponentType<Inventory>(); 
        w.AddComponentType<Inventory.Item>();
        w.AddComponentType<InventoryOrganizeMessage>();
        w.AddComponentType<Machine>(); 
        w.AddComponentType<MachineRequestButton>(); 
        w.AddComponentType<MachineUI>(); 
        w.AddComponentType<MapUIFlag>();
        w.AddComponentType<Menu>(); 
        w.AddComponentType<Message>(); 
        w.AddComponentType<Label>(); 
        w.AddComponentType<LinearLayout>(); 
        w.AddComponentType<Lines>(); 
        w.AddComponentType<NextDrawTestButton>();
        w.AddComponentType<NextDrawTestControl>(); 
        w.AddComponentType<Outline>(); 
        w.AddComponentType<PauseButton>(); 
        w.AddComponentType<PlayerAccessTrainButton>(); 
        w.AddComponentType<PushSceneMessage>(); 
        w.AddComponentType<PopSceneMessage>(); 
        w.AddComponentType<Scene>(); 
        w.AddComponentType<Sprite>(); 
        w.AddComponentType<Stepper>(); 
        w.AddComponentType<StepperButton>(); 
        w.AddComponentType<StepperMessage>(); 
        w.AddComponentType<TextBox>(); 
        w.AddComponentType<Toast>(); 
        w.AddComponentType<Train>(); 
        w.AddComponentType<TrainUI>(); 
        w.AddComponentType<UnpauseButton>(); 
        w.AddComponentType<Velocity>(); 
    }
}