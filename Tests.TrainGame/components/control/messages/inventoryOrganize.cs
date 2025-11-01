using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class InventoryOrganizeMessageTest {
    [Fact] 
    public void InventoryOrganizeMessage_ShouldRespectConstructors() {
        Inventory.Item target = new Inventory.Item(ItemId: "Apple"); 
        Inventory.Item cur = new Inventory.Item(ItemId: "Orange"); 
        Draggable d = new Draggable();
        Vector2 targetVector = new Vector2(10, 30); 
        InventoryOrganizeMessage organize = new InventoryOrganizeMessage(
            3, //targetRow
            4, //targetCol
            2, //curRow
            5, //curCol
            target, //targetItem
            cur, //curItem
            d, //curDraggable
            targetVector //targetVector
        ); 
        Assert.Equal(3, organize.TargetRow); 
        Assert.Equal(4, organize.TargetColumn); 
        Assert.Equal(2, organize.CurRow); 
        Assert.Equal(5, organize.CurColumn); 
        Assert.Equal(target, organize.TargetItem); 
        Assert.Equal(cur, organize.CurItem); 
        Assert.Equal(d, organize.CurDraggable); 
        Assert.Equal(targetVector, organize.TargetVector); 
    }
}