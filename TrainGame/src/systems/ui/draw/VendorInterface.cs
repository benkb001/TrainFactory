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

            foreach (KeyValuePair<string, (Dictionary<string, int>, int)> kvp in VendorID.ProductMap[vendorID]) {
                string itemID = kvp.Key; 
                Dictionary<string, int> cost = kvp.Value.Item1; 
                int count = kvp.Value.Item2; 

                PurchaseButton pb = new PurchaseButton(itemID, cost, inv, count);
                int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, text: $"Purchase {itemID}? {Util.FormatMap(cost)}", 
                    setOutline: true, setButton: true);
                w.SetComponent<PurchaseButton>(btnEnt, pb); 
                vendor.AddChild(btnEnt, w); 
            }

            vendor.ResizeChildren(w); 
            outer.AddChild(vendor.GetParentEntity(), w); 

            //add city inv to bottom
            
            InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 
                invWidth, invHeight, DrawLabel: true);
            outer.AddChild(invView.GetParentEntity(), w); 

        });
    }
}