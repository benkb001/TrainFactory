namespace TrainGame.Constants;

using System.Collections.Generic;
using TrainGame.Components;

public class PurchaseItem : IBuyable { 
    public readonly string ItemID; 
    public readonly int Count; 
    public readonly Dictionary<string, int> Cost; 

    public PurchaseItem(string ItemID, int Count, Dictionary<string, int> Cost) {
        this.ItemID = ItemID; 
        this.Count = Count; 
        this.Cost = Cost; 
    }

    public Dictionary<string, int> GetCost() {
        return Cost; 
    }
}

public class ResetHP : IBuyable {
    private static Dictionary<string, int> cost = new() {
        [ItemID.Credit] = 1000, //TODO: make this dynamic ? 
    };

    public readonly int Credits;
    public readonly Inventory Dest; 

    public ResetHP(int Credits = 0, Inventory Dest = null) {
        this.Credits = Credits; 
        this.Dest = Dest; 
    }

    public Dictionary<string, int> GetCost() {
        return cost;
    }
}

public class PurchaseInfo {
    public readonly IBuyable Buyable;

    private PurchaseInfo(IBuyable Buyable) {
        this.Buyable = Buyable;
    }

    public static PurchaseInfo AddItemInfo(string ItemID, int Count, Dictionary<string, int> Cost) {
        return new PurchaseInfo(new PurchaseItem(ItemID, Count, Cost)); 
    }

    public static PurchaseInfo AddResetHP() {
        return new PurchaseInfo(new ResetHP());
    }
}

public static class VendorID {
    public const string ArmorCraftsman = "Armor Craftsman"; 
    public const string WeaponCraftsman = "Weapon Craftsman"; 
    public const string MineralCollector = "Mineral Collector"; 

    public static List<string> All = new() {
        ArmorCraftsman, WeaponCraftsman, MineralCollector
    };

    public static Dictionary<string, List<PurchaseInfo>> ProductMap = new() {
        [ArmorCraftsman] = new () {
            PurchaseInfo.AddItemInfo(ItemID.Armor1, 1, new() {
                [ItemID.Credit] = 50, 
                [ItemID.Iron] = 50,
                [ItemID.Cobalt] = 50
            }),
            PurchaseInfo.AddItemInfo(ItemID.Armor2, 1, new () {
                [ItemID.Credit] = 1000, 
                [ItemID.Iron] = 200, 
                [ItemID.Cobalt] = 200,
                [ItemID.Glass] = 100,
            }),
            PurchaseInfo.AddItemInfo(ItemID.Armor3, 1, new () {
                [ItemID.Credit] = 3000,
                [ItemID.Iron] = 2000, 
                [ItemID.Oil] = 1000,
                [ItemID.Mythril] = 250
            }),
        },
        [WeaponCraftsman] = new () {
            PurchaseInfo.AddItemInfo(ItemID.Shotgun, 1, new () {
                [ItemID.Credit] = 1000, 
                [ItemID.Water] = 1000,
                [ItemID.Iron] = 500, 
                [ItemID.Cobalt] = 500,
                [ItemID.Fuel] = 500
            }),
            PurchaseInfo.AddItemInfo(ItemID.Ring, 1, new () {
                [ItemID.Credit] = 2000, 
                [ItemID.Iron] = 2000,
                [ItemID.Mythril] = 500,
                [ItemID.Petroleum] = 500,
                [ItemID.Lubricant] = 100
            }),
        },
        [MineralCollector] = new() {
            PurchaseInfo.AddItemInfo(ItemID.Iron, 100, new() {
                [ItemID.Credit] = 100
            }),
            PurchaseInfo.AddItemInfo(ItemID.Sand, 100, new() {
                [ItemID.Credit] = 100
            }),
            PurchaseInfo.AddItemInfo(ItemID.Water, 100, new() {
                [ItemID.Credit] = 100
            })
        }
    };
}