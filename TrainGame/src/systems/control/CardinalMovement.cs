namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class CardinalMovementSystem() {
    private static Type[] types = [typeof(CardinalMovement), typeof(Frame), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        Vector2 v = Vector2.Zero; 
        float speed = w.GetComponent<CardinalMovement>(e).Speed; 
        if (VirtualKeyboard.IsPressed(KeyBinds.MoveLeft)) {
            v += new Vector2(-speed, 0);
        }

        if (VirtualKeyboard.IsPressed(KeyBinds.MoveRight)) {
            v += new Vector2(speed, 0); 
        }

        if (VirtualKeyboard.IsPressed(KeyBinds.MoveUp)) {
            v += new Vector2(0, -speed); 
        }

        if (VirtualKeyboard.IsPressed(KeyBinds.MoveDown)) {
            v += new Vector2(0, speed); 
        }

        if (v != Vector2.Zero) {
            v = Vector2.Normalize(v); 
            v *= speed; 
        }

        w.SetComponent<Velocity>(e, new Velocity(v)); 
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}