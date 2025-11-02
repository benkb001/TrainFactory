namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class InventoryControlSystem() {
    public static void RegisterOrganize(World world) {
        Type[] ts = [typeof(InventoryOrganizeMessage), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            InventoryOrganizeMessage msg = w.GetComponent<InventoryOrganizeMessage>(e); 

            int targetRow = msg.TargetRow;
            int targetCol = msg.TargetColumn; 
            int curRow = msg.CurRow; 
            int curCol = msg.CurColumn; 
            Inventory.Item curItem = msg.CurItem; 
            Inventory.Item targetItem = msg.TargetItem; 
            Inventory curInv = curItem.Inv; 
            Inventory targetInv = targetItem.Inv; 
            Draggable d = msg.CurDraggable; 
            Vector2 targetVector = msg.TargetVector; 

            targetInv.Take(targetRow, targetCol);
            curInv.Take(curRow, curCol);

            if (targetItem.ItemId == curItem.ItemId) {
                curItem.Count += targetItem.Count;
                targetItem.Count = 0; 
                targetItem.ItemId = ""; 
            }
            
            targetInv.Add(curItem, targetRow, targetCol); 
            curInv.Add(targetItem, curRow, curCol);
            d.SnapPosition = targetVector; 
            w.RemoveEntity(e); 
        }; 
        
        world.AddSystem(ts, tf); 
    }

    public static void RegisterUpdate(World world) {
        Type[] ts = [typeof(Frame), typeof(LinearLayout), typeof(Inventory), typeof(Active)]; 

        Action<World, int> tf = (w, e) => {
            Inventory inv = w.GetComponent<Inventory>(e);
            LinearLayout ll = w.GetComponent<LinearLayout>(e); 
            Frame f = w.GetComponent<Frame>(e); 

            int rows = inv.GetRows(); 
            int cols = inv.GetCols(); 

            string invId = inv.GetId(); 
            List<int> row_entities = ll.GetChildren(); 

            for (int row_index = 0; row_index < row_entities.Count; row_index++) {
                int row = row_entities[row_index]; 
                LinearLayout rowLL = w.GetComponent<LinearLayout>(row); 
                List<int> children = rowLL.GetChildren(); 
                for (int j = 0; j < children.Count; j++) {
                    int c = children[j]; 
                    Inventory.Item i = inv.Get(row_index, j); 

                    w.GetComponent<TextBox>(c).Text = i.ToString(); 
                    w.SetComponent<Inventory.Item>(c, i);
                }
            }
        };

        world.AddSystem(ts, tf); 
    }
}

