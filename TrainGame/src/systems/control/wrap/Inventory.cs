namespace TrainGame.Systems;

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS;
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Constants;

public static class InventoryWrap {
    public static (int, Inventory) Add(World w, string id, int rows, int cols, int level = 1, CartType filter = CartType.General) {
        ID.Use(id);
        Inventory inv = new Inventory(id, rows, cols, level, filter);
        int e = EntityFactory.AddData<Inventory>(w, inv); 
        return (e, inv); 
    }

    public static void Add(World w, Inventory inv) {
        EntityFactory.AddData<Inventory>(w, inv); 
    }

    public static Inventory GetyByEntityOrId(World w, int entity = -1, string inventoryId = "") {
        if (w.ComponentContainsEntity<Inventory>(entity)) {
            return w.GetComponent<Inventory>(entity); 
        }
        return GetByID(w, inventoryId); 
    }

    public static Inventory GetDefault() {
        return new Inventory("Test", 2, 2); 
    }

    public static Inventory GetPlayerInv(World w) {
        return GetByID(w, Constants.PlayerInvID); 
    }

    public static Inventory GetByID(World w, string inventoryID) {
        return w.GetComponentArray<Inventory>().Where(pair => pair.Value.GetId() == inventoryID).FirstOrDefault().Value; 
    }

    public static int GetEntity(string inventoryId, World w) {
        return w.GetMatchingEntities([typeof(Data), typeof(Inventory)]).Where(
            e => w.GetComponent<Inventory>(e).Id == inventoryId).FirstOrDefault(); 
    }

    public static (float, float) GetUI(Inventory inv, float scale = 1f) {
        float width = inv.GetCols() * (Constants.InventoryCellSize + Constants.InventoryPadding) + 
            Constants.InventoryPadding; 
        float height = inv.GetRows() * (Constants.InventoryCellSize + Constants.InventoryPadding) +
            Constants.InventoryPadding + Constants.LabelHeight;
        return (width * scale, height * scale);
    }

    public static int ItemCount(List<Inventory> invs, string itemID) {
        return invs.Aggregate(0, (acc, inv) => acc + inv.ItemCount(itemID));
    }
}