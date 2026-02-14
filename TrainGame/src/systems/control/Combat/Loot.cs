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
using TrainGame.Utils; 
using TrainGame.Constants;

public static class LootSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Loot), typeof(Health), typeof(Frame), typeof(Active)], (w, e) => {
            if (w.GetComponent<Health>(e).HP <= 0) {
                Loot loot = w.GetComponent<Loot>(e);
                int transferred = loot.Transfer(); 
                string itemID = loot.GetItemID(); 
                Vector2 pos = w.GetComponent<Frame>(e).Position; 
                int toastEnt = EntityFactory.AddToast(w, pos, 100, 30, $"+{transferred} {itemID}!");
                w.SetComponent<Velocity>(toastEnt, new Velocity(0, -1));
            }
        });
    }
}
