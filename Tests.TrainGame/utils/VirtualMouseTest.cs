using TrainGame.Utils; 

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//sequential because global state (mouse) doesnt play nice with parallel 
[Collection("Sequential")]
public class VirtualMouseTest
{
    [Fact]
    public void VirtualMouse_ShouldReturnDefaultMouseIfNotSetToVirtual() {
        VirtualMouse.Reset(); 
        MouseState m = VirtualMouse.GetState(); 
        Assert.Equal(0, m.X); 
        Assert.Equal(0, m.Y); 
    }

    [Fact] 
    void VirtualMouse_ShouldBeAbleToSpecifyCoordinatesOfMouse() {
        VirtualMouse.Reset(); 
        VirtualMouse.UseVirtualMouse(); 
        MouseState prev = VirtualMouse.GetState(); 
        Assert.Equal(0, prev.X); 
        Assert.Equal(0, prev.Y); 

        VirtualMouse.SetCoordinates(100, 100); 
        MouseState m = VirtualMouse.GetState(); 

        Assert.Equal(100, m.X); 
        Assert.Equal(100, m.Y); 
    }

    [Fact]
    void VirtualMouse_ShouldBeAbleToClick() {
        VirtualMouse.Reset(); 

        Assert.False(VirtualMouse.LeftClicked()); 
        VirtualMouse.LeftClick(); 
        Assert.True(VirtualMouse.LeftClicked()); 
    }

    [Fact]
    void VirtualMouse_ShouldBeAbleToRelease() {
        VirtualMouse.Reset(); 
        VirtualMouse.LeftClick(); 
        VirtualMouse.RightClick(); 
        VirtualMouse.LeftRelease(); 
        VirtualMouse.RightRelease(); 

        MouseState m = VirtualMouse.GetState(); 

        Assert.Equal(ButtonState.Released, m.LeftButton); 
        Assert.Equal(ButtonState.Released, m.RightButton); 
    }
}