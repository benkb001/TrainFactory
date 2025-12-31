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

//TODO: test
public class InventorySplitSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Inventory.Item), typeof(Button), typeof(Active)], (w, e) => {
            if (w.GetComponent<Button>(e).ClickType == Click.Right) {
                Inventory.Item item = w.GetComponent<Inventory.Item>(e); 
                Inventory inv = item.Inv; 

                int half = item.Count / 2; 
                Inventory.Item taken = inv.Take(item.Row, item.Column, half); 
                inv.Add(taken, newCell: true);
            }
        }); 
    }
}