using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 

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

        public static readonly float EmbarkLayoutWidth = 200f; 
        public static readonly float EmbarkLayoutHeight = 500f;
        public static readonly float EmbarkLayoutPadding = 5f;  

        public static float InventoryCellSize = 60f; 
        public static float InventoryPadding = 5f;

        public static float LabelHeight = 25f; 

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
    }

    public static class Colors {
        public static readonly Color Placebo = Color.DarkGray; 
        public static readonly Color UIBG = Color.LightGray; 
        public static readonly Color UIAccent = Color.DarkGray; 
        public static readonly Color BG = Color.CornflowerBlue;
        public static readonly Color InventoryHeld = Color.Red; 
        public static readonly Color InventoryNotHeld = Color.White; 
    }

    public static class CityID {
        public const string Coast = "Coast"; 
        public const string Collisseum = "Collisseum"; 
        public const string Factory = "Factory"; 
        public const string Greenhouse = "Greenhouse"; 
        public const string Mine = "Mine"; 
        public const string Reservoir = "Reservoir"; 
        public const string Test = "Test"; 
    }

    public static class ItemID {
        public const string ArmorUpgrade = "ArmorUpgrade"; 
        public const string Fuel = "Fuel"; 
        public const string Glass = "Glass"; 
        public const string GunUpgrade = "Gun Upgrade"; 
        public const string Iron = "Iron"; 
        public const string MachineUpgrade = "Machine Upgrade"; 
        public const string Oil = "Oil"; 
        public const string Rail = "Rail"; 
        public const string Sand = "Sand"; 
        public const string Water = "Water"; 
        public const string Wood = "Wood"; 
    }

    public static class Machines {

        public static Machine Gasifier(Inventory inv) {

            Dictionary<string, int> recipe = new() {
                [ItemID.Wood] = 1
            }; 
            string productItemId = ItemID.Fuel; 
            string machineId = "Gasifier"; 
            int productCount = 1; 
            int ticks = 60; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId); 
        }

        public static Machine Kiln(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                [ItemID.Sand] = 1, 
                [ItemID.Fuel] = 1
            }; 
            string productItemId = ItemID.Glass; 
            string machineId = "Kiln"; 
            int productCount = 1; 
            int ticks = 60; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId); 
        }

        public static Machine LocomotiveAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Iron"] = 10
            }; 
            string productItemId = "Locomotive"; 
            string machineId = "Locomitive Assembler"; 
            int productCount = 1; 
            int ticks = 600; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        } 

        public static Machine CargoWagonAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Wood"] = 10
            }; 
            string productItemId = "Cargo Wagon"; 
            string machineId = "Cargo Wagon Assembler"; 
            int productCount = 1; 
            int ticks = 600; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        } 

        public static Machine LiquidWagonAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Iron"] = 10, 
                ["Glass"] = 5
            }; 
            string productItemId = "Liquid Wagon"; 
            string machineId = "Liquid Wagon Assembler"; 
            int productCount = 1; 
            int ticks = 900; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        }

        public static Machine MachineUpgradeAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Iron"] = 10, 
                ["Glass"] = 10
            }; 
            string productItemId = "Machine Upgrade"; 
            string machineId = "Machine Upgrade Assembler"; 
            int productCount = 1; 
            int ticks = 1200; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        }

        public static Machine GunUpgradeAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Iron"] = 100, 
                ["Glass"] = 50, 
                ["Fuel"] = 200
            };

            string productItemId = "Gun Upgrade"; 
            string machineId = "Gun Upgrade Assembler"; 
            int productCount = 1; 
            int ticks = 2400; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        }

        public static Machine ArmorUpgradeAssembler(Inventory inv) {
            Dictionary<string, int> recipe = new() {
                ["Iron"] = 400
            };

            string productItemId = "Armor Upgrade"; 
            string machineId = "Armor Upgrade Assembler"; 
            int productCount = 1; 
            int ticks = 2000; 

            return new Machine(inv, recipe, productItemId, productCount, ticks, machineId);
        }
    }
}