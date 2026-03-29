namespace TrainGame.Systems; 

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

public class EquipmentUI {}

public static class EquipSystem {
    public static void Register<T>(World w) {
        Type[] ts = {
            typeof(Inventory), typeof(EquipmentSlot<T>), typeof(InventoryUpdatedFlag), 
            typeof(EquipmentData), typeof(Data)
        };
        
        w.AddSystem(ts, (w, e) => {
            
            Inventory inv = w.GetComponent<Inventory>(e); 
            EquipmentSlot<T> slot = w.GetComponent<EquipmentSlot<T>>(e); 
            Inventory.Item item = inv.Get(0); 
            string itemID = item.ID; 
            int itemCount = item.Count; 

            if (itemID != "" && itemID != slot.ItemID && itemCount > 0) {
                slot.SetEquipped(itemID); 
                T equip = slot.GetEquipment();
                w.GetMatchingEntities([typeof(EquipmentSlot<T>)])
                .Where(ent => w.GetComponent<EquipmentSlot<T>>(ent).Equals(slot))
                .ToList()
                .ForEach(ent => w.SetComponent<T>(ent, equip));
            }
        });
    }
}

public static class ToolSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Player), typeof(Active), typeof(HeldItem)], (w, e) => {
            HeldItem h = w.GetComponent<HeldItem>(e);
            
            if (Weapons.PlayerGunMap.ContainsKey(h.ID)) {

                PlayerGun pg = Weapons.PlayerGunMap[h.ID];
                Shooter shooter = pg.GetShooter(); 
                IShootPattern sp = pg.GetShootPattern();

                if (w.EntityExists(h.LabelEntity)) {
                    w.SetComponent<Shooter>(e, shooter);
                    ShootPatternRegistry.Add(w, sp, e);
                }
            } 
        });
    }
}

public class EquipmentInterfaceData : IInterfaceData {

    public SceneType GetSceneType() => SceneType.EquipmentInterface; 
    public Menu GetMenu() => new Menu(); 
}

public static class DrawEquipmentInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<EquipmentInterfaceData>(w, (w, e) => {
            int playerDataEnt = PlayerWrap.GetEntity(w); 
            Inventory playerInv = w.GetComponent<Inventory>(playerDataEnt); 
            EquipmentSlot<Armor> armorSlot = w.GetComponent<EquipmentSlot<Armor>>(playerDataEnt); 
            Inventory armorInv = armorSlot.GetInventory(); 

            LinearLayoutContainer outer = LinearLayoutContainer.AddOuter(w); 

            (float invWidth, float invHeight) = InventoryWrap.GetUI(playerInv);
            InventoryView playerInvView = DrawInventoryCallback.Draw(
                w, 
                playerInv,
                Vector2.Zero, 
                invWidth, 
                invHeight
            ); 

            (float armorWidth, float armorHeight) = InventoryWrap.GetUI(armorInv); 
            InventoryView armorInvView = DrawInventoryCallback.Draw(
                w, 
                armorInv, 
                Vector2.Zero,
                armorWidth, 
                armorHeight,
                DrawLabel: true
            );

            int armorInvEnt = armorInvView.GetInventoryEntity();
            w.SetComponent<EquipmentSlot<Armor>>(armorInvEnt, armorSlot); 
            w.SetComponent<EquipmentUI>(armorInvEnt, new EquipmentUI());

            outer.AddChild(armorInvView.GetParentEntity(), w); 
            outer.AddChild(playerInvView.GetParentEntity(), w); 
        });
    }
}