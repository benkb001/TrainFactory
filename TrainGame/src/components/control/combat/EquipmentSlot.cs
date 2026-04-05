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

/*
In order to have a new equipmentslot we have to: 
1. Register a callback to EquipmentRegistry
2. include the <T> in equipment slot generic load and save
3. include the map in EquipmentID.InitMaps()
4. Add EquipSystem.Register<T> in world
*/
public class EquipmentSlot<T> where T : IEquippable {
    private string itemID; 
    private Inventory inv;
    private T equipment; 

    public string ItemID => itemID; 
    public Inventory GetInventory() => inv; 
    public T GetEquipment() => equipment; 
    
    public static Dictionary<string, T> EquipmentMap;

    public EquipmentSlot(Inventory inv, string itemID = "") {
        inv.Whitelist(EquipmentMap.Keys);
        this.inv = inv; 
        this.itemID = itemID; 
        if (EquipmentMap.ContainsKey(itemID)) {
            this.equipment = EquipmentMap[itemID];
        } else {
            this.equipment = default(T);
        }
        
    }

    public void Equip() {
        itemID = inv.Get(0).ID; 
        if (EquipmentMap.ContainsKey(itemID)) {
            equipment = EquipmentMap[itemID]; 
        }
    }
}

public class EquipmentSlotWrap {
    public static EquipmentSlot<T> Add<T>(World w, Inventory inv, int e) where T : IEquippable {
        EquipmentSlot<T> equip = new EquipmentSlot<T>(inv);
        w.SetComponent<EquipmentData>(e, new EquipmentData()); 
        w.SetComponent<EquipmentSlot<T>>(e, equip); 
        w.SetComponent<InventoryUpdatedFlag>(e, InventoryUpdatedFlag.Get());
        return equip;
    }
}

public class EquipmentData {}