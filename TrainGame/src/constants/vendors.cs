namespace TrainGame.Constants;

using System.Collections.Generic;
using TrainGame.Components;

public class PurchaseItem : IBuyable { 
    public readonly string ItemID; 
    public readonly int Count; 
    public readonly Dictionary<string, int> Cost; 
    public Inventory Destination;

    public PurchaseItem(string ItemID, int Count, Dictionary<string, int> Cost) {
        this.ItemID = ItemID; 
        this.Count = Count; 
        this.Cost = Cost; 
    }

    public Dictionary<string, int> GetCost() {
        return Cost; 
    }
}

public class PurchaseLootMultiplier : IBuyable {
    public CombatRewardSpawner RewardSpawner;
}

public class PurchaseShieldHealAmount : IBuyable {
    public CombatRewardSpawner RewardSpawner;
}

public static class VendorID {
    public const string ArmorCraftsman = "Armor Craftsman"; 
    public const string WeaponCraftsman = "Weapon Craftsman"; 
    public const string MineralCollector = "Mineral Collector"; 

    public static List<string> All = new() {
        ArmorCraftsman, WeaponCraftsman, MineralCollector
    };

    public static Dictionary<string, List<IBuyable>> ProductMap = new() {
        [ArmorCraftsman] = new () {
            new PurchaseItem(ItemID.Armor1, 1, new() {
                [ItemID.Credit] = 50, 
                [ItemID.Iron] = 50,
                [ItemID.Cobalt] = 50
            }),
            new PurchaseItem(ItemID.Armor2, 1, new () {
                [ItemID.Credit] = 1000, 
                [ItemID.Iron] = 200, 
                [ItemID.Cobalt] = 200,
                [ItemID.Glass] = 100,
            }),
            new PurchaseItem(ItemID.Armor3, 1, new () {
                [ItemID.Credit] = 3000,
                [ItemID.Iron] = 2000, 
                [ItemID.Oil] = 1000,
                [ItemID.Mythril] = 250
            }),
        },
        [WeaponCraftsman] = new () {
            new PurchaseItem(ItemID.Shotgun, 1, new () {
                [ItemID.Credit] = 1000, 
                [ItemID.Water] = 1000,
                [ItemID.Iron] = 500, 
                [ItemID.Cobalt] = 500,
                [ItemID.Fuel] = 500
            }),
            new PurchaseItem(ItemID.Ring, 1, new () {
                [ItemID.Credit] = 2000, 
                [ItemID.Iron] = 2000,
                [ItemID.Mythril] = 500,
                [ItemID.Petroleum] = 500,
                [ItemID.Lubricant] = 100
            }),
            new PurchaseUpgradeGunDamage(ItemID.Pistol),
            new PurchaseLootMultiplier(),
            new PurchaseShieldHealAmount()
        },
        [MineralCollector] = new() {
            new PurchaseItem(ItemID.Iron, 100, new() {
                [ItemID.Credit] = 100
            }),
            new PurchaseItem(ItemID.Sand, 100, new() {
                [ItemID.Credit] = 100
            }),
            new PurchaseItem(ItemID.Water, 100, new() {
                [ItemID.Credit] = 100
            })
        }
    };

    public static List<Dictionary<string, int>> BaseGunDamageUpgradeCosts = new() {
        new() {
            [ItemID.Credit] = 1000,
            [ItemID.Cobalt] = 1000,
            [ItemID.Fuel] = 500
        },
        new() {
            [ItemID.Credit] = 5000,
            [ItemID.Cobalt] = 2500,
            [ItemID.Mythril] = 500,
            [ItemID.Petroleum] = 250,
            [ItemID.Lubricant] = 250
        },
        new(){
            [ItemID.Credit] = 15000,
            [ItemID.Cobalt] = 5000,
            [ItemID.Mythril] = 3000,
            [ItemID.Adamantite] = 2500,
            [ItemID.Duplicator] = 10,
            [ItemID.PocketDimension] = 10,
            [ItemID.AirResistor] = 10
        }
    };

    //TODO: Different guns can have different upgrade costs to reflect when you unlock them
    public static Dictionary<string, int> UpgradeGunDamageCost(string gunID, int level) {
        return BaseGunDamageUpgradeCosts[level - 1];
    }

    public static List<Dictionary<string, int>> UpgradeLootMultiplierCost = new() {
        new() {
            [ItemID.Credit] = 5000,
            [ItemID.Fuel] = 3000,
            [ItemID.Cobalt] = 1000
        },
        new() {
            [ItemID.Duplicator] = 100,
            [ItemID.Adamantite] = 5000,
            [ItemID.Mythril] = 7500,
            [ItemID.Cobalt] = 10000,
            [ItemID.Credit] = 10000
        }
    };

    public static List<Dictionary<string, int>> UpgradeShieldHealAmountCost = new(){
        new(){
            [ItemID.Credit] = 2000,
            [ItemID.Iron] = 1000,
            [ItemID.Cobalt] = 500
        },
        new(){
            [ItemID.Credit] = 4000,
            [ItemID.Mythril] = 2000,
            [ItemID.Oil] = 1000
        },
        new(){
            [ItemID.Credit] = 6000,
            [ItemID.Mythril] = 3000,
            [ItemID.Cobalt] = 3000,
            [ItemID.Lubricant] = 1000
        },
        new(){
            [ItemID.Credit] = 10000,
            [ItemID.Adamantite] = 4000,
            [ItemID.Duplicator] = 100
        }
    };
}