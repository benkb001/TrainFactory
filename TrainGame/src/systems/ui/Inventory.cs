namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

public static class InventoryUISystem {

    public static void RegisterBuild(World world) {
        Type[] ts = [typeof(DrawInventoryMessage)]; 
        Action<World, int> tf = (w, e) => {
            DrawInventoryMessage dm = w.GetComponent<DrawInventoryMessage>(e); 
            Inventory inv = dm.Inv; 
            int inventoryUI = dm.Entity; 
            LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
            ll.Padding = dm.Padding; 
            w.SetComponent<LinearLayout>(inventoryUI, ll);
            w.SetComponent<Frame>(inventoryUI, new Frame(dm.Position.X, dm.Position.Y, dm.Width, dm.Height, dm.Padding)); 
            w.SetComponent<Outline>(inventoryUI, new Outline()); 

            int rows = inv.GetRows(); 
            int cols = inv.GetCols(); 
            float rowWidth = dm.Width - (dm.Padding * 2); 
            float rowHeight = (dm.Height / rows) - (dm.Padding * (rows + 1)); 

            float cellHeight = rowHeight - dm.Padding * 2;
            float cellWidth = (rowWidth / cols) - (dm.Padding * (cols + 1)); 

            for (int i = 0; i < rows; i++) {
                int row = w.AddEntity(); 
                
                w.SetComponent<Frame>(row, new Frame(0, 0, rowWidth, rowHeight)); 
                w.SetComponent<Outline>(row, new Outline()); 

                LinearLayout rowLL = new LinearLayout("horizontal", "alignLow"); 
                rowLL.Padding = dm.Padding; 
                w.SetComponent<LinearLayout>(row, rowLL);

                for (int j = 0; j < cols; j++) {
                    int cell = w.AddEntity(); 

                    w.SetComponent<Frame>(cell, new Frame(0, 0, cellWidth, cellHeight));
                    w.SetComponent<Outline>(cell, new Outline()); 
                    Inventory.Item item = inv.Get(i, j); 
                    w.SetComponent<Inventory.Item>(cell, item); 
                    w.SetComponent<TextBox>(cell, new TextBox(item.ToString()));
                    w.SetComponent<Draggable>(cell, new Draggable()); 
                    w.SetComponent<Button>(cell, new Button()); 
                    rowLL.AddChild(cell);
                }
                ll.AddChild(row); 
                
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
            float llWidth = f.GetWidth(); 
            float llHeight = f.GetHeight(); 

            float rowWidth = llWidth - (ll.Padding * 2); 
            float rowHeight = (llHeight / rows) - (ll.Padding * (rows + 1)); 

            float cellHeight = rowHeight - ll.Padding * 2;
            float cellWidth = (rowWidth / cols) - (ll.Padding * (cols + 1)); 

            string invId = inv.GetId(); 
            List<int> row_entities = ll.GetChildren(); 
            Random r = new Random(); 
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