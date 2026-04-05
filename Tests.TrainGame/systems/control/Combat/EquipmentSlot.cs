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
    public void EquipSystem_EquipGunShouldSetShooterToAllEntitiesWithThatEquipmentSlot() {
        World w = WorldFactory.Build();

        Inventory inv = InventoryWrap.GetDefault(); 
        inv.Add("Test", 1); 
        EquipmentSlot<PlayerGun> slot = new EquipmentSlot<PlayerGun>(inv);
        int slotEnt = EntityFactory.Add(w); 
        w.SetComponent<EquipmentSlot<PlayerGun>>(slotEnt, slot); 
        w.SetComponent<EquipmentData>(slotEnt, new EquipmentData()); 
        w.SetComponent<InventoryUpdatedFlag>(slotEnt, new InventoryUpdatedFlag());
        w.SetComponent<Data>(slotEnt, new Data());

        int playerEnt = EntityFactory.Add(w); 
        w.SetComponent<EquipmentSlot<PlayerGun>>(playerEnt, slot); 

        w.Update(); 

        Assert.Equal(w.GetComponent<Shooter>(slotEnt), w.GetComponent<Shooter>(playerEnt)); 
    }
}