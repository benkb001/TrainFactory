namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class HeldItem {
    private Inventory inv; 
    private Inventory.Item item; 
    public int InvIndex => invIndex; 
    public int InvSize => invSize; 
    public string ItemId => item.ItemId; 
    public int Count => item.Count; 
    public int ItemCount => item.Count; 
    public int LabelEntity; 
    public int InventoryEntity; 
    private int invIndex; 
    private int invSize; 

    public HeldItem(Inventory inv, int InventoryEntity) {
        this.inv = inv; 
        this.invIndex = 0; 
        this.item = inv.Get(invIndex); 
        this.invSize = inv.GetCols(); 
        this.LabelEntity = -1; 
        this.InventoryEntity = InventoryEntity;  
    }

    //NOTE: Mainly for testing
    public HeldItem(Inventory.Item item) {
        this.item = item; 
    }

    public void SetItem(int index) {
        if (index >= 0 && index < invSize) {
            invIndex = index; 
            this.item = inv.Get(invIndex); 
        }
    }

    public Inventory GetInventory() {
        return inv; 
    }
}