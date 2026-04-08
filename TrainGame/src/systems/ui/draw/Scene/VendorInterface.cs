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

public static class DrawVendorInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<VendorInterfaceData>(w, (w, e) => {
            VendorInterfaceData data = w.GetComponent<DrawInterfaceMessage<VendorInterfaceData>>(e).Data; 
            string vendorID = data.VendorID; 
            Inventory src = data.Source;
            Inventory dest = data.Destination;

            (float invWidth, float invHeight) = InventoryWrap.GetUI(src, 0.8f);

            //make outer container 
            LinearLayoutContainer outer = LinearLayoutContainer.AddOuter(w);

            //make vendor container, a button for each product

            LinearLayoutContainer vendor = LinearLayoutContainer.Add(
                w, 
                Vector2.Zero, 
                w.ScreenWidth - 20, 
                w.ScreenHeight - invWidth - 20, 
                direction: "horizontal", 
                usePaging: true, 
                childrenPerPage: 3
            );

            RegisterBuyableContext ctx = new RegisterBuyableContext(w, src, dest);

            foreach (PurchaseInfo purchaseInfo in VendorID.ProductMap[vendorID]) {
                IBuyable buyable = purchaseInfo.Buyable; 
                int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, setOutline: true, setButton: true);
                BuyableRegistry.Add(ctx, buyable, btnEnt); 
                vendor.AddChild(btnEnt, w);
            }

            vendor.ResizeChildren(w); 
            outer.AddChild(vendor.GetParentEntity(), w); 

            //add city inv to bottom
            
            InventoryView invView = DrawInventoryCallback.Draw(w, src, Vector2.Zero, 
                invWidth, invHeight, DrawLabel: true);
            outer.AddChild(invView.GetParentEntity(), w); 

        });
    }
}

public class RegisterBuyableContext {
    public World W;
    public Inventory Source;
    public Inventory Destination; 

    public RegisterBuyableContext(World w, Inventory Source, Inventory Destination) {
        this.W = w; 
        this.Source = Source;
        this.Destination = Destination;
    }
}

public static class PurchaseItemCallback {
    public static void Register() {
        BuyableRegistry.Register<PurchaseItem>((ctx, item, e) => {
            World w = ctx.W;
            PurchaseButton<PurchaseItem> pb = new PurchaseButton<PurchaseItem>(item, item.GetCost(), ctx.Source);
            w.SetComponent<PurchaseButton<PurchaseItem>>(e, pb);
            Inventory dest = ctx.Destination;
            item.Destination = dest; 
            w.SetComponent<TextBox>(e, new TextBox($"Purchase {item.Count} {item.ItemID}?\n {Util.FormatMap(item.GetCost())}"));
        });
    }
}

public static class RegisterBuyableCallbacks {
    public static void All() {
        PurchaseItemCallback.Register();
        UpgradeDamageCallback.Register();
    }
}

public static class UpgradeDamageCallback {
    public static void Register() {
        BuyableRegistry.Register<PurchaseUpgradeGunDamage>((ctx, upgrade, e) => {
            World w = ctx.W; 
            PlayerGun pg = EquipmentSlot<PlayerGun>.EquipmentMap[upgrade.ID];
            Dictionary<string, int> cost = VendorID.UpgradeGunDamageCost(upgrade.ID, pg.DamageLevel);
            PurchaseButton<PurchaseUpgradeGunDamage> pb = 
                new PurchaseButton<PurchaseUpgradeGunDamage>(upgrade, cost, ctx.Source); 
            w.SetComponent<PurchaseButton<PurchaseUpgradeGunDamage>>(e, pb); 
            PurchaseUpgradeGunDamageWrap.SetPurchaseText(w, pg, upgrade.ID, e);
            if (pg.DamageLevel >= pg.MaxDamageLevel) {
                w.RemoveComponent<Button>(e);
            }
        });
    }
}

public static class BuyableRegistry {
    private static CallbackRegistry<RegisterBuyableContext, IBuyable, int> registry = new();
    public static void Register<T>(Action<RegisterBuyableContext, T, int> cb) where T : IBuyable {
        registry.Register<T>(cb);
    }
    public static void Add(RegisterBuyableContext w, IBuyable b, int e) {
        registry.Callback(w, b, e); 
    }
}