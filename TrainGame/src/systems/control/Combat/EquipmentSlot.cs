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
        Type[] ts = { typeof(EquipmentSlot<T>), typeof(EquipmentData), typeof(InventoryUpdatedFlag) };
        
        w.AddSystem(ts, (w, e) => {
            EquipmentSlot<T> slot = w.GetComponent<EquipmentSlot<T>>(e); 
            slot.Equip();
            T equip = slot.GetEquipment();
            EquipmentRegistry.Add(w, equip, e);
        });
    }
}

public static class EquipPlayerGunSystem {
    public static void Register() {
        EquipmentRegistry.Register<PlayerGun>((w, playerGun, _) => {
            Shooter shooter = playerGun.InstantiateShooter(); 
            IShootPattern sp = playerGun.InstantiateShootPattern(); 

            foreach (int e in w.GetMatchingEntities([typeof(EquipmentSlot<PlayerGun>)])) {
                w.SetComponent<Shooter>(e, shooter);
                ShootPatternRegistry.Add(w, sp, e); 
            }
        }); 
    }
}

public static class RegisterEquipmentCallbacks {
    public static void All() {
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
            Inventory playerInv = InventoryWrap.GetByID(w, Constants.WeaponsInvID);
            Inventory gunInv = InventoryWrap.GetByID(w, Constants.EquipmentInvID<PlayerGun>());

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

            outer.AddChild(gunInvView.GetParentEntity(), w); 
            outer.AddChild(playerInvView.GetParentEntity(), w); 
        });
    }
}