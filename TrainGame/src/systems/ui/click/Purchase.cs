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
    private static void register<T>(World w, Action<World, int, PurchaseButton<T>> tf) where T : IBuyable {
        ClickSystem.Register<PurchaseButton<T>>(w, (w, e, btn) => {
            Inventory src = btn.Source;
            Dictionary<string, int> cost = btn.Cost; 
            if (src.TakeRecipe(cost)) {
                tf(w, e, btn);
            }
        });
    }
    
    private static void registerPurchaseItem(World w) {
        ClickAndHoldSystem.Register<PurchaseButton<PurchaseItem>>(w, (w, e, btn) => {
            PurchaseItem i = btn.Buyable;
            Inventory src = btn.Source;
            Inventory dest = i.Destination;

            int count = i.Count; 
            string itemID = i.ItemID; 

            int addAttempt = dest.Add(itemID, count);

            if (addAttempt != count || !src.TakeRecipe(btn.Cost)) {
                dest.Take(itemID, addAttempt); 
            } 
        });
    }

    private static void registerPurchaseGunUpgrade(World w) {
        register<PurchaseUpgradeGunDamage>(w, (w, e, btn) => {
            PurchaseUpgradeGunDamage upgrade = btn.Buyable; 
            PlayerGun pg = EquipmentSlot<PlayerGun>.EquipmentMap[upgrade.ID];
            pg.UpgradeDamage();
            BuyableRegistry.Add(new RegisterBuyableContext(w, btn.Source, btn.Source), upgrade, e);
            EquipmentRegistry.Add(w, pg, -1);
        });
    }


    public static void Register(World w) {
        registerPurchaseItem(w);
        registerPurchaseGunUpgrade(w);
    }
}