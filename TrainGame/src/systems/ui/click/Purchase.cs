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
        ClickAndHoldSystem.Register<PurchaseButton<PurchaseItem>>(w, (w, e, btn) => {

            PurchaseItem i = w.GetComponent<PurchaseButton<PurchaseItem>>(e).Buyable; 
            Inventory dest = CityWrap.GetCityWithPlayer(w).Inv;
            int count = i.Count; 
            string itemID = i.ItemID; 

            int addAttempt = dest.Add(itemID, count);

            if (addAttempt != count || !dest.TakeRecipe(btn.Cost)) {
                dest.Take(itemID, addAttempt); 
            } 
        });
    }

    public static void RegisterResetHP(World w) {
        ClickSystem.Register<PurchaseButton<ResetHP>>(w, (w, e, btn) => {
            ResetHP r = w.GetComponent<PurchaseButton<ResetHP>>(e).Buyable; 
            Inventory dest = r.Dest; 
            int credits = r.Credits; 

            Dictionary<string, int> cost = new() {
                [ItemID.Credit] = credits
            };

            if (dest.TakeRecipe(cost)) {
                PlayerStats.Reset(w);
                //TODO: probably should not be this given it might not be a text box in the future 
                w.RemoveComponent<TextBox>(e);
            }
        });
    }
}