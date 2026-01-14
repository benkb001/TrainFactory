namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public static class PurchaseClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<PurchaseButton>(w, (w, e) => {
            PurchaseButton btn = w.GetComponent<PurchaseButton>(e); 
            Inventory dest = btn.Dest; 
            int count = btn.ItemCount; 
            string itemID = btn.ItemID; 

            int addAttempt = dest.Add(itemID, count);

            if (addAttempt != count || !dest.TakeRecipe(btn.Cost)) {
                dest.Take(itemID, addAttempt); 
            } 
        });
    }
}