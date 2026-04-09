namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public static class PlayerWrap {
    private static EquipmentSlot<T> addEquipSlot<T>(World w, int dataEnt) where T : IEquippable {
        (int invEnt, Inventory equipInv) = InventoryWrap.Add(w, Constants.EquipmentInvID<T>(), 1, 1);
        EquipmentSlot<T> equip = EquipmentSlotWrap.Add<T>(w, equipInv, invEnt);
        w.SetComponent<EquipmentSlot<T>>(dataEnt, equip); 
        return equip; 
    }

    public static int GetEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Health), typeof(Parrier), typeof(Data)])[0];
    }

    public static int GetRPGEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Frame), typeof(Health), typeof(Active)])[0];
    }

    //This code runs when we build world form scratch
    //We need to handle equip slots here to build them for 
    //the first time
    public static void AddData(World w) {
        Inventory playerInv = new Inventory(Constants.PlayerInvID, 
                Constants.PlayerInvRows, Constants.PlayerInvCols);

        Health h = new Health(Constants.PlayerHP);
        Parrier p = new Parrier(Constants.PlayerParrierHP);
        int e = AddData(w, playerInv, h, p);
        EquipmentSlot<PlayerGun> slot = addEquipSlot<PlayerGun>(w, e); 
        slot.GetInventory().Add(ItemID.Pistol, 1);
        w.SetComponent<CombatRewardSpawner>(e, new CombatRewardSpawner());
    }
    
    //This code runs when we load from persistent state
    //which handles adding the equipment slots. 
    public static int AddData(World w, Inventory inv, Health h, Parrier p) {
        int playerDataEnt = EntityFactory.AddData<Inventory>(w, inv); 
        w.SetComponent<Player>(playerDataEnt, new Player()); 
        w.SetComponent<Health>(playerDataEnt, h); 
        w.SetComponent<Parrier>(playerDataEnt, p);

        Armor armor = new Armor(0); 
        w.SetComponent<Armor>(playerDataEnt, armor);
        return playerDataEnt;
    }

    public static void SetRespawn(World w, City c) {
        w.SetComponent<RespawnLocation>(GetEntity(w), new RespawnLocation(c));
    }

    public static int GetHeldItemEnt(World w) {
        List<int> es = w.GetMatchingEntities([typeof(HeldItem), typeof(Active)]);
        
        if (es.Count > 0) {
            return es[0]; 
        }

        return -1; 
    }

    public static Inventory GetInventory(World w) {
        return w.GetComponent<Inventory>(GetEntity(w)); 
    }

    public static Inventory GetEquipmentInventory<T>(World w) where T : IEquippable {
        int playerEnt = PlayerWrap.GetEntity(w); 
        return w.GetComponent<EquipmentSlot<T>>(playerEnt).GetInventory();
    }

    public static Health GetHP(World w) {
        return w.GetComponent<Health>(GetEntity(w));
    }

    public static Parrier GetParrier(World w) {
        return w.GetComponent<Parrier>(GetEntity(w));
    }

    public static CombatRewardSpawner GetCombatRewardSpawner(World w) {
        return w.GetComponent<CombatRewardSpawner>(GetEntity(w));
    }

    public static string GetHeldItemID(World w) {
        int e = GetHeldItemEnt(w); 
        (HeldItem h, bool s) = w.GetComponentSafe<HeldItem>(e); 

        if (s) {
            return h.ItemID; 
        }
        
        return "";
    }

    public static Shooter GetShooter(World w) {
        (Shooter s, bool has) = w.GetComponentSafe<Shooter>(GetEntity(w));
        return has ? s : null; 
    }

    public static int Draw(Vector2 position, World w) {
        int playerEntity = EntityFactory.Add(w); 
        int playerDataEnt = PlayerWrap.GetEntity(w); 

        Inventory playerInv = w.GetComponent<Inventory>(playerDataEnt); 

        (float playerInvWidth, float playerInvHeight) = InventoryWrap.GetUI(playerInv); 

        LinearLayoutContainer playerHUD = LinearLayoutContainer.Add(
            w, 
            Vector2.Zero, 
            playerInvWidth + 100f, 
            playerInvHeight,
            outline: false, 
            screenAnchor: true
        );
        w.SetComponent<PlayerHUD>(playerHUD.GetParentEntity(), new PlayerHUD());

        InventoryView playerInvView = DrawInventoryCallback.Draw(w, playerInv, Vector2.Zero, playerInvWidth, 
            playerInvHeight, Padding: Constants.InventoryPadding, SetMenu: false, DrawLabel: false);
        
        playerHUD.AddChild(playerInvView.GetParentEntity(), w); 

        int hpEnt = EntityFactory.AddUI(w, Vector2.Zero, 80, 80, setOutline: true, text: "HP"); 
        w.SetComponent<Health>(hpEnt, w.GetComponent<Health>(playerDataEnt)); 
        //TODO: we should store the parrier hp in the data ent. 
        Parrier parrier = w.GetComponent<Parrier>(playerDataEnt);
        w.SetComponent<Parrier>(hpEnt, parrier);
        playerHUD.AddChild(hpEnt, w); 

        int ammoEnt = EntityFactory.AddUI(w, Vector2.Zero, 80, 80, setOutline: true, text: "Ammo"); 
        w.SetComponent<AmmoHUD>(ammoEnt, new AmmoHUD());
        playerHUD.AddChild(ammoEnt, w); 
        
        int playerInvEnt = playerInvView.GetInventoryEntity(); 
        w.SetComponent<Frame>(playerEntity, new Frame(position, Constants.PlayerWidth, Constants.PlayerHeight)); 
        w.SetComponent<Interactor>(playerEntity, Interactor.Get());
        w.SetComponent<CardinalMovement>(playerEntity, new CardinalMovement(Constants.PlayerSpeed)); 
        w.SetComponent<Collidable>(playerEntity, Collidable.Get()); 
        w.SetComponent<HeldItem>(playerEntity, new HeldItem(playerInv, playerInvEnt)); 
        w.SetComponent<Outline>(playerEntity, 
            new Outline(Colors.PlayerOutline, Constants.PlayerOutlineThickness, Depth.PlayerOutline)); 
        w.SetComponent<Background>(playerEntity, new Background(Colors.PlayerBackground, Depth.PlayerBackground));
        w.SetComponent<Player>(playerEntity, new Player()); 
        w.SetComponent<Health>(playerEntity, w.GetComponent<Health>(playerDataEnt));
        w.SetComponent<RespawnLocation>(playerEntity, w.GetComponent<RespawnLocation>(playerDataEnt));
        w.SetComponent<Inventory>(playerEntity, playerInv); 

        w.SetComponent<Parrier>(playerEntity, parrier);
        w.SetComponent<EquipmentSlot<PlayerGun>>(playerEntity, w.GetComponent<EquipmentSlot<PlayerGun>>(playerDataEnt)); 
        (Shooter shooter, bool hasShooter) = w.GetComponentSafe<Shooter>(playerDataEnt); 
        (IShootPattern sp, bool hasSP) = w.GetComponentSafe<IShootPattern>(playerDataEnt); 

        if (hasShooter) {
            w.SetComponent<Shooter>(playerEntity, shooter); 
        }

        if (hasSP) {
            ShootPatternRegistry.Add(w, sp, playerEntity); 
        }

        w.SetComponent<Targetable>(playerEntity, new Targetable());

        w.UnlockCameraPan(); 
        w.TrackEntity(playerEntity); 
        return playerEntity; 
    }

    public static void ResetStats(World w) {
        int e = PlayerWrap.GetEntity(w); 
        Health h = w.GetComponent<Health>(e); 
        h.ResetHP(); 
        Armor armor = w.GetComponent<Armor>(e); 
        armor.ResetTempDefense(); 
        w.GetComponent<Parrier>(e).Reset();
        EquipmentRegistry.Add(w, w.GetComponent<EquipmentSlot<PlayerGun>>(e).GetEquipment(), e);
    }

    public static int AddTest(World w) {
        AddData(w);
        SetRespawn(w, CityWrap.GetTest());
        return Draw(Vector2.Zero, w);
    }
}