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
public class TestEquip : IEquippable {}
public class EquipmentSlotTest {
    [Fact]
    public void EquipmentSlot_EquipShouldReturnTheAssociatedComponentInEquipmentMapAfterEquipping() {
        Inventory inv = new Inventory("Test", 1, 1);
        inv.Add("Test", 1);
        TestEquip test = new();
        EquipmentSlot<TestEquip>.EquipmentMap = new() {
            ["Test"] = test
        };
        EquipmentSlot<TestEquip> slot = new(inv); 
        slot.Equip();
        Assert.Equal(test, slot.GetEquipment());
    }
}