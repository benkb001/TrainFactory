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

public static class EquipSystem {
    public static void Register<T>(World w) where T : IEquippable {
        Type[] ts = { typeof(EquipmentSlot<T>), typeof(EquipmentData), typeof(Data), typeof(InventoryUpdatedFlag) };
        
        w.AddSystem(ts, (w, e) => {
            EquipmentSlot<T> slot = w.GetComponent<EquipmentSlot<T>>(e); 
            slot.Equip();
            T equip = slot.GetEquipment();
            w.GetMatchingEntities([typeof(EquipmentSlot<T>)])
            .Where(ent => w.GetComponent<EquipmentSlot<T>>(ent).Equals(slot))
            .ToList()
            .ForEach(ent => EquipmentRegistry.Add(w, equip, ent));
        });
    }
}

public static class EquipPlayerGunSystem {
    public static void Register() {
        EquipmentRegistry.Register<PlayerGun>((w, playerGun, e) => {
            (IShootPattern spPrev, bool hadSP) = w.GetComponentSafe<IShootPattern>(e); 

            if (hadSP) {
                ShootPatternRegistry.Remove(w, spPrev, e); 
            }
            
            w.SetComponent<Shooter>(e, playerGun.GetShooter().Clone()); 
            IShootPattern sp = playerGun.GetShootPattern().Clone();
            w.SetComponent<IShootPattern>(e, sp); 
            ShootPatternRegistry.Add(w, sp, e); 
        }); 
    }
}

public static class EquipArmorSystem {
    public static void Register() {
        EquipmentRegistry.Register<Armor>((w, armor, e) => {
            w.SetComponent<Armor>(e, armor); 
        });
    }
}

public static class RegisterEquipmentCallbacks {
    public static void All() {
        EquipArmorSystem.Register();
        EquipPlayerGunSystem.Register(); 
    }
}

public static class EquipmentRegistry {
    private static CallbackRegistry<World, IEquippable, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IEquippable {
        registry.Register<T>(callback); 
    }

    public static void Add(World w, IEquippable equip, int e) {
        registry.Callback(w, equip, e);
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
            (EquipmentSlot<PlayerGun> gunSlot, bool hasGunSlot) = w.GetComponentSafe<EquipmentSlot<PlayerGun>>(playerDataEnt); 
            if (hasGunSlot) {
                Inventory gunInv = gunSlot.GetInventory(); 
                LinearLayoutContainer outer = LinearLayoutContainer.AddOuter(w); 

                (float invWidth, float invHeight) = InventoryWrap.GetUI(playerInv);
                InventoryView playerInvView = DrawInventoryCallback.Draw(
                    w, 
                    playerInv,
                    Vector2.Zero, 
                    invWidth, 
                    invHeight
                ); 

                (float armorWidth, float armorHeight) = InventoryWrap.GetUI(gunInv); 
                InventoryView gunInvView = DrawInventoryCallback.Draw(
                    w, 
                    gunInv, 
                    Vector2.Zero,
                    armorWidth, 
                    armorHeight,
                    DrawLabel: true
                );

                int gunInvEnt = gunInvView.GetInventoryEntity();
                w.SetComponent<EquipmentSlot<PlayerGun>>(gunInvEnt, gunSlot); 
                w.SetComponent<EquipmentData>(gunInvEnt, new EquipmentData()); 

                outer.AddChild(gunInvView.GetParentEntity(), w); 
                outer.AddChild(playerInvView.GetParentEntity(), w); 
            }
        });
    }
}