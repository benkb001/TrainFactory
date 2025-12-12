using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

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
        public static readonly Color UIBG = Color.LightGray; 
        public static readonly Color UIAccent = Color.DarkGray; 
        public static readonly Color BG = Color.CornflowerBlue;
        public static readonly Color InventoryHeld = Color.Red; 
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
}