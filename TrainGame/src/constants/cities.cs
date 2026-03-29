namespace TrainGame.Constants;

using System.Collections.Generic;

public class CityArg {
    public string[] Machines; 
    public float UiX; 
    public float UiY; 
    public float RealX; 
    public float RealY; 
    public string[] AdjacentCities; 
    public Dictionary<string, Dictionary<string, int>> FutureConnections;

    public CityArg(string[] Machines, float UiX, float UiY, 
        float RealX, float RealY, string[] AdjacentCities, 
        Dictionary<string, Dictionary<string, int>> FutureConnections) {

        this.Machines = Machines; 
        this.UiX = UiX; 
        this.UiY = UiY; 
        this.RealX = RealX; 
        this.RealY = RealY; 
        this.AdjacentCities = AdjacentCities; 
        this.FutureConnections = FutureConnections;
    }
}

public static class CityID {
    public const string Coast = "Coast"; 
    public const string Factory = "Factory"; 
    public const string Greenhouse = "Greenhouse"; 
    public const string Laboratory = "Laboratory";
    public const string Mine = "Mine"; 
    public const string Refinery = "Refinery";
    public const string Reservoir = "Reservoir"; 
    public const string TrainYard = "Train Yard";
    public const string Test = "Test"; 

    public static readonly List<string> All = [
        Coast, Factory, Greenhouse, 
        Mine, Refinery, Reservoir, TrainYard
    ];

    private static Dictionary<string, Dictionary<string, int>> noConnections = new(); 
    private static Dictionary<string, Dictionary<string, int>> railroadCosts = new() {
        [CityID.Refinery] = new Dictionary<string, int>() {
            [ItemID.Oil] = 100, 
            [ItemID.Iron] = 100, 
            [ItemID.Wood] = 250
        }, 
        [CityID.Reservoir] = new Dictionary<string, int>() {
            [ItemID.Iron] = 500, 
            [ItemID.Wood] = 500, 
            [ItemID.Glass] = 100, 
            [ItemID.Water] = 3000
        },
        [CityID.TrainYard] = new Dictionary<string, int>() {
            [ItemID.Petroleum] = 100, 
            [ItemID.Lubricant] = 100,
            [ItemID.Iron] = 2000, 
            [ItemID.Wood] = 2000
        },
        [CityID.Laboratory] = new Dictionary<string, int>() {
            [ItemID.Engine] = 100,
            [ItemID.CombustionController] = 100,
            [ItemID.Iron] = 10000, 
            [ItemID.Wood] = 10000
        }
    };

    public static readonly Dictionary<string, CityArg> CityMap = new() {
        [CityID.Factory] = new CityArg(
            [
                MachineID.AssemblerFactory,
                MachineID.CargoWagonAssembler, 
                MachineID.DepotUpgradeAssembler,
                MachineID.ExcavatorAssembler,
                MachineID.Gasifier, 
                MachineID.GasifierAssembler,
                MachineID.GreenhouseAssembler,
                MachineID.Kiln, 
                MachineID.KilnAssembler,
                MachineID.LocomotiveAssembler, 
                MachineID.LiquidWagonAssembler, 
                MachineID.MotherboardAssembler,
                MachineID.OilRigAssembler,
                MachineID.PumpAssembler,
                MachineID.RefineryAssembler
            ], 
            550f, 210f, 0f, 0f, 
            [CityID.Greenhouse, CityID.Coast, CityID.Mine],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.Reservoir] = railroadCosts[CityID.Reservoir]
            }
        ),
        [CityID.Greenhouse] = new CityArg(
            [MachineID.Greenhouse],
            550f, 10f, 0f, -2.5f, 
            [CityID.Factory],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.Refinery] = railroadCosts[CityID.Refinery],
                [CityID.Laboratory] = railroadCosts[CityID.Laboratory]
            }
        ),
        [CityID.Coast] = new CityArg(
            [MachineID.Excavator, MachineID.Pump], 
            350f, 210f, -2.5f, 0f, 
            [CityID.Factory],
            new Dictionary<string, Dictionary<string, int>>() {
                
            }
        ),
        [CityID.Mine] = new CityArg(
            [
                MachineID.Drill,
                MachineID.CobaltDrill,
                MachineID.MythrilDrill,
                MachineID.AdamantiteDrill,
                MachineID.DrillAssembler,
                MachineID.CobaltDrillAssembler,
                MachineID.MythrilDrillAssembler,
                MachineID.AdamantiteDrillAssembler
            ], 
            550f, 410f, 0f, 2.5f, 
            [CityID.Factory],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.TrainYard] = railroadCosts[CityID.TrainYard]
            }
        ),
        [CityID.Reservoir] = new CityArg(
            [MachineID.OilRig], 
            750f, 210f, 2.5f, 0f,
            [],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.Factory] = railroadCosts[CityID.Reservoir],
                [CityID.Laboratory] = railroadCosts[CityID.Laboratory],
                [CityID.TrainYard] = railroadCosts[CityID.TrainYard]
            }
        ),
        [CityID.Refinery] = new CityArg(
            [MachineID.FuelRefinery, MachineID.LubricantRefinery, MachineID.PetroleumRefinery],
            350f, 10f, -2.5f, -2.5f, 
            [],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.Coast] = railroadCosts[CityID.Refinery], 
                [CityID.Greenhouse] = railroadCosts[CityID.Refinery]
            }
        ),
        [CityID.TrainYard] = new CityArg(
            [MachineID.EngineAssembler, MachineID.CombustionControllerAssembler],
            750f, 410f, 2.5f, 2.5f,
            [],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.Mine] = railroadCosts[CityID.TrainYard],
                [CityID.Reservoir] = railroadCosts[CityID.TrainYard]
            }
        ),
        [CityID.Laboratory] = new CityArg(
            [MachineID.SmartAssemblerFactory, MachineID.AcceleratorAssembler,
                MachineID.AntiGravityAssembler, MachineID.AirResistorAssembler, 
                MachineID.DuplicatorAssembler, MachineID.PocketDimensionAssembler],
            750f, 10f, 2.5f, -2.5f,
            [],
            new Dictionary<string, Dictionary<string, int>>() {
                [CityID.TrainYard] = railroadCosts[CityID.Laboratory],
                [CityID.Greenhouse] = railroadCosts[CityID.Laboratory]
            }
        )
    };
}