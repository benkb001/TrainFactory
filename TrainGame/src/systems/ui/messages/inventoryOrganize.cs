namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InventoryOrganizeMessage {
    public int TargetRow; 
    public int TargetColumn; 
    public int CurRow; 
    public int CurColumn; 
    public Inventory.Item TargetItem; 
    public Inventory.Item CurItem; 
    public Draggable CurDraggable; 
    public Vector2 TargetVector; 

    public InventoryOrganizeMessage(
        int TargetRow,
        int TargetColumn,
        int CurRow,
        int CurColumn,
        Inventory.Item TargetItem, 
        Inventory.Item CurItem, 
        Draggable CurDraggable, 
        Vector2 TargetVector
    ) {
        this.TargetRow = TargetRow; 
        this.TargetColumn = TargetColumn; 
        this.CurRow = CurRow; 
        this.CurColumn = CurColumn; 
        this.TargetItem = TargetItem; 
        this.CurItem = CurItem; 
        this.CurDraggable = CurDraggable; 
        this.TargetVector = TargetVector; 
    }
}