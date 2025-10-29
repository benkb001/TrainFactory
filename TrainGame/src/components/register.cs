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
        w.AddComponentType<Button>(); 
        w.AddComponentType<CardinalMovement>(); 
        w.AddComponentType<Collidable>(); 
        w.AddComponentType<Draggable>(); 
        w.AddComponentType<DrawInventoryMessage>(); 
        w.AddComponentType<Frame>(); 
        w.AddComponentType<GameClockView>(); 
        w.AddComponentType<Inventory>(); 
        w.AddComponentType<Inventory.Item>(); 
        w.AddComponentType<Message>(); 
        w.AddComponentType<LinearLayout>(); 
        w.AddComponentType<Lines>(); 
        w.AddComponentType<NextDrawTestButton>();
        w.AddComponentType<NextDrawTestControl>(); 
        w.AddComponentType<Outline>(); 
        w.AddComponentType<PauseButton>(); 
        w.AddComponentType<UnpauseButton>(); 
        w.AddComponentType<PushSceneMessage>(); 
        w.AddComponentType<PopSceneMessage>(); 
        w.AddComponentType<Scene>(); 
        w.AddComponentType<Sprite>(); 
        w.AddComponentType<Stepper>(); 
        w.AddComponentType<StepperButton>(); 
        w.AddComponentType<StepperMessage>(); 
        w.AddComponentType<TextBox>(); 
        w.AddComponentType<Toast>(); 
        w.AddComponentType<Velocity>(); 
    }
}