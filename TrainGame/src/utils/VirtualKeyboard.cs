namespace TrainGame.Utils; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class VirtualKeyboard {
    private static bool useVirtualKeyboard = false; 
    private static KeyboardState vks = new KeyboardState([]); 
    private static List<Keys> keysPressed = new List<Keys>(); 
    
    public static void UseVirtualKeyboard() {
        useVirtualKeyboard = true; 
    }

    public static void PressW() {
        UseVirtualKeyboard(); 
        keysPressed.Add(Keys.W);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void PressA() {
        UseVirtualKeyboard(); 
        keysPressed.Add(Keys.A);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void PressS() {
        UseVirtualKeyboard(); 
        keysPressed.Add(Keys.S);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void PressE() {
        UseVirtualKeyboard(); 
        keysPressed.Add(Keys.E);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void PressD() {
        UseVirtualKeyboard(); 
        keysPressed.Add(Keys.D);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void ReleaseW() {
        UseVirtualKeyboard(); 
        keysPressed.Remove(Keys.W); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void ReleaseA() {
        UseVirtualKeyboard(); 
        keysPressed.Remove(Keys.A); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void ReleaseS() {
        UseVirtualKeyboard(); 
        keysPressed.Remove(Keys.S); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void ReleaseD() {
        UseVirtualKeyboard(); 
        keysPressed.Remove(Keys.D); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void ReleaseE() {
        UseVirtualKeyboard(); 
        keysPressed.Remove(Keys.E); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static KeyboardState GetState() {
        if (useVirtualKeyboard) {
            return vks; 
        }
        return Keyboard.GetState(); 
    }

    public static bool IsVirtual() {
        return useVirtualKeyboard; 
    }

    public static void Reset() {
        useVirtualKeyboard = false; 
        keysPressed = new List<Keys>(); 
    }
}