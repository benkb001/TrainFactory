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

            if (!w.EntityExists(inventoryUI)) {
                inventoryUI = EntityFactory.Add(w); 
            }

            w.SetComponent<Inventory>(inventoryUI, inv); 

            if (dm.SetMenu) {
                w.SetComponent<Menu>(inventoryUI, Menu.Get()); 
            }
            
            LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
            ll.Padding = dm.Padding; 
            w.SetComponent<LinearLayout>(inventoryUI, ll);
            
            Vector2 drawPosition = w.WorldVector(dm.Position); 
            w.SetComponent<Frame>(inventoryUI, new Frame(drawPosition.X, drawPosition.Y, dm.Width, dm.Height, dm.Padding)); 
            w.SetComponent<Outline>(inventoryUI, new Outline(Depth: Constants.InventoryOutlineDepth)); 
            w.SetComponent<Background>(inventoryUI, new Background(Colors.UIBG, Depth: Constants.InventoryBackgroundDepth)); 

            int rows = inv.GetRows(); 
            int cols = inv.GetCols(); 
            float rowWidth = dm.Width - (dm.Padding * 2); 
            float rowHeight = (dm.Height - dm.Padding * (rows + 1)) / rows; 

            float cellHeight = rowHeight - dm.Padding * 2;
            float cellWidth = (rowWidth  - (dm.Padding * (cols + 1))) / cols; 

            for (int i = 0; i < rows; i++) {
                int row = EntityFactory.Add(w); 
                
                w.SetComponent<Frame>(row, new Frame(0, 0, rowWidth, rowHeight)); 
                w.SetComponent<Outline>(row, new Outline(Depth: Constants.InventoryRowOutlineDepth)); 
                w.SetComponent<Background>(row, new Background(Colors.UIAccent, Constants.InventoryRowBackgroundDepth)); 

                LinearLayout rowLL = new LinearLayout("horizontal", "alignLow"); 
                rowLL.Padding = dm.Padding; 
                w.SetComponent<LinearLayout>(row, rowLL);

                for (int j = 0; j < cols; j++) {
                    int cell = EntityFactory.Add(w); 

                    w.SetComponent<Frame>(cell, new Frame(0, 0, cellWidth, cellHeight));
                    w.SetComponent<Outline>(cell, new Outline(Depth: Constants.InventoryCellOutlineDepth)); 
                    w.SetComponent<Background>(cell, new Background(Colors.UIBG, Constants.InventoryCellBackgroundDepth)); 
                    Inventory.Item item = inv.Get(i, j); 
                    w.SetComponent<Inventory.Item>(cell, item); 
                    TextBox tb = new TextBox(item.ToString()); 
                    tb.Depth = Constants.InventoryCellTextBoxDepth; 
                    w.SetComponent<TextBox>(cell, tb);
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
}