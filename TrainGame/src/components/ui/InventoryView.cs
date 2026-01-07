namespace TrainGame.Components; 

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

public class InventoryView {
    int parentEntity; 
    int mainEntity; 
    int headerEntity; 
    LinearLayout parentLL; 
    LinearLayout mainLL; 
    LinearLayout headerRowLL; 
    Inventory inv; 
    public int GetInventoryEntity() => mainEntity; 
    public Inventory GetInventory() => inv; 

    public InventoryView(int parentEntity, int mainEntity, int headerEntity, 
        LinearLayout parentLL, LinearLayout mainLL, LinearLayout headerRowLL, 
        Inventory inv) {
        
        this.parentEntity = parentEntity; 
        this.headerEntity = headerEntity;
        this.mainEntity = mainEntity; 
        this.parentLL = parentLL; 
        this.mainLL = mainLL; 
        this.headerRowLL = headerRowLL; 
        this.inv = inv; 
    }

    public void AddChildToHeader(int cEnt, World w) {
        LinearLayoutWrap.AddChild(cEnt, headerEntity, headerRowLL, w);
    }

    public void Clear(World w) {
        LinearLayoutWrap.Clear(parentEntity, w, parentLL);
    }

    public Vector2 GetPosition(World w) {
        return w.GetComponent<Frame>(parentEntity).Position;
    }

    public int GetParentEntity() {
        return parentEntity; 
    }

    public int GetCellEntity(int row, int col, World w) {
        inv.EnsureValidIndices(row, col); 
        int rowEntity = mainLL.PagedChildren[row]; 
        return w.GetComponent<LinearLayout>(rowEntity).GetChildren()[col]; 
    }
}