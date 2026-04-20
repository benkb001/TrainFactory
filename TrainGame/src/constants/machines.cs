namespace TrainGame.Constants;

using System.Collections.Generic;
using TrainGame.Components;

public static class MachineID {
    public const string AcceleratorAssembler = "Accelerator Assembler";
    public const string AdamantiteDrill = "Adamantite Drill"; 
    public const string AdamantiteDrillAssembler = "Adamantite Drill Assembler"; 
    public const string AirResistorAssembler = "Air Resistor Assembler";
    public const string AntiGravityAssembler = "Anti Gravity Assembler";
    public const string AssemblerFactory = "Assembler Factory"; 
    public const string CargoWagonAssembler = "Cargo Wagon Assembler"; 
    public const string CobaltDrill = "Cobalt Drill"; 
    public const string CobaltDrillAssembler = "Cobalt Drill Assembler";
    public const string CombustionControllerAssembler = "Combustion Controller Assembler";
    public const string DepotUpgradeAssembler = "Depot Upgrade Assembler"; 
    public const string Drill = "Drill"; 
    public const string DrillAssembler = "Drill Assembler"; 
    public const string DuplicatorAssembler = "Duplicator Assembler";
    public const string EngineAssembler = "Engine Assembler";
    public const string Excavator = "Excavator"; 
    public const string ExcavatorAssembler = "Excavator Assembler"; 
    public const string FuelRefinery = "Fuel Refinery"; 
    public const string Gasifier = "Gasifier"; 
    public const string GasifierAssembler = "Gasifier Assembler"; 
    public const string Greenhouse = "Greenhouse"; 
    public const string GreenhouseAssembler = "Greenhouse Assembler";
    public const string Kiln = "Kiln"; 
    public const string KilnAssembler = "Kiln Assembler"; 
    public const string LiquidWagonAssembler = "Liquid Wagon Assembler"; 
    public const string LocomotiveAssembler = "Locomotive Assembler"; 
    public const string LubricantRefinery = "Lubricant Refinery"; 
    public const string MotherboardAssembler = "Motherboard Assembler"; 
    public const string MythrilDrill = "Mythril Drill"; 
    public const string MythrilDrillAssembler = "Mythril Drill Assembler"; 
    public const string OilRig = "Oil Rig";
    public const string OilRigAssembler = "Oil Rig Assembler";
    public const string SmartAssemblerFactory = "Smart Assembler Factory";
    public const string PetroleumRefinery = "Petroleum Refinery";
    public const string PocketDimensionAssembler = "Pocket Dimension Assembler";
    public const string Pump = "Pump"; 
    public const string PumpAssembler = "Pump Assembler"; 
    public const string RefineryAssembler = "Refinery Assembler";
    public const string TimeForest = "Time Forest";

    public static List<string> All = new() {
        AcceleratorAssembler, AdamantiteDrill, AdamantiteDrillAssembler, 
        AirResistorAssembler, AntiGravityAssembler,
        AssemblerFactory, CargoWagonAssembler, CobaltDrill, CobaltDrillAssembler, 
        CombustionControllerAssembler,
        DepotUpgradeAssembler, Drill, DrillAssembler, DuplicatorAssembler,
        EngineAssembler, Excavator, ExcavatorAssembler, FuelRefinery, Gasifier, 
        GasifierAssembler, Greenhouse, GreenhouseAssembler, 
        Kiln, KilnAssembler, LiquidWagonAssembler, LocomotiveAssembler, 
        LubricantRefinery, MotherboardAssembler, MythrilDrill, MythrilDrillAssembler, 
        OilRig, OilRigAssembler, 
        PetroleumRefinery, PocketDimensionAssembler,
        Pump, PumpAssembler, RefineryAssembler, SmartAssemblerFactory
    };
}

public class MachineArg {
    public readonly bool AllowManual; 
    public readonly string ProductItemId; 
    public readonly int ProductCount; 
    public readonly Dictionary<string, int> Recipe; 
    public readonly int MinTicks; 
    public readonly int SlowFactor; 
    public readonly int StartFactor; 
    public readonly string UpgradeItemID; 
    public readonly int Level;

    public MachineArg(string ProductItemId, int ProductCount, Dictionary<string, int> Recipe, int MinTicks, 
        int SlowFactor = 0, int StartFactor = 1, string UpgradeItemID = ItemID.MachineUpgrade,
        bool AllowManual = false, int Level = -1) {
        
        this.ProductItemId = ProductItemId; 
        this.ProductCount = ProductCount; 
        this.Recipe = Recipe; 
        this.MinTicks = MinTicks; 
        this.SlowFactor = SlowFactor; 
        this.StartFactor = StartFactor; 
        this.UpgradeItemID = UpgradeItemID; 
        this.AllowManual = AllowManual; 
        this.Level = Level; 
    }
}

public static class Machines {

    private static Dictionary<string, MachineArg> args = new() {
        [MachineID.AcceleratorAssembler] = new MachineArg(
            ProductItemId: ItemID.Accelerator,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Lubricant] = 1000,
                [ItemID.Petroleum] = 1000,
                [ItemID.Cobalt] = 1000,
                [ItemID.Engine] = 100
            },
            MinTicks: 3,
            SlowFactor: 18000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.AdamantiteDrill] = new MachineArg(
            ProductItemId: ItemID.Adamantite, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 10000
            },
            MinTicks: 3, 
            SlowFactor: 600, 
            StartFactor: 10,
            UpgradeItemID: ItemID.AdamantiteDrill
        ),
        [MachineID.AdamantiteDrillAssembler] = new MachineArg(
            ProductItemId: ItemID.AdamantiteDrill,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Adamantite] = 15000
            },
            MinTicks: 3, 
            SlowFactor: 6000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.AirResistorAssembler] = new MachineArg(
            ProductItemId: ItemID.AirResistor, 
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Lubricant] = 1000,
                [ItemID.Cobalt] = 1000,
                [ItemID.Iron] = 2000
            },
            MinTicks: 3,
            SlowFactor: 18000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.AntiGravityAssembler] = new MachineArg(
            ProductItemId: ItemID.AntiGravity,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Glass] = 1000,
                [ItemID.Fuel] = 2000,
                [ItemID.Cobalt] = 3000
            },
            MinTicks: 3,
            SlowFactor: 18000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.AssemblerFactory] = new MachineArg(
            ProductItemId: ItemID.Assembler, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 50, 
                [ItemID.Cobalt] = 50
            },
            MinTicks: 3,
            SlowFactor: 6000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler,
            Level: 0
        ),
        [MachineID.CobaltDrill] = new MachineArg(
            ProductItemId: ItemID.Cobalt, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 100
            }, 
            MinTicks: 3, 
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.CobaltDrill
        ),
        [MachineID.CobaltDrillAssembler] = new MachineArg(
            ProductItemId: ItemID.CobaltDrill,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Cobalt] = 5000
            },
            MinTicks: 3,
            SlowFactor: 6000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.CombustionControllerAssembler] = new MachineArg(
            ProductItemId: ItemID.CombustionController,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Lubricant] = 50, 
                [ItemID.Petroleum] = 100, 
                [ItemID.Mythril] = 100,
                [ItemID.Iron] = 500, 
                [ItemID.Fuel] = 500
            }, 
            MinTicks: 3,
            SlowFactor: 12000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.DepotUpgradeAssembler] = new MachineArg(
            ProductItemId: ItemID.DepotUpgrade, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Wood] = 500, 
                [ItemID.Iron] = 200
            }, 
            MinTicks: 3,
            SlowFactor: 60000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler, 
            Level: -1
        ),
        [MachineID.DuplicatorAssembler] = new MachineArg(
            ProductItemId: ItemID.Duplicator,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Wood] = 1000,
                [ItemID.Glass] = 2000,
                [ItemID.Petroleum] = 2000
            },
            MinTicks: 3,
            SlowFactor: 18000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.Gasifier] = new MachineArg(
            ProductItemId: ItemID.Fuel, 
            ProductCount: 2, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Wood] = 1
            },
            MinTicks: 3, 
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.Gasifier,
            Level: 0
        ), 
        [MachineID.GasifierAssembler] = new MachineArg(
            ProductItemId: ItemID.Gasifier, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 50, 
                [ItemID.Iron] = 100, 
                [ItemID.Glass] = 50
            },
            MinTicks: 3, 
            SlowFactor: 4800,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.Kiln] = new MachineArg(
            ProductItemId: ItemID.Glass, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 1, 
                [ItemID.Sand] = 1
            },
            MinTicks: 3,
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.Kiln
        ), 
        [MachineID.KilnAssembler] = new MachineArg(
            ProductItemId: ItemID.Kiln, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 100, 
                [ItemID.Fuel] = 10
            },
            MinTicks: 3,
            SlowFactor: 5400,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.LocomotiveAssembler] = new MachineArg(
            ProductItemId: "", 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 500,
                [ItemID.Cobalt] = 500
            },
            MinTicks: 3,
            SlowFactor: 24000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ), 
        [MachineID.MythrilDrill] = new MachineArg(
            ProductItemId: ItemID.Mythril,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 1000
            },
            MinTicks: 3,
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.MythrilDrill
        ),
        [MachineID.MythrilDrillAssembler] = new MachineArg(
            ProductItemId: ItemID.MythrilDrill,
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Mythril] = 10000
            },
            MinTicks: 3,
            SlowFactor: 6000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.CargoWagonAssembler] = new MachineArg(
            ProductItemId: "", 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Wood] = 200
            },
            MinTicks: 3,
            SlowFactor: 6000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.Drill] = new MachineArg(
            ProductItemId: ItemID.Iron, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 1
            }, 
            MinTicks: 3,
            SlowFactor: 300,
            StartFactor: 10,
            UpgradeItemID: ItemID.Drill,
            AllowManual: true
        ), 
        [MachineID.DrillAssembler] = new MachineArg(
            ProductItemId: ItemID.Drill,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 250, 
                [ItemID.Fuel] = 100
            },
            MinTicks: 3, 
            SlowFactor: 6000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.EngineAssembler] = new MachineArg(
            ProductItemId: ItemID.Engine, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Lubricant] = 50, 
                [ItemID.Iron] = 200, 
                [ItemID.Fuel] = 100,
                [ItemID.Mythril] = 100
            }, 
            MinTicks: 3,
            SlowFactor: 6000, 
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.Excavator] = new MachineArg(
            ProductItemId: ItemID.Sand, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 2
            },
            MinTicks: 3,
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.Excavator,
            AllowManual: true
        ), 
        [MachineID.ExcavatorAssembler] = new MachineArg(
            ProductItemId: ItemID.Excavator, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 100, 
                [ItemID.Glass] = 10
            },
            MinTicks: 3,
            SlowFactor: 4800,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.FuelRefinery] = new MachineArg(
            ProductItemId: ItemID.Fuel, 
            ProductCount: 4, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Oil] = 1, 
                [ItemID.Water] = 1
            },
            MinTicks: 3,
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.Refinery
        ),
        [MachineID.Greenhouse] = new MachineArg(
            ProductItemId: ItemID.Wood, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Water] = 5
            },
            MinTicks: 3,
            SlowFactor: 300,
            StartFactor: 10,
            UpgradeItemID: ItemID.Greenhouse,
            AllowManual: true,
            Level: 0
        ),
        [MachineID.GreenhouseAssembler] = new MachineArg(
            ProductItemId: ItemID.Greenhouse,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Glass] = 100, 
                [ItemID.Iron] = 100
            },
            MinTicks: 3,
            SlowFactor: 6000,
            StartFactor: 10,
            Level: 0,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.LiquidWagonAssembler] = new MachineArg(
            ProductItemId: "", 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 400, 
                [ItemID.Glass] = 100
            },
            MinTicks: 3,
            SlowFactor: 9000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ), 
        [MachineID.LubricantRefinery] = new MachineArg(
            ProductItemId: ItemID.Lubricant, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Oil] = 10, 
                [ItemID.Water] = 5, 
                [ItemID.Petroleum] = 1
            },
            MinTicks: 3,
            SlowFactor: 1800,
            StartFactor: 10,
            UpgradeItemID: ItemID.Refinery
        ),
        [MachineID.MotherboardAssembler] = new MachineArg(
            ProductItemId: ItemID.Motherboard,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Glass] = 200,
                [ItemID.Mythril] = 200,
                [ItemID.Iron] = 400,
                [ItemID.Cobalt] = 400
            },
            MinTicks: 3,
            SlowFactor: 12000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.OilRig] = new MachineArg(
            ProductItemId: ItemID.Oil, 
            ProductCount: 20, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 1
            },
            MinTicks: 3,
            SlowFactor: 600,
            StartFactor: 10,
            UpgradeItemID: ItemID.OilRig
        ),
        [MachineID.OilRigAssembler] = new MachineArg(
            ProductItemId: ItemID.OilRig, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 500, 
                [ItemID.Mythril] = 250, 
                [ItemID.Fuel] = 250
            },
            MinTicks: 3,
            SlowFactor: 12000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.RefineryAssembler] = new MachineArg(
            ProductItemId: ItemID.Refinery,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Mythril] = 250, 
                [ItemID.Iron] = 250, 
                [ItemID.Oil] = 250, 
                [ItemID.Water] = 1000
            },
            MinTicks: 3,
            SlowFactor: 6000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.PetroleumRefinery] = new MachineArg(
            ProductItemId: ItemID.Petroleum, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 1,
                [ItemID.Oil] = 2, 
                [ItemID.Water] = 10
            },
            MinTicks: 3,
            SlowFactor: 1200,
            StartFactor: 10,
            UpgradeItemID: ItemID.Refinery
        ),
        [MachineID.PocketDimensionAssembler] = new MachineArg(
            ProductItemId: ItemID.PocketDimension,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Glass] = 2000,
                [ItemID.Petroleum] = 2000
            },
            MinTicks: 3,
            SlowFactor: 18000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler
        ),
        [MachineID.Pump] = new MachineArg(
            ProductItemId: ItemID.Water, 
            ProductCount: 40, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Fuel] = 1
            }, 
            MinTicks: 3,
            SlowFactor: 100,
            StartFactor: 10,
            UpgradeItemID: ItemID.Pump
        ),
        [MachineID.PumpAssembler] = new MachineArg(
            ProductItemId: ItemID.Pump, 
            ProductCount: 1, 
            Recipe: new Dictionary<string, int>() {
                [ItemID.Iron] = 200
            },
            MinTicks: 3,
            SlowFactor: 6000,
            StartFactor: 10,
            UpgradeItemID: ItemID.Assembler
        ),
        [MachineID.SmartAssemblerFactory] = new MachineArg(
            ProductItemId: ItemID.SmartAssembler, 
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Assembler] = 100,
                [ItemID.Engine] = 100,
                [ItemID.Lubricant] = 1000,
                [ItemID.Adamantite] = 1000
            },
            MinTicks: 3,
            SlowFactor: 18000,
            StartFactor: 10,
            UpgradeItemID: ItemID.SmartAssembler, 
            Level: 0
        ),
        [MachineID.TimeForest] = new MachineArg(
            ProductItemId: ItemID.TimeCrystal,
            ProductCount: 1,
            Recipe: new Dictionary<string, int>() {
                [ItemID.Water] = 1000
            },
            MinTicks: 600, 
            SlowFactor: 60000,
            StartFactor: 10,
            UpgradeItemID: ItemID.TimeSeed
        )
    }; 

    public static Machine Get(Inventory inv, string id, int curCraftTicks = 0, 
        CraftState state = CraftState.Idle, int level = -2, int priority = 0, 
        int numRecipeToStore = 0) {
        
        MachineArg arg = args[id]; 
        return new Machine(inv, arg.Recipe, arg.ProductItemId, arg.ProductCount, arg.MinTicks, 
            id, arg.SlowFactor, arg.StartFactor, upgradeItemID: arg.UpgradeItemID,
            allowManual: arg.AllowManual, level: level < -1 ? arg.Level : level, 
            curCraftTicks: curCraftTicks, state: state, priority: priority, 
            numRecipeToStore: numRecipeToStore);
    }

}