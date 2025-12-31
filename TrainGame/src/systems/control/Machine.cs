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

public class MachineUpdateSystem {
    public static readonly Type[] Ts = [typeof(Machine), typeof(Data)]; 
    public static readonly Action<World, int> Tf = (w, e) => {
        Machine m = w.GetComponent<Machine>(e); 
        
        if (m.State == CraftState.Idle && (m.RequestedAmount >= m.ProductCount || m.ProduceInfinite)) {
            int numCraftable = m.GetNumCraftable();
            if (numCraftable > 0) {
                m.StartRecipe(numCraftable); 
            }
        }

        if (m.CraftComplete && m.State == CraftState.Crafting) {
            m.FinishRecipe(); 
        }

        if (m.State == CraftState.Delivering) {
            m.DeliverRecipe(); 
        }

        if (m.State == CraftState.Crafting) {
            m.UpdateCrafting(); 
        }
    }; 
}