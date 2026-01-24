namespace TrainGame.Utils; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//TODO: Fill out tests 
public static class VirtualKeyboard {
    private static bool useVirtualKeyboard = false; 
    private static KeyboardState vks = new KeyboardState([]); 
    private static KeyboardState ks_prev = new KeyboardState([]); 
    private static List<Keys> keysPressed = new List<Keys>(); 
    
    public static void UseVirtualKeyboard() {
        useVirtualKeyboard = true; 
    }

    public static void Click(Keys k) {
        keysPressed.Remove(k); 
        ks_prev = new KeyboardState(keysPressed.ToArray()); 
        keysPressed.Add(k); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void Press(Keys k) {
        UseVirtualKeyboard(); 
        keysPressed.Add(k);
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static void Release(Keys k) {
        UseVirtualKeyboard(); 
        keysPressed.Remove(k); 
        vks = new KeyboardState(keysPressed.ToArray()); 
    }

    public static bool IsClicked(Keys k) {
        KeyboardState ks = GetState(); 
        return !ks_prev.IsKeyDown(k) && ks.IsKeyDown(k); 
    }

    public static bool IsPressed(Keys k) {
        return GetState().IsKeyDown(k); 
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
        vks = new KeyboardState([]); 
        ks_prev = new KeyboardState([]); 
    }

    public static void UpdatePrevFrame() {
        if (useVirtualKeyboard) {
            ks_prev = vks; 
        } else {
            ks_prev = Keyboard.GetState(); 
        }
    }
}