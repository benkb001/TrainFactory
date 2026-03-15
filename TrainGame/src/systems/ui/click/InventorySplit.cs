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

public class InventorySplitSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Inventory.Item), typeof(CurrentInventory), typeof(Button), typeof(Active)], (w, e) => {
            if (w.GetComponent<Button>(e).ClickType == Click.Right) {
                Inventory.Item item = w.GetComponent<Inventory.Item>(e); 
                Inventory inv = w.GetComponent<CurrentInventory>(e).Inv;
                (int row, int col) = inv.GetIndices(item);

                int half = item.Count / 2; 
                Inventory.Item taken = inv.Take(row, col, half); 
                int num_taken = taken.Count; 
                int num_added = inv.Add(taken, newCell: true);
                inv.Add(item.Id, num_taken - num_added); 
            }
        }); 
    }
}