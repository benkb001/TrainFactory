using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 
using System.Text.RegularExpressions; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components;
using TrainGame.Systems; 
using TrainGame.ECS; 
using TrainGame.Utils; 

namespace TrainGame.Constants 
{
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

        public const float PlayerWidth = 50f; 
        public const float PlayerHeight = 50f; 
        public const float PlayerSpeed = 5f; 
        public const int PlayerOutlineThickness = 1; 

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

        public const float TrainDefaultPower = 25000f; 
        public const float UpgradePowerStep = 1000f; 
        public const float TrainDefaultMass = 1000f; 

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
        public const int PlayerInvCols = 10; 

        public const string TrainStr = "Train"; 
        public const string PlayerInvID = "PlayerInv"; 
        public const string PlayerStr = "Player"; 

        public const string DefaultSaveFile = "game"; 

        public const float BulletSpeed = 8f; 
        public const int BulletSize = 5; 

        public static int ItemStackSize(string itemId) {
            return itemId switch {
                ItemID.ArmorUpgrade => 100,
                ItemID.Fuel => 100,
                ItemID.Glass => 50, 
                ItemID.GunUpgrade => 100,
                ItemID.Iron => 100, 
                ItemID.MachineUpgrade => 100, 
                ItemID.Oil => 1000, 
                ItemID.Rail => 100, 
                ItemID.Sand => 1000,
                ItemID.Water => 1000, 
                ItemID.Wood => 100,
                "StackSize1" => 1,
                _ => 100
            }; 
        }
    }

    public static class Textures {
        public static readonly string Button = "button";
        public static readonly string Pixel = "pixel"; 
    }

    public static class Depth {
        public static readonly int NextTestButton = 0; //??

        public const float PlayerOutline = 0.5f; 
        public const float PlayerBackground = 0.55f; 
        
        public static readonly float MapCity = 0.9f; 
        public static readonly float MapTrain = 0.8f; 
        public static readonly float MapCityDetail = 0.5f; 
    }

    public static class AspectRatio {
        public static readonly float Button = 1.5F; 
    }

    public static class KeyBinds {
        public static readonly Keys CameraMoveDown = Keys.Down;
        public static readonly Keys CameraMoveUp = Keys.Up;
        public static readonly Keys CameraMoveLeft = Keys.Left;
        public static readonly Keys CameraMoveRight = Keys.Right;
        public static readonly Keys MoveUp = Keys.W;
        public static readonly Keys MoveLeft = Keys.A;
        public static readonly Keys MoveDown = Keys.S;
        public static readonly Keys MoveRight = Keys.D;
        public static readonly Keys Interact = Keys.E; 
        public static readonly Keys OpenMap = Keys.M; 

        public static readonly Keys[] AlphaList = {
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, 
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, 
            Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, 
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, 
            Keys.Z
        };

        public static Dictionary<Keys, string> StringMap = new() {
            [Keys.Enter] = "\n",
            [Keys.Space] = " ",
            [Keys.Tab] = "    ", 
        };

        public static Dictionary<(Keys, bool), string> StringMapShift = new() {
            [(Keys.OemPlus, false)] = "=", 
            [(Keys.OemPlus, true)] = "+",
            [(Keys.D0, false)] = "0",
            [(Keys.D0, true)] = ")",
            [(Keys.D1, false)] = "1",
            [(Keys.D1, true)] = "!", 
            [(Keys.D2, false)] = "2",
            [(Keys.D2, true)] = "@",
            [(Keys.D3, false)] = "3",
            [(Keys.D3, true)] = "#",
            [(Keys.D4, false)] = "5",
            [(Keys.D4, true)] = "$",
            [(Keys.D5, false)] = "5",
            [(Keys.D5, true)] = "%",
            [(Keys.D6, false)] = "6",
            [(Keys.D6, true)] = "^",
            [(Keys.D7, false)] = "7",
            [(Keys.D7, true)] = "&",
            [(Keys.D8, false)] = "8", 
            [(Keys.D8, true)] = "*", 
            [(Keys.D9, false)] = "9",
            [(Keys.D9, true)] = ")",
            [(Keys.OemOpenBrackets, false)] = "[", 
            [(Keys.OemOpenBrackets, true)] = "{", 
            [(Keys.OemCloseBrackets, false)] = "]", 
            [(Keys.OemCloseBrackets, true)] = "}", 
            [(Keys.OemMinus, false)] = "-", 
            [(Keys.OemMinus, true)] = "_", 
            [(Keys.OemPeriod, false)] = ".",
            [(Keys.OemPeriod, true)] = ">",
            [(Keys.OemQuestion, false)] = "/",
            [(Keys.OemQuestion, true)] = "?",
            [(Keys.OemSemicolon, false)] = ";",
            [(Keys.OemSemicolon, true)] = ":"
            
        };
    }

    public static class Colors {
        public static readonly Color Placebo = Color.DarkGray; 
        public static readonly Color UIBG = Color.LightGray; 
        public static readonly Color UIAccent = Color.DarkGray; 
        public static readonly Color BG = Color.CornflowerBlue;
        public static readonly Color InventoryHeld = Color.Red; 
        public static readonly Color InventoryNotHeld = Color.White; 

        public static readonly Color PlayerOutline = Color.Black; 
        public static readonly Color PlayerBackground = Color.White; 
    }

    public class CityArg {
        public string[] Machines; 
        public float UiX; 
        public float UiY; 
        public float RealX; 
        public float RealY; 
        public string[] AdjacentCities; 

        public CityArg(string[] Machines, float UiX, float UiY, 
            float RealX, float RealY, string[] AdjacentCities) {

            this.Machines = Machines; 
            this.UiX = UiX; 
            this.UiY = UiY; 
            this.RealX = RealX; 
            this.RealY = RealY; 
            this.AdjacentCities = AdjacentCities; 
        }
    }

    public static class CityID {
        public const string Coast = "Coast"; 
        public const string Collisseum = "Collisseum"; 
        public const string Factory = "Factory"; 
        public const string Greenhouse = "Greenhouse"; 
        public const string Mine = "Mine"; 
        public const string Reservoir = "Reservoir"; 
        public const string Test = "Test"; 

        public static readonly List<string> All = [
            Coast, Collisseum, Factory, Greenhouse, 
            Mine, Reservoir
        ];

        public static readonly Dictionary<string, CityArg> CityMap = new() {
            [CityID.Factory] = new CityArg(
                [
                    MachineID.AssemblerFactory,
                    MachineID.CargoWagonAssembler, 
                    MachineID.DrillAssembler,
                    MachineID.ExcavatorAssembler,
                    MachineID.Gasifier, 
                    MachineID.GasifierAssembler,
                    MachineID.GreenhouseAssembler,
                    MachineID.Kiln, 
                    MachineID.KilnAssembler,
                    MachineID.LocomotiveAssembler, 
                    MachineID.LiquidWagonAssembler, 
                    MachineID.MotherboardAssembler,
                    MachineID.PumpAssembler,
                    MachineID.TrainUpgradeAssembler
                ], 
                550f, 210f, 0f, 0f, 
                [CityID.Greenhouse, CityID.Coast, CityID.Mine]
            ),
            [CityID.Greenhouse] = new CityArg(
                [MachineID.Greenhouse],
                550f, 10f, 0f, -2.5f, [CityID.Factory]
            ),
            [CityID.Coast] = new CityArg(
                [MachineID.Excavator, MachineID.Pump], 
                350f, 210f, -2.5f, 0f, [CityID.Factory]
            ),
            [CityID.Mine] = new CityArg(
                [MachineID.Drill], 
                550f, 410f, 0f, 2.5f, [CityID.Factory]
            )
        };
    }

    public static class ItemID {
        public const string ArmorUpgrade = "ArmorUpgrade"; 
        public const string Assembler = "Assembler"; 
        public const string Drill = "Drill"; 
        public const string Excavator = "Excavator"; 
        public const string Fuel = "Fuel"; 
        public const string Gasifier = "Gasifier";
        public const string Greenhouse = "Greenhouse"; 
        public const string Glass = "Glass"; 
        public const string Gun = "Gun"; 
        public const string GunUpgrade = "GunUpgrade"; 
        public const string Iron = "Iron"; 
        public const string Kiln = "Kiln"; 
        public const string MachineUpgrade = "MachineUpgrade"; 
        public const string Motherboard = "Motherboard"; 
        public const string Oil = "Oil"; 
        public const string Pump = "Pump"; 
        public const string Rail = "Rail"; 
        public const string Sand = "Sand"; 
        public const string Water = "Water"; 
        public const string Wood = "Wood"; 
        public const string TimeCrystal = "Time Crystal"; 
        public const string TrainUpgrade = "TrainUpgrade"; 

        public static readonly List<string> All = [
            ArmorUpgrade, Assembler, Drill, Excavator, Fuel, Gasifier, Greenhouse,
            Glass, Gun, GunUpgrade, Iron, Kiln, MachineUpgrade, Motherboard, Oil, 
            Pump, Rail, Sand, TimeCrystal, Water, Wood
        ]; 

        public static readonly List<string> Liquids = [
            Oil, Water
        ]; 

        public static readonly List<string> Solids = All.Where(s => !Liquids.Contains(s)).ToList(); 
    }

    public static class Weapons {
        public static Dictionary<string, int> GunMap = new() {
            [ItemID.Gun] = 1
        };
    }

    public static class MachineID {
        public const string AssemblerFactory = "Assembler Factory"; 
        public const string ArmorUpgradeAssembler = "Armor Upgrade Assembler"; 
        public const string CargoWagonAssembler = "Cargo Wagon Assembler"; 
        public const string Drill = "Drill"; 
        public const string DrillAssembler = "Drill Assembler"; 
        public const string Excavator = "Excavator"; 
        public const string ExcavatorAssembler = "Excavator Assembler"; 
        public const string Gasifier = "Gasifier"; 
        public const string GasifierAssembler = "GasifierAssembler"; 
        public const string Greenhouse = "Greenhouse"; 
        public const string GreenhouseAssembler = "Greenhouse Assembler";
        public const string GunUpgradeAssembler = "Gun Upgrade Assembler"; 
        public const string Kiln = "Kiln"; 
        public const string KilnAssembler = "KilnAssembler"; 
        public const string LiquidWagonAssembler = "Liquid Wagon Assembler"; 
        public const string LocomotiveAssembler = "Locomotive Assembler"; 
        public const string MotherboardAssembler = "Motherboard Assembler"; 
        public const string Pump = "Pump"; 
        public const string PumpAssembler = "Pump Assembler"; 
        public const string TrainUpgradeAssembler = "Train Upgrade Assembler"; 
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
            [MachineID.AssemblerFactory] = new MachineArg(
                ProductItemId: ItemID.Assembler, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 20
                },
                MinTicks: 600,
                UpgradeItemID: ItemID.Assembler,
                Level: 0
            ),
            [MachineID.Gasifier] = new MachineArg(
                ProductItemId: ItemID.Fuel, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Wood] = 1
                },
                MinTicks: 60, 
                UpgradeItemID: ItemID.Gasifier,
                Level: 0
            ), 
            [MachineID.GasifierAssembler] = new MachineArg(
                ProductItemId: ItemID.Gasifier, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Fuel] = 10, 
                    [ItemID.Iron] = 15, 
                    [ItemID.Glass] = 5
                },
                MinTicks: 480, 
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.Kiln] = new MachineArg(
                ProductItemId: ItemID.Glass, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Fuel] = 1, 
                    [ItemID.Sand] = 1
                },
                MinTicks: 60,
                UpgradeItemID: ItemID.Kiln
            ), 
            [MachineID.KilnAssembler] = new MachineArg(
                ProductItemId: ItemID.Kiln, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 10, 
                    [ItemID.Fuel] = 10
                },
                MinTicks: 540,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.LocomotiveAssembler] = new MachineArg(
                ProductItemId: "", 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 10
                },
                MinTicks: 600,
                UpgradeItemID: ItemID.Assembler
            ), 
            [MachineID.CargoWagonAssembler] = new MachineArg(
                ProductItemId: "", 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Wood] = 10
                },
                MinTicks: 600,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.Drill] = new MachineArg(
                ProductItemId: ItemID.Iron, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Fuel] = 1
                }, 
                MinTicks: 30,
                UpgradeItemID: ItemID.Drill,
                AllowManual: true
            ), 
            [MachineID.DrillAssembler] = new MachineArg(
                ProductItemId: ItemID.Drill,
                ProductCount: 1,
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 20, 
                    [ItemID.Fuel] = 5
                },
                MinTicks: 600, 
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.Excavator] = new MachineArg(
                ProductItemId: ItemID.Sand, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Fuel] = 2
                },
                MinTicks: 60,
                UpgradeItemID: ItemID.Excavator,
                AllowManual: true
            ), 
            [MachineID.ExcavatorAssembler] = new MachineArg(
                ProductItemId: ItemID.Excavator, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 20, 
                    [ItemID.Glass] = 5
                },
                MinTicks: 480,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.Greenhouse] = new MachineArg(
                ProductItemId: ItemID.Wood, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Water] = 10
                },
                MinTicks: 30,
                UpgradeItemID: ItemID.Greenhouse,
                AllowManual: true
            ),
            [MachineID.GreenhouseAssembler] = new MachineArg(
                ProductItemId: ItemID.Greenhouse,
                ProductCount: 1,
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Glass] = 20, 
                    [ItemID.Iron] = 10
                },
                MinTicks: 600,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.GunUpgradeAssembler] = new MachineArg(
                ProductItemId: ItemID.GunUpgrade, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 100, 
                    [ItemID.Glass] = 50, 
                    [ItemID.Fuel] = 200
                },
                MinTicks: 2400,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.LiquidWagonAssembler] = new MachineArg(
                ProductItemId: "", 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 10, 
                    [ItemID.Glass] = 5
                },
                MinTicks: 900,
                UpgradeItemID: ItemID.Assembler
            ), 
            [MachineID.MotherboardAssembler] = new MachineArg(
                ProductItemId: ItemID.Motherboard,
                ProductCount: 1,
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Glass] = 50,
                    [ItemID.Iron] = 20
                },
                MinTicks: 1200,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.TrainUpgradeAssembler] = new MachineArg(
                ProductItemId: ItemID.TrainUpgrade, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Glass] = 10, 
                    [ItemID.Iron] = 20
                },
                MinTicks: 90,
                UpgradeItemID: ItemID.Assembler
            ),
            [MachineID.Pump] = new MachineArg(
                ProductItemId: ItemID.Water, 
                ProductCount: 20, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Fuel] = 1
                }, 
                MinTicks: 10,
                UpgradeItemID: ItemID.Pump
            ),
            [MachineID.PumpAssembler] = new MachineArg(
                ProductItemId: ItemID.Pump, 
                ProductCount: 1, 
                Recipe: new Dictionary<string, int>() {
                    [ItemID.Iron] = 15, 
                    [ItemID.Glass] = 10
                },
                MinTicks: 600,
                UpgradeItemID: ItemID.Assembler
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

    public static class Bootstrap {
        public static void InitWorld(World w) {
            Dictionary<string, (int, City)> cities = new(); 
            Dictionary<string, (int, Machine)> machines = new(); 

            //initialize cities and machines 
            foreach (KeyValuePair<string, CityArg> kvp in CityID.CityMap) {
                string cityId = kvp.Key; 
                CityArg args = kvp.Value; 

                Inventory inv = new Inventory($"{cityId} Depot", Constants.CityInvRows, Constants.CityInvCols); 
                int invEnt = EntityFactory.Add(w, setData: true); 
                w.SetComponent<Inventory>(invEnt, inv); 

                int cityEnt = EntityFactory.Add(w, setData: true); 
                City c = new City(cityId, inv, args.UiX, args.UiY, args.RealX, args.RealY); 
                w.SetComponent<City>(cityEnt, c); 
                cities[cityId] = (cityEnt, c); 

                foreach (string machineID in args.Machines) {
                    int machineEnt = EntityFactory.Add(w, setData: true); 
                    Machine m = Machines.Get(inv, machineID); 
                    machines[machineID] = (machineEnt, m); 
                    w.SetComponent<Machine>(machineEnt, m); 
                    c.AddMachine(m); 
                }
            }

            //add city connections 
            foreach (KeyValuePair<string, (int, City)> kvp in cities) {
                string cityId = kvp.Key; 
                (int ent, City city) = kvp.Value; 
                
                foreach (string otherCityID in CityID.CityMap[cityId].AdjacentCities) {
                    (int _, City otherCity) = cities[otherCityID]; 
                    city.AddConnection(otherCity);
                }
            }

            //add player inventory

            int playerInvDataEnt = EntityFactory.Add(w, setData: true); 
            Inventory playerInv = new Inventory(Constants.PlayerInvID, 
                Constants.PlayerInvRows, Constants.PlayerInvCols);
            w.SetComponent<Inventory>(playerInvDataEnt, playerInv); 
            w.SetComponent<Player>(playerInvDataEnt, new Player()); 
            w.SetComponent<Health>(playerInvDataEnt, new Health(6)); 
            w.SetComponent<RespawnLocation>(playerInvDataEnt, new RespawnLocation(cities[CityID.Coast].Item2));

            playerInv.Add(ItemID.Gun, 1); 
            //Player.SetInventory(playerInv);

            //add one train to factory

            int trainInvDataEnt = EntityFactory.Add(w, setData: true); 
            Inventory trainInv = new Inventory("T0", Constants.TrainRows, Constants.TrainCols); 
            trainInv.SetSolid(); 
            w.SetComponent<Inventory>(trainInvDataEnt, trainInv); 

            (int _, City factory) = cities[CityID.Factory];
            
            Train t = new Train(trainInv, factory, "T0", 
                power: Constants.TrainDefaultPower, mass: Constants.TrainDefaultMass);
            int trainDataEnt = TrainWrap.Add(w, t);

            //add some fuel to factory
            factory.Inv.Add(new Inventory.Item(ItemId: ItemID.Fuel, Count: 50)); 
            factory.Inv.Add(ItemID.Drill, 1); 
            factory.Inv.Add(ItemID.Pump, 1); 
            factory.Inv.Add(ItemID.Greenhouse, 1); 
            factory.Inv.Add(ItemID.Assembler, 10); 
            factory.Inv.Add(ItemID.Gasifier, 1);
            factory.Inv.Add(ItemID.Kiln, 1); 
            factory.Inv.Add(ItemID.Excavator, 1); 
            factory.Inv.Add(ItemID.Motherboard, 5); 
            factory.Inv.Add(ItemID.Iron, 50); 
            factory.Inv.Add(ItemID.Glass, 20); 
            factory.AddCart(new Cart(CartType.Liquid)); 

            //set assembler components
            (int locomotiveAssemblerEnt, Machine locomotiveAssembler) = machines[MachineID.LocomotiveAssembler]; 
            w.SetComponent<TrainAssembler>(locomotiveAssemblerEnt, new TrainAssembler(factory, locomotiveAssembler)); 

            (int cargoWagonAssemblerEnt, Machine cargoAssembler) = machines[MachineID.CargoWagonAssembler]; 
            w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
                new CartAssembler(factory, cargoAssembler, CartType.Freight));

            (int liquidAssemblerEnt, Machine liquidAssembler) = machines[MachineID.LiquidWagonAssembler]; 
            w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
                new CartAssembler(factory, liquidAssembler, CartType.Liquid));

            //add player to factory 
            factory.HasPlayer = true; 
        }
    }
}