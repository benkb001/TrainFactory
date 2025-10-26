namespace TrainGame.Utils; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//TODO: Update tests, some of the functions here aren't tested yet
//TODO: Update clicking so that it is more dynamic, will have to store an array of 
//previous mouse states and check if any within a certain number of previous frames 
//was a click, and that the current one is a release
public static class VirtualMouse {
    private static bool useVirtualMouse = false; 
    
    private static MouseState prev_mouse = new MouseState(0, 0, 0, 
        ButtonState.Released, ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released);

    private static MouseState vm = new MouseState(0, 0, 0, 
        ButtonState.Released, ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released); 

    public static void Reset() {
        useVirtualMouse = false; 
        vm = new MouseState(0, 0, 0, 
        ButtonState.Released, ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released); 
        prev_mouse = new MouseState(0, 0, 0, 
        ButtonState.Released, ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released); 
    }

    public static void UseVirtualMouse() {
        useVirtualMouse = true;
    }

    public static void UsePhysicalMouse() {
        useVirtualMouse = false; 
    }

    public static MouseState GetState() {
        if (useVirtualMouse) {
            return vm; 
        } else {
            return Mouse.GetState(); 
        }
    }

    public static Vector2 GetCoordinates() {
        MouseState ms = GetState(); 
        return new Vector2(ms.X, ms.Y); 
    }

    public static void SetCoordinates(int x, int y) {
        UseVirtualMouse();
        vm = new MouseState(x, y, vm.ScrollWheelValue,
            vm.LeftButton, vm.MiddleButton, 
            vm.RightButton, vm.XButton1, 
            vm.XButton2, vm.HorizontalScrollWheelValue); 
    }

    public static void LeftClick() {
        UseVirtualMouse();
        vm = new MouseState(vm.X, vm.Y, vm.ScrollWheelValue,
            ButtonState.Pressed, vm.MiddleButton, 
            vm.RightButton, vm.XButton1, 
            vm.XButton2, vm.HorizontalScrollWheelValue); 
    }

    public static void RightClick() {
        UseVirtualMouse();
        vm = new MouseState(vm.X, vm.Y, vm.ScrollWheelValue,
            vm.LeftButton, vm.MiddleButton, 
            ButtonState.Pressed, vm.XButton1, 
            vm.XButton2, vm.HorizontalScrollWheelValue); 
    }

    public static void LeftRelease() {
        UseVirtualMouse();
        vm = new MouseState(vm.X, vm.Y, vm.ScrollWheelValue,
            ButtonState.Released, vm.MiddleButton, 
            vm.RightButton, vm.XButton1, 
            vm.XButton2, vm.HorizontalScrollWheelValue); 
    }

    public static void RightRelease() {
        UseVirtualMouse();
        vm = new MouseState(vm.X, vm.Y, vm.ScrollWheelValue,
            vm.LeftButton, vm.MiddleButton, 
            ButtonState.Released, vm.XButton1, 
            vm.XButton2, vm.HorizontalScrollWheelValue); 
    }
    
    //update tests 
    public static bool LeftPressed() {
        return GetState().LeftButton == ButtonState.Pressed; 
    }

    public static bool RightPressed() {
        return GetState().RightButton == ButtonState.Pressed; 
    }

    public static bool LeftClicked() {
        return prev_mouse.LeftButton == ButtonState.Released && 
                GetState().LeftButton == ButtonState.Pressed; 
    }

    public static bool RightClicked() {
        return prev_mouse.RightButton == ButtonState.Released && 
                GetState().RightButton == ButtonState.Pressed; 
    }

    public static bool IsVirtual() {
        return useVirtualMouse; 
    }

    public static void UpdatePrevFrame() {
        if (useVirtualMouse) {
            prev_mouse = vm; 
        } else {
            prev_mouse = Mouse.GetState(); 
        }
    }
}