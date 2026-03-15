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
            City c = data.GetCity(); 
            string vendorID = data.VendorID; 
            Inventory inv = c.Inv; 

            (float invWidth, float invHeight) = InventoryWrap.GetUI(inv, 0.8f);

            //make outer container 
            LinearLayoutContainer outer = LinearLayoutWrap.AddOuter(w);

            //make vendor container, a button for each product

            LinearLayoutContainer vendor = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                w.ScreenWidth - 20, 
                w.ScreenHeight - invWidth - 20, 
                direction: "horizontal", 
                usePaging: true, 
                childrenPerPage: 3
            );

            foreach (PurchaseInfo<IBuyable> purchaseInfo in VendorID.ProductMap[vendorID]) {
                IBuyable buyable = purchaseInfo.Buyable; 
                int btnEnt = -1;
                
                if (buyable is PurchaseItem item) {
                    PurchaseButton<PurchaseItem> pb = new PurchaseButton<PurchaseItem>(item);
                    btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, 
                    text: $"Purchase {item.Count} {item.ItemID}?\n {Util.FormatMap(item.GetCost())}", 
                        setOutline: true, setButton: true);
                    w.SetComponent<PurchaseButton<PurchaseItem>>(btnEnt, pb);
                } else if (buyable is ResetHP _) {
                    //this is very gross but whatever, 
                    //basically just use the resetHP as the type check 
                    //and then here we can calculate the price and pass this 
                    //to the button. We can't calculate the price from the constant
                    
                    Inventory dest = CityWrap.GetCityWithPlayer(w).Inv; 
                    Health playerHP = PlayerWrap.GetHP(w);
                    
                    int credits = VendorID.GetResetHPCost(dest, playerHP);
                    PurchaseButton<ResetHP> pb = new PurchaseButton<ResetHP>(new ResetHP(credits, dest));
                    Dictionary<string, int> cost = new() {
                        [ItemID.Credit] = credits
                    };

                    btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, 
                        text: $"Reset HP?\n{Util.FormatMap(cost)}", setOutline: true, 
                        setButton: true);
                    w.SetComponent<PurchaseButton<ResetHP>>(btnEnt, pb);
                } else {
                    throw new InvalidOperationException("Unimplemented purchase button type");
                }
                
                LinearLayoutWrap.AddChild(w, btnEnt, vendor); 
            }

            LinearLayoutWrap.ResizeChildren(w, vendor);
            LinearLayoutWrap.AddChild(w, vendor.GetParentEntity(), outer); 

            //add city inv to bottom
            
            InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 
                invWidth, invHeight, DrawLabel: true);
            LinearLayoutWrap.AddChild(w, invView.GetParentEntity(), outer); 

        });
    }
}