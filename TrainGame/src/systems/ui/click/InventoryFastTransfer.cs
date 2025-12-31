namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Callbacks; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InventoryFastTransferSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Inventory.Item), typeof(Active), typeof(Button)], (w, e) => {
            if (w.GetComponent<Button>(e).ShiftClicked) {
                Inventory.Item item = w.GetComponent<Inventory.Item>(e); 
                string itemID = item.Id; 
                Inventory invClicked = item.Inv; 

                List<int> invEntities = w.GetMatchingEntities([typeof(Inventory), typeof(Active)]); 
                
                if (invEntities.Count == 2) {
                    Inventory invOther = invEntities.Select(invEnt => w.GetComponent<Inventory>(invEnt)).Where(
                        inv => inv != invClicked).FirstOrDefault(); 

                    if (invOther != default(Inventory)) {
                        Inventory.Item itemClicked = invClicked.Take(itemID, invClicked.ItemCount(itemID));
                        int taken = itemClicked.Count; 
                        int added = invOther.Add(itemClicked);  
                        invClicked.Add(new Inventory.Item(ItemId: itemID, Count: taken - added)); 
                    }
                }
            }
        }); 
    }
}