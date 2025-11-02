using Microsoft.Xna.Framework.Input;

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
        
    }

    public static class Textures {
        public static readonly string Button = "button";
        public static readonly string Pixel = "pixel"; 
    }

    public static class Depth {
        public static readonly int NextTestButton = 0; 
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
    }
}