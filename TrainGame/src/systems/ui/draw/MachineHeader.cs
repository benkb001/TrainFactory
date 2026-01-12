namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public static class SetMachineHeaderSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(MachineHeader), typeof(TextBox), typeof(Active)], (w, e) => {
            Machine m = w.GetComponent<MachineHeader>(e).GetMachine(); 

            string hStr = $"{m.Id}\n"; 
            hStr += $"Level: {m.Level}\n"; 
            hStr += $"Craft Speed: {m.GetCraftSpeedFormatted()}"; 
            hStr += $"Total {m.ProductItemId} Crafted: {m.LifetimeProductsCrafted}\n";
            hStr += $"Recipe: \n{m.GetRecipeFormatted()}"; 
            hStr += $"Stored: \n"; 

            foreach (KeyValuePair<string, int> kvp in m.Stored) {
                hStr += $"{kvp.Key}: {kvp.Value}\n";
            }

            w.GetComponent<TextBox>(e).Text = hStr; 
        });
    }
}