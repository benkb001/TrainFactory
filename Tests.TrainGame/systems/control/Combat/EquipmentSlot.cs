using TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks;

public class EquipSystemTest {
    [Fact]
    public void EquipSystem_ShouldSetEquippedItemToAllEntitiesWithThatEquipmentSlot() {

        World w = WorldFactory.Build(); 
        EquipmentID.InitMaps(); 

        Inventory inv = InventoryWrap.GetDefault(); 
        EquipmentSlot<Armor> slot = new EquipmentSlot<Armor>(inv);
        EquipmentUI eUI = new EquipmentUI(); 
        int slotEnt = EntityFactory.Add(w); 
        w.SetComponent<Inventory>(slotEnt, inv); 
        w.SetComponent<EquipmentSlot<Armor>>(slotEnt, slot); 
        w.SetComponent<EquipmentUI>(slotEnt, eUI); 

        int playerEnt = EntityFactory.Add(w); 
        w.SetComponent<EquipmentSlot<Armor>>(playerEnt, slot); 

        w.Update(); 

        inv.Add(ItemID.Armor2, 1); 

        w.Update(); 

        Assert.Equal(EquipmentSlot<Armor>.EquipmentMap[ItemID.Armor2], w.GetComponent<Armor>(playerEnt)); 
    }
}