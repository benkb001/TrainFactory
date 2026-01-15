namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Systems;
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class EquipmentSlot<T> {
    private string itemID; 
    private Inventory inv;
    private T equipment; 

    public string ItemID => itemID; 
    public Inventory GetInventory() => inv; 
    public T GetEquipment() => equipment; 
    
    public static Dictionary<string, T> EquipmentMap;

    public EquipmentSlot(Inventory inv, string itemID = "") {
        this.inv = inv; 
        this.itemID = itemID; 
    }

    public void SetEquipped(string itemID) {
        this.itemID = itemID; 
        equipment = EquipmentMap[itemID]; 
    }
}