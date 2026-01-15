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
        w.AddSystem([typeof(Inventory), typeof(EquipmentSlot<T>), typeof(EquipmentUI), typeof(Active)], (w, e) => {
            Inventory inv = w.GetComponent<Inventory>(e); 
            EquipmentSlot<T> slot = w.GetComponent<EquipmentSlot<T>>(e); 
            string itemID = inv.GetItemId(0); 

            if (itemID != "" && itemID != slot.ItemID) {
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

            LinearLayoutContainer outer = LinearLayoutWrap.AddOuter(w); 

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