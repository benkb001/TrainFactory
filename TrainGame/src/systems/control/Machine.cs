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
        
        if ((m.RequestedAmount >= m.ProductCount || m.ProduceInfinite) && m.InvHasRequiredItems()) {
            if (m.CraftComplete) {
                m.FinishRecipe(); 
            }
            m.UpdateCrafting();  
        }
    }; 
}