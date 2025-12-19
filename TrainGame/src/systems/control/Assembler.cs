namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 

public class AssemblerSystem {
    public static void Register<T>(World world) where T : IAssembler {
        world.AddSystem([typeof(T), typeof(Data)], (w, e) => {
            T asm = w.GetComponent<T>(e); 

            if (asm.GetMachine().CraftComplete) {
                asm.Assemble(); 
            }
        }); 
    }
}