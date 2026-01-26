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
            Machine m = asm.GetMachine(); 

            if (m.CraftComplete) {
                for (int i = 0; i < m.ProductDelivered; i++) {
                    U assembled = asm.Assemble(); 
                    int assembledEnt = EntityFactory.AddData<U>(w, assembled); 

                    //TODO: change this to make a RegisterAssembledMessage<U> 
                    if (assembled is Train t) {
                        TrainWrap.Add(w, t); 
                    }
                }
            }
        }); 
    }
}