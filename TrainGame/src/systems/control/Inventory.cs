namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class InventoryPosition {
    public int Row; 
    public int Column; 

    public InventoryPosition(int Row, int Column) {
        this.Row = Row; 
        this.Column = Column; 
    }

    public void Deconstruct(out int row, out int column) {
        row = Row;
        column = Column;
    }
}

public static class InventoryControlSystem {
    public static void RegisterOrganize(World world) {
        Type[] ts = [typeof(InventoryOrganizeMessage), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            InventoryOrganizeMessage msg = w.GetComponent<InventoryOrganizeMessage>(e); 

            Inventory curInv = msg.CurInv;
            Inventory targetInv = msg.TargetInv;
            Draggable d = msg.CurDraggable; 
            Vector2 targetVector = msg.TargetVector; 

            int curRow = msg.CurRow; 
            int curCol = msg.CurColumn; 
            int targetRow = msg.TargetRow; 
            int targetCol = msg.TargetColumn;

            d.SnapPosition = targetVector; 

            if (!(curInv.AreValidIndices(curRow, curCol) && targetInv.AreValidIndices(targetRow, targetCol))) {
                w.RemoveEntity(e); 
                Console.WriteLine($"Invalid InvOrganizeMessage {curInv.ID}, {targetInv.ID}");
                return;
            }

            curInv.AddItemTo(targetInv, curRow, curCol, targetRow, targetCol);
            Inventory[] invs = {targetInv, curInv}; 

            foreach (Inventory inv in invs) {
                int invEnt = InventoryWrap.GetEntity(inv.ID, w);
                bool updated = w.SetComponentSafe<InventoryUpdatedFlag>(invEnt, InventoryUpdatedFlag.Get());
                Console.WriteLine($"updated? : {updated}, invent: {invEnt}");
            }

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
                    TextBox tb = w.GetComponent<TextBox>(c); 
                    
                    tb.Text = i.ToString(); 
                    w.SetComponent<Inventory.Item>(c, i);
                    w.SetComponent<InventoryPosition>(c, new InventoryPosition(row_index, j));
                    w.SetComponent<CurrentInventory>(c, new CurrentInventory(inv));
                }
            }
        };

        world.AddSystem(ts, tf); 
    }
}

