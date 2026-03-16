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
    public Inventory CurInv; 
    public Inventory TargetInv;
    public Draggable CurDraggable; 
    public Vector2 TargetVector; 

    public InventoryOrganizeMessage(
        Inventory CurInv, 
        Inventory TargetInv,
        Inventory.Item CurItem, 
        Inventory.Item TargetItem, 
        Draggable CurDraggable, 
        Vector2 TargetVector
    ) {
        this.TargetItem = TargetItem; 
        this.CurItem = CurItem; 
        this.CurDraggable = CurDraggable; 
        this.TargetVector = TargetVector; 
        this.CurInv = CurInv;
        this.TargetInv = TargetInv;
    }
}