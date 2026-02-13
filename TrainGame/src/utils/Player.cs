namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public static class PlayerWrap {
    public static int GetEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Data)])[0];
    }

    public static int GetRPGEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Frame), typeof(Health), typeof(Active)])[0];
    }

    public static void AddData(World w) {
        Inventory playerInv = new Inventory(Constants.PlayerInvID, 
                Constants.PlayerInvRows, Constants.PlayerInvCols);

        int playerInvDataEnt = EntityFactory.AddData<Inventory>(w, playerInv); 

        w.SetComponent<Inventory>(playerInvDataEnt, playerInv); 
        w.SetComponent<Player>(playerInvDataEnt, new Player()); 
        w.SetComponent<Health>(playerInvDataEnt, new Health(Constants.PlayerHP)); 
        w.SetComponent<Armor>(playerInvDataEnt, new Armor(0));
        w.SetComponent<Floor>(playerInvDataEnt, new Floor());

        (int armorInvEnt, Inventory armorInv) = InventoryWrap.Add(w, "Armor", 1, 1);
        armorInv.SetArmor();
        w.SetComponent<EquipmentSlot<Armor>>(playerInvDataEnt, new EquipmentSlot<Armor>(armorInv)); 

        playerInv.Add(ItemID.Gun, 1); 
    }

    public static void SetRespawn(World w, City c) {
        w.SetComponent<RespawnLocation>(GetEntity(w), new RespawnLocation(c));
    }

    public static int Draw(Vector2 position, World w) {
        int playerEntity = EntityFactory.Add(w); 
        int playerDataEnt = PlayerWrap.GetEntity(w); 

        Inventory playerInv = w.GetComponent<Inventory>(playerDataEnt); 

        (float playerInvWidth, float playerInvHeight) = InventoryWrap.GetUI(playerInv); 

        LinearLayoutContainer playerHUD = LinearLayoutWrap.Add(
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
        playerHUD.AddChild(hpEnt, w); 
        
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

        w.SetComponent<Parrier>(playerEntity, new Parrier());
        w.SetComponent<Armor>(playerEntity, w.GetComponent<Armor>(playerDataEnt)); 
        w.SetComponent<EquipmentSlot<Armor>>(playerEntity, w.GetComponent<EquipmentSlot<Armor>>(playerDataEnt)); 
        w.SetComponent<Damage>(playerEntity, new Damage(0)); 

        w.UnlockCameraPan(); 
        w.TrackEntity(playerEntity); 
        return playerEntity; 
    }

    public static void AddTest(World w) {
        AddData(w);
        SetRespawn(w, CityWrap.GetTest());
        Draw(new Vector2(-10, -10), w);
    }
}