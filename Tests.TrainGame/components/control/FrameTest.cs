using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class FrameTest {
    [Fact]
    public void Frame_ShouldRespectConstructorParameters() {
        int x = 10; 
        int y = 10; 
        int width = 20; 
        int height = 5; 
        float rotation = 1.5f; 
        Frame s = new Frame(x, y, width, height, rotation); 

        Assert.Equal(x, s.GetX()); 
        Assert.Equal(y, s.GetY()); 
        Assert.Equal(height, s.GetHeight()); 
        Assert.Equal(width, s.GetWidth()); 
        Assert.Equal(rotation, s.GetRotation());
    }

    [Fact]
    public void Frame_ShouldProduceRectangleCorrespondingToCoordinatesAndSize() {
        int x = 10; 
        int y = 10; 
        int width = 20; 
        int height = 5; 
        Frame s = new Frame(x, y, width, height); 

        RectangleF r = s.GetRectangle(); 

        Assert.Equal(x + width, r.Right); 
        Assert.Equal(y + height, r.Bottom); 
    }

    [Fact]
    public void Frame_ShouldRespectCoordinateChanges() {
        int x = 10; 
        int y = 10; 
        int width = 20; 
        int height = 5; 
        Frame s = new Frame(x, y, width, height); 

        Assert.Equal(x, s.GetX()); 
        Assert.Equal(y, s.GetY()); 
        s.SetCoordinates(20, 20); 
        Assert.Equal(20, s.GetX()); 
        Assert.Equal(20, s.GetY()); 
    }

    [Fact]
    public void Frame_ShouldCorrectlyReportIfTouching() {
        Frame f1 = new Frame(0, 0, 10, 10); 
        Frame f2 = new Frame(10, 0, 10, 10); 
        Assert.True(f1.IsTouching(f2)); 
        Frame f3 = new Frame(15, 0, 10, 10); 
        Assert.False(f1.IsTouching(f3));

        Frame f4 = new Frame(0, 20, 10, 10); 
        Assert.False(f1.IsTouching(f4)); 
    }
}