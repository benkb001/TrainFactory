using TrainGame.Utils; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//sequential because global state (mouse) doesnt play nice with parallel 
[Collection("Sequential")]
public class VirtualKeyboardTest {
    [Fact]
    public void VirtualKeyboard_ShouldResetToNotUsingVirtual() {
        VirtualKeyboard.Reset(); 
        Assert.False(VirtualKeyboard.IsVirtual()); 
        VirtualKeyboard.Reset(); 
    }

    [Fact]
    public void VirtualKeyboard_UseVirtualShouldSetVirtualFlag() {
        VirtualKeyboard.Reset(); 
        VirtualKeyboard.UseVirtualKeyboard(); 
        Assert.True(VirtualKeyboard.IsVirtual()); 
        VirtualKeyboard.Reset(); 
    }

    [Fact]
    public void VirtualKeyboard_ShouldRespondToKeyPresses() {
        VirtualKeyboard.Reset(); 
        VirtualKeyboard.PressW(); 
        Assert.True(VirtualKeyboard.GetState().IsKeyDown(Keys.W));
        VirtualKeyboard.Reset(); 
    }
    
    [Fact]
    public void VirtualKeyboard_ShouldRespondToKeyReleases() {
        VirtualKeyboard.Reset(); 
        VirtualKeyboard.PressW(); 
        VirtualKeyboard.ReleaseW(); 
        Assert.False(VirtualKeyboard.GetState().IsKeyDown(Keys.W)); 
        VirtualKeyboard.Reset(); 
    }
}