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

public static class RemoveInventoryUpdatedFlagSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(InventoryUpdatedFlag)], (w, e) => {
            w.RemoveComponent<InventoryUpdatedFlag>(e);
        });
    }
}