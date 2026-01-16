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
    public static void Register<T, U>(World world) where T : IAssembler<U> {
        world.AddSystem([typeof(T), typeof(Data)], (w, e) => {
            T asm = w.GetComponent<T>(e); 

            if (asm.GetMachine().CraftComplete) {
                U assembled = asm.Assemble(); 
                EntityFactory.AddData<U>(w, assembled); 

                //dont love it but dont want to remove generics for assembly registering
                if (assembled is Train t) {
                    TrainWrap.Add(w, t); 
                }
            }
        }); 
    }
}