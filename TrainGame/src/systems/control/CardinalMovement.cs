namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

//TODO: Implement virtual keyboard, then Test
public class CardinalMovementSystem() {
    private static Type[] types = [typeof(CardinalMovement), typeof(Frame), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        Vector2 v = Vector2.Zero; 
        float speed = w.GetComponent<CardinalMovement>(e).Speed; 
        KeyboardState kbstate = Keyboard.GetState(); 
        if (kbstate.IsKeyDown(KeyBinds.MoveLeft)) {
            v += new Vector2(-speed, 0);
        }

        if (kbstate.IsKeyDown(KeyBinds.MoveRight)) {
            v += new Vector2(speed, 0); 
        }

        if (kbstate.IsKeyDown(KeyBinds.MoveUp)) {
            v += new Vector2(0, -speed); 
        }

        if (kbstate.IsKeyDown(KeyBinds.MoveDown)) {
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