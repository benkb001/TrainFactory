namespace TrainGame.Callbacks; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public static class DrawInventoryCallback {
    public static DrawCallback Instantiate(World w, Inventory inv, Vector2 Position, float Width, 
        float Height, int Entity = -1, float Padding = 0f, bool SetMenu = true, bool DrawLabel = false) {
            return new DrawCallback(() => {
                Draw(w, inv, Position, Width, Height, Entity, Padding, SetMenu, DrawLabel); 
        }); 
    }

    public static void Create(World w, Inventory inv, Vector2 Position, float Width, float Height, 
        int Entity = -1, float Padding = 0f, bool SetMenu = true, bool DrawLabel = false) {
        
        int e = EntityFactory.Add(w); 
        w.SetComponent<DrawCallback>(e, Instantiate(w, inv, Position, Width, Height, 
            Entity: Entity, Padding, SetMenu, DrawLabel)); 
    }

    // returns inventoryEntity, can get the container from LLChild component
    public static InventoryView Draw(World w, Inventory inv, Vector2 Position, float Width, float Height, 
        int Entity = -1, float Padding = 0f, bool SetMenu = true, bool DrawLabel = false) {

        int containerEntity = w.EntityExists(Entity) ? Entity : EntityFactory.Add(w); 
        LinearLayout container = new LinearLayout("vertical", "alignlow"); 
        Frame f = new Frame(Position, Width, Height); 
        w.SetComponent<LinearLayout>(containerEntity, container); 
        w.SetComponent<Frame>(containerEntity, f); 

        float invHeight = Height; 
        float drawY = Position.Y; 
        int headerRowLLEntity = -1; 
        LinearLayout headerRowLL = null; 

        if (DrawLabel) {
            float labelWidth = Constants.LabelHeight * 2; 
            invHeight -= Constants.LabelHeight; 

            headerRowLLEntity = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(headerRowLLEntity, containerEntity, container, w); 

            headerRowLL = new LinearLayout("horizontal", "alignlow"); 
            headerRowLL.Padding = Constants.InventoryPadding; 
            w.SetComponent<LinearLayout>(headerRowLLEntity, headerRowLL); 
            w.SetComponent<Frame>(headerRowLLEntity, new Frame(0, 0, Width, Constants.LabelHeight)); 

            int labelEntity = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(labelEntity, headerRowLLEntity, headerRowLL, w);
            
            w.SetComponent<Frame>(labelEntity, new Frame(0, 0, labelWidth, Constants.LabelHeight));
            w.SetComponent<Outline>(labelEntity, new Outline()); 
            w.SetComponent<TextBox>(labelEntity, new TextBox(inv.GetId())); 
        }

        int inventoryEntity = EntityFactory.Add(w); 

        LinearLayoutWrap.AddChild(inventoryEntity, containerEntity, container, w); 

        w.SetComponent<Inventory>(inventoryEntity, inv); 
        
        LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
        ll.Padding = Padding; 
        w.SetComponent<LinearLayout>(inventoryEntity, ll);

        w.SetComponent<Frame>(inventoryEntity, new Frame(Position.X, drawY, 
            Width, invHeight, Padding)); 
        w.SetComponent<Outline>(inventoryEntity, new Outline(Depth: Constants.InventoryOutlineDepth)); 
        w.SetComponent<Background>(inventoryEntity, new Background(Colors.UIBG, Depth: Constants.InventoryBackgroundDepth)); 

        int rows = inv.GetRows(); 
        int cols = inv.GetCols(); 
        float rowWidth = Width - (Padding * 2); 
        float rowHeight = (invHeight - Padding * (rows + 1)) / rows; 

        float cellHeight = rowHeight - Padding * 2;
        float cellWidth = (rowWidth  - (Padding * (cols + 1))) / cols; 

        for (int i = 0; i < rows; i++) {
            int row = EntityFactory.Add(w); 
            
            w.SetComponent<Frame>(row, new Frame(0, 0, rowWidth, rowHeight)); 
            w.SetComponent<Outline>(row, new Outline(Depth: Constants.InventoryRowOutlineDepth)); 
            w.SetComponent<Background>(row, new Background(Colors.UIAccent, Constants.InventoryRowBackgroundDepth)); 

            LinearLayout rowLL = new LinearLayout("horizontal", "alignLow"); 
            rowLL.Padding = Padding; 
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
                LinearLayoutWrap.AddChild(cell, row, rowLL, w);
            }
            LinearLayoutWrap.AddChild(row, inventoryEntity, ll, w); 
        }

        return new InventoryView(containerEntity, inventoryEntity, headerRowLLEntity, 
            container, ll, headerRowLL, inv); 
    }
}
