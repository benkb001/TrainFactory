namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InventoryOrganizeMessage {
    public int CurRow; 
    public int CurColumn; 
    public int TargetRow; 
    public int TargetColumn; 
    public Inventory CurInv; 
    public Inventory TargetInv;
    public Draggable CurDraggable; 
    public Vector2 TargetVector; 

    public InventoryOrganizeMessage(
        int CurRow, 
        int CurColumn, 
        int TargetRow, 
        int TargetColumn,
        Inventory CurInv, 
        Inventory TargetInv,
        Draggable CurDraggable, 
        Vector2 TargetVector
    ) {
        this.TargetRow = TargetRow; 
        this.TargetColumn = TargetColumn;
        this.CurRow = CurRow; 
        this.CurColumn = CurColumn;
        this.CurDraggable = CurDraggable; 
        this.TargetVector = TargetVector; 
        this.CurInv = CurInv;
        this.TargetInv = TargetInv;
    }
}