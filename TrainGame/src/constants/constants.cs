namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using TrainGame.Components;

public static class Constants {
    public static readonly int MaxComponents = 1024; 

    public static readonly float InventoryBackgroundDepth = 0.8f; 
    public static readonly float InventoryOutlineDepth = 0.75f; 
    public static readonly float InventoryRowBackgroundDepth = 0.7f; 
    public static readonly float InventoryRowOutlineDepth = 0.65f; 
    public static readonly float InventoryCellBackgroundDepth = 0.6f; 
    public static readonly float InventoryCellOutlineDepth = 0.55f; 
    public static readonly float InventoryCellTextBoxDepth = 0.55f; 
    public static readonly float InventoryHeldBackgroundDepth = 0.5f; 
    public static readonly float InventoryHeldOutlineDepth = 0.45f; 
    public static readonly float InventoryHeldTextBoxDepth = 0.45f; 

    public const float TileWidth = 50f;

    public const float PlayerWidth = 45f; 
    public const float PlayerHeight = TileWidth; 
    public const float PlayerSpeed = 5f; 
    public const float ParryingSpeed = 2f;
    public const int PlayerOutlineThickness = 1; 
    public const int PlayerHP = 3;

    public const int MaxFloorLevel1 = 20; 
    public const int MaxFloorLevel2 = 40;

    public const int ButtonOutlineThickness = 1; 
    public const int ButtonHeldOutlineThickness = 2; 
    public const int ButtonHoveredOutlineThickness = 3; 

    public static readonly float EmbarkLayoutWidth = 200f; 
    public static readonly float EmbarkLayoutHeight = 500f;
    public static readonly float EmbarkLayoutPadding = 5f;  

    public const float InventoryCellSize = 60f; 
    public const float InventoryPadding = 5f;

    public static float LabelHeight = 25f; 

    public static float InvUpgradeMass = 1000f; 

    public const float TrainDefaultPower = 1250f; 
    public const float PowerPerEngine = 100f;
    public const float UpgradePowerStep = 100f; 
    public const float TrainDefaultMass = 1000f; 
    public const float MassMilesPerFuel = 25000f;
    public const float MassMilesPerFuelPerCombustionController = 1000f;
    public const float MinSpeed = 0.00001f;

    public static readonly Dictionary<CartType, float> CartMass = new() {
        [CartType.Freight] = 1250f, 
        [CartType.Liquid] = 750f
    };

    public const int CartRows = 3; 
    public const int CartCols = 5; 
    public const int TrainRows = 1; 
    public const int TrainCols = 5; 
    public const int CityInvRows = 3; 
    public const int CityInvCols = 5; 
    public const int PlayerInvRows = 1; 
    public const int PlayerInvCols = 5; 

    public const string TrainStr = "Train"; 
    public const string PlayerInvID = "PlayerInv"; 
    public const string PlayerStr = "Player"; 

    public const string DefaultSaveFile = "game"; 

    public const float DefaultBulletSpeed = 2f; 
    public const int BulletSize = 5; 
    public const float DefaultBulletSize = 5f;
    public const float DefaultEnemySpeed = PlayerSpeed / 2f;

    public const float EnemySize = 50f;
    public const int InvincibilityFrames = 60;

    public const float ExponentialInvSizeUpgradeFactor = 1.1f; 
    public const float ExponentialMilesPerFuelUpgradeFactor = 1.1f;
    public const float ExponentialTrainPowerUpgradeFactor = 1.1f; 
    public const float ExponentialProductCountUpgradeFactor = 1.1f; 

    public static int ItemStackSize(string itemId) {
        return itemId switch {
            ItemID.Credit => 10000,
            ItemID.Fuel => 1000,
            ItemID.Glass => 500, 
            ItemID.Iron => 1000, 
            ItemID.Oil => 10000, 
            ItemID.Cobalt => 1000,
            ItemID.Sand => 1000,
            ItemID.TimeCrystal => 1000,
            ItemID.Water => 10000, 
            ItemID.Wood => 1000,
            ItemID.Mythril => 1000,
            ItemID.Adamantite => 1000,
            ItemID.Lubricant => 1000,
            ItemID.Petroleum => 1000,
            "StackSize1" => 1,
            _ => 100
        }; 
    }

    public static int FloorDifficulty(int floor) {
        if (floor >= 60) {
            return 12;
        } else {
            return floor / 5; 
        }
    }
}