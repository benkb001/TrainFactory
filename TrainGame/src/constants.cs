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

        public const float TileWidth = 50f;

        public const float PlayerWidth = 45f; 
        public const float PlayerHeight = TileWidth; 
        public const float PlayerSpeed = 5f; 
        public const int PlayerOutlineThickness = 1; 
        public const int PlayerHP = 100;

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

        public static readonly Color Warning = Color.Red; 
        public static readonly Color Vampiric = Color.Purple;
    }

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

    public static class EnemyID {
        public static readonly Dictionary<EnemyType, EnemyConst> Enemies = new() {
            [EnemyType.Artillery] = new EnemyConst(
                new Shooter(
                    ammo: 4, 
                    ticksPerShot: 100, 
                    reloadTicks: 300
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(15, maxFramesActive: 600),
                        traits: new List<IBulletTrait>(){
                            new Homing()
                        }
                    )
                ), 
                new DefaultMovePattern(
                    ticksToMove: 60, 
                    ticksToWait: 60,
                    speed: 0.5f
                ),
                Type: EnemyType.Artillery, 
                HP: 15, 
                Size: Constants.TileWidth * 2, 
                Difficulty: 2
            ),
            [EnemyType.Barbarian] = new EnemyConst(
                new Shooter(
                    ticksPerShot: 200, 
                    reloadTicks: 200, 
                    ammo: 1
                ),
                new MeleeShootPattern(
                    new BulletContainer(
                        new Bullet(1, maxFramesActive: 10),
                        width: Constants.TileWidth * 3,
                        traits: new List<IBulletTrait>(){
                            new Warned(new WorldTime(ticks: 45)),
                            new RemoveOnCollision()
                        }
                    )
                ), 
                new DefaultMovePattern(
                    ticksToMove: 60,
                    ticksToWait: 200,
                    speed: Constants.PlayerSpeed / 3f
                ),
                Type: EnemyType.Barbarian, 
                HP: 20, 
                Size: Constants.TileWidth,
                Difficulty: 2
            ),
            [EnemyType.Default] = new EnemyConst(
                new Shooter(),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(5),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    )
                ),
                new DefaultMovePattern()
            ),
            [EnemyType.MachineGun] = new EnemyConst(
                new Shooter(
                    ammo: 36, 
                    reloadTicks: 120
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(15),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    Inaccuracy: 10
                ),
                new ChaseMovePattern(
                    Constants.PlayerSpeed / 2f
                ),
                Type: EnemyType.MachineGun,
                HP: 12
            ),
            [EnemyType.Ninja] = new EnemyConst(
                new Shooter(
                    ticksPerShot: 10, 
                    ammo: 2, 
                    reloadTicks: 120
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(5),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    Inaccuracy: 10
                ),
                new ChaseMovePattern(
                    Constants.PlayerSpeed / 1.5f
                ),
                Type: EnemyType.Ninja, 
                HP: 6
            ),
            [EnemyType.Robot] = new EnemyConst(
                new Shooter(
                    ammo: 32, 
                    ticksPerShot: 4, 
                    reloadTicks: 60
                ),
                new RadialShootPattern(
                    2,
                    new BulletContainer(
                        new Bullet(10),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ), 
                    Math.PI / 2
                ),
                new CyclicalMovePattern(
                    new List<Vector2>() { new Vector2(1, 0), new Vector2(-1, 0) },
                    new List<WorldTime>() { new WorldTime(ticks: 60 ) },
                    new WorldTime(ticks: 360),
                    1f
                ),
                Type: EnemyType.Robot, 
                HP: 8
            ),
            [EnemyType.Shotgun] = new EnemyConst(
                new Shooter(
                    ammo: 12, 
                    ticksPerShot: 120, 
                    reloadTicks: 240
                ),
                new ShotgunShootPattern(
                    new BulletContainer(
                        new Bullet(10, maxFramesActive: 180),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    4,
                    Math.PI / 5
                ),
                new DefaultMovePattern(),
                Type: EnemyType.Shotgun, 
                HP: 8
            ),
            [EnemyType.Sniper] = new EnemyConst(
                new Shooter(
                    ammo: 1, 
                    reloadTicks: 300
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(40, maxFramesActive: 240),
                        BulletSpeed: 10f,
                        traits: new List<IBulletTrait>(){
                            new Warned(new WorldTime(ticks: 20)),
                            new RemoveOnCollision()
                        }
                    )
                ),
                new DefaultMovePattern(),
                Type: EnemyType.Sniper, 
                HP: 25
            ),
            [EnemyType.Volley] = new EnemyConst(
                new Shooter(
                    ammo: 24, 
                    reloadTicks: 200, 
                    ticksPerShot: 60
                ),
                new ShotgunShootPattern(
                    new BulletContainer(
                        new Bullet(40, maxFramesActive: 60),
                        BulletSpeed: 3f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    12,
                    Math.PI / 1.5
                ),
                new DefaultMovePattern(),
                Type: EnemyType.Volley, 
                HP: 25
            ),
            [EnemyType.Vampire] = new EnemyConst(
                new Shooter(
                    ammo: 1,
                    reloadTicks: 600
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(10, maxFramesActive: 600),
                        BulletSpeed: Constants.PlayerSpeed / 1.5f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision(),
                            new Vampiric(5),
                            new Homing()
                        }
                    )
                ),
                new CyclicalMovePattern(
                    new List<Vector2>(){
                        new Vector2(0.5f, -0.86f),
                        new Vector2(0.5f, 0.86f),
                        new Vector2(-1f, 0f)
                    }, 
                    new List<WorldTime>(){
                        new WorldTime(ticks: 10), 
                        new WorldTime(ticks: 60),
                        new WorldTime(ticks: 10)
                    },
                    new WorldTime(ticks: 10),
                    Speed: 8f
                ),
                Type: EnemyType.Vampire,
                HP: 100,
                Size: Constants.TileWidth * 1.5f
            ),
            [EnemyType.Warrior] = new EnemyConst(
                new Shooter(
                    ammo: 20, 
                    reloadTicks: 480
                ),
                new ShotgunShootPattern(
                    new BulletContainer(
                        new Bullet(60, maxFramesActive: 240),
                        BulletSpeed: 4f,
                        traits: new List<IBulletTrait>(){
                            new Warned(new WorldTime(ticks: 30)),
                            new RemoveOnCollision()
                        }
                    ),
                    20, 
                    Math.PI
                ),
                new DefaultMovePattern(),
                Type: EnemyType.Warrior, 
                HP: 40,
                Size: Constants.TileWidth * 2
            ),
            [EnemyType.Wizard] = new EnemyConst(
                new Shooter(
                    ammo: 10,
                    ticksPerShot: 12,
                    reloadTicks: 120
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(60, maxFramesActive: 3600),
                        traits: new List<IBulletTrait>(){
                            new ParametricCurve(
                                //spiral
                                (t) => {
                                    t = t + 1; 
                                    float r = 10 + (t * 0.3f);
                                    float theta = MathF.Log(1 + t*5) * 5.0f;
                                    return (float)(r * MathF.Cos(theta));
                                },
                                (t) => {
                                    t = t + 1; 
                                    float r = 10 + (t * 0.3f);
                                    float theta =  MathF.Log(1 + t*5) * 5.0f;
                                    return (float)(r * MathF.Sin(theta));
                                }
                            ),
                            new RemoveOnCollision()
                        }
                    )
                ),
                new DefaultMovePattern(0),
                Type: EnemyType.Wizard,
                HP: 40,
                Size: Constants.TileWidth
            )
        };
    }

    public static class EquipmentID {
        public static List<string> Armor = new() {
            ItemID.Armor1, ItemID.Armor2, ItemID.Armor3
        };

        public static void InitMaps() {
            EquipmentSlot<Armor>.EquipmentMap = new() {
                [ItemID.Armor1] = new Armor(5),
                [ItemID.Armor2] = new Armor(20),
                [ItemID.Armor3] = new Armor(40)
            };
        }
    }

    public static class ItemID {
        public const string Accelerator = "Accelerator";
        public const string Adamantite = "Adamntite"; 
        public const string AdamantiteDrill = "Adamantite Drill"; 
        public const string AirResistor = "Air Resistor";
        public const string AntiGravity = "Anti Gravity";
        public const string Armor1 = "Armor1"; 
        public const string Armor2 = "Armor2"; 
        public const string Armor3 = "Armor3"; 
        public const string Assembler = "Assembler"; 
        public const string Cobalt = "Cobalt"; 
        public const string CobaltDrill = "Cobalt Drill"; 
        public const string CombustionController = "Combustion Controller";
        public const string Credit = "Credit";
        public const string DepotUpgrade = "Depot Upgrade"; 
        public const string Duplicator = "Duplicator";
        public const string Drill = "Drill"; 
        public const string Engine = "Engine";
        public const string Excavator = "Excavator"; 
        public const string Fuel = "Fuel"; 
        public const string Gasifier = "Gasifier";
        public const string Greenhouse = "Greenhouse"; 
        public const string Glass = "Glass"; 
        public const string Gun = "Gun"; 
        public const string Gun2 = "Gun2"; 
        public const string Gun3 = "Gun3"; 
        public const string GunUpgrade = "Gun Upgrade"; 
        public const string Iron = "Iron"; 
        public const string Kiln = "Kiln"; 
        public const string Lubricant = "Lubricant";
        public const string MachineUpgrade = "Machine Upgrade"; 
        public const string Motherboard = "Motherboard"; 
        public const string Mythril = "Mythril";
        public const string MythrilDrill = "Mythril Drill";
        public const string Oil = "Oil"; 
        public const string OilRig = "Oil Rig";
        public const string Petroleum = "Petroleum";
        public const string PocketDimension = "Pocket Dimension";
        public const string Pump = "Pump"; 
        public const string Rail = "Rail"; 
        public const string Refinery = "Refinery";
        public const string Sand = "Sand"; 
        public const string SmartAssembler = "Smart Assembler";
        public const string Water = "Water"; 
        public const string Wood = "Wood"; 
        public const string TimeCrystal = "Time Crystal"; 

        public static readonly List<string> All = [
            Accelerator, Adamantite, AdamantiteDrill, AirResistor, AntiGravity, Armor1, Armor2, 
            Armor3, Assembler, Cobalt, CobaltDrill, Credit, DepotUpgrade,
            Drill, Duplicator, Engine, Excavator, Fuel, Gasifier, Greenhouse,
            Glass, Gun, Gun2, Gun3, GunUpgrade, Iron, 
            Kiln, Lubricant, MachineUpgrade, Motherboard, Mythril, MythrilDrill, Oil, 
            OilRig, Petroleum, PocketDimension, Pump, Rail, Refinery, Sand, 
            SmartAssembler, TimeCrystal, Water, Wood
        ]; 

        public static readonly List<string> Liquids = [
            Oil, Lubricant, Petroleum, Water
        ]; 

        public static readonly List<string> Solids = All.Where(s => !Liquids.Contains(s)).ToList(); 
    }

    public class PlayerGun {

        private IShootPattern shootPattern;
        private Shooter shooter;
        public IShootPattern GetShootPattern() => shootPattern;
        public Shooter GetShooter() => shooter; 

        public PlayerGun(Shooter shooter, IShootPattern shootPattern) {
            this.shooter = shooter; 
            this.shootPattern = shootPattern;
        }
    }

    public static class Weapons {
        public static Dictionary<string, PlayerGun> PlayerGunMap = new() {
            [ItemID.Gun] = new PlayerGun(
                new Shooter(
                    ammo: 8, 
                    ticksPerShot: 30, 
                    reloadTicks: 60
                ), 
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(1),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    )
                )
            ),
            [ItemID.Gun2] = new PlayerGun(
                new Shooter(
                    ammo: 16, 
                    ticksPerShot: 15, 
                    reloadTicks: 30
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(1),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    )
                )
            ),
            [ItemID.Gun3] = new PlayerGun(
                new Shooter(
                    ammo: 32, 
                    ticksPerShot: 8, 
                    reloadTicks: 16
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(1),
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    )
                )
            )
        };
    }

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
        public const string GunUpgradeAssembler = "Gun Upgrade Assembler"; 
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

        public static List<string> All = new() {
            AcceleratorAssembler, AdamantiteDrill, AdamantiteDrillAssembler, 
            AirResistorAssembler, AntiGravityAssembler,
            AssemblerFactory, CargoWagonAssembler, CobaltDrill, CobaltDrillAssembler, 
            CombustionControllerAssembler,
            DepotUpgradeAssembler, Drill, DrillAssembler, DuplicatorAssembler,
            EngineAssembler, Excavator, ExcavatorAssembler, FuelRefinery, Gasifier, 
            GasifierAssembler, Greenhouse, GreenhouseAssembler, GunUpgradeAssembler, 
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
                    [ItemID.Adamantite] = 10000
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
                    [ItemID.Cobalt] = 10000
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
                SlowFactor: 6000,
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
                    [ItemID.Iron] = 150, 
                    [ItemID.Glass] = 20
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
        public const string HPPVendor = "HPP Vendor"; 

        public static List<String> All = new() {
            ArmorCraftsman, WeaponCraftsman, HPPVendor
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
                PurchaseInfo.AddItemInfo(ItemID.Gun2, 1, new () {
                    [ItemID.Credit] = 1000, 
                    [ItemID.Water] = 1000,
                    [ItemID.Iron] = 500, 
                    [ItemID.Cobalt] = 500,
                    [ItemID.Fuel] = 500
                }),
                PurchaseInfo.AddItemInfo(ItemID.Gun3, 1, new () {
                    [ItemID.Credit] = 2000, 
                    [ItemID.Iron] = 2000,
                    [ItemID.Mythril] = 500,
                    [ItemID.Petroleum] = 500,
                    [ItemID.Lubricant] = 100
                }),
            },
            [HPPVendor] = new() {
                PurchaseInfo.AddResetHP(),
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

        public static int GetResetHPCost(Inventory dest, Health playerHP) {
            float proportionMissing = 1f - ((float)playerHP.HP / playerHP.MaxHP);
            int cost = (int)(proportionMissing * 0.5 * (1000 + dest.ItemCount(ItemID.Credit)));
            return cost; 
        }
    }

    public static class Bootstrap {
        public static void InitWorld(World w) {
            EquipmentID.InitMaps(); 
            
            Dictionary<string, (int, City)> cities = new(); 
            Dictionary<string, (int, Machine)> machines = new(); 

            //initialize cities and machines 
            foreach (KeyValuePair<string, CityArg> kvp in CityID.CityMap) {
                string cityId = kvp.Key; 
                CityArg args = kvp.Value; 

                Inventory inv = new Inventory($"{cityId} Depot", Constants.CityInvRows, Constants.CityInvCols); 
                int invEnt = EntityFactory.AddData<Inventory>(w, inv); 
                w.SetComponent<Inventory>(invEnt, inv); 

                City c = new City(cityId, inv, args.UiX, args.UiY, args.RealX, args.RealY); 
                int cityEnt = EntityFactory.AddData<City>(w, c); 
                cities[cityId] = (cityEnt, c); 

                foreach (string machineID in args.Machines) {
                    Machine m = Machines.Get(inv, machineID); 
                    int machineEnt = EntityFactory.AddData<Machine>(w, m); 
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

            //add player data

            PlayerWrap.AddData(w);
            PlayerWrap.SetRespawn(w, cities[CityID.Factory].Item2);

            //add one train to factory

            Inventory trainInv = new Inventory("T0", Constants.TrainRows, Constants.TrainCols); 
            int trainInvDataEnt = EntityFactory.AddData<Inventory>(w, trainInv); 

            (int _, City factory) = cities[CityID.Factory];
            
            TrainWrap.AssembleToWorld(w, factory);

            //add some fuel to factory
            factory.Inv.Add(new Inventory.Item(ItemId: ItemID.Fuel, Count: 50)); 

            //set assembler components
            (int locomotiveAssemblerEnt, Machine locomotiveAssembler) = machines[MachineID.LocomotiveAssembler]; 
            w.SetComponent<TrainAssembler>(locomotiveAssemblerEnt, new TrainAssembler(factory, locomotiveAssembler)); 

            (int cargoWagonAssemblerEnt, Machine cargoAssembler) = machines[MachineID.CargoWagonAssembler]; 
            w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
                new CartAssembler(factory, cargoAssembler, CartType.Freight));

            (int liquidAssemblerEnt, Machine liquidAssembler) = machines[MachineID.LiquidWagonAssembler]; 
            w.SetComponent<CartAssembler>(liquidAssemblerEnt, 
                new CartAssembler(factory, liquidAssembler, CartType.Liquid));

            //add player to factory 
            factory.HasPlayer = true; 
        }
    }
}