using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class UtilsTest {
    [Fact]
    public void RectangleFromRectangleF_ShouldTruncateValuesToNearestInteger() {
        RectangleF rf = new RectangleF(14.0f, 14.5f, 14.9f, 14.1f); 
        Rectangle r = Util.RectangleFromRectangleF(rf); 
        Assert.Equal(14, r.X); 
        Assert.Equal(14, r.Y); 
        Assert.Equal(14, r.Width); 
        Assert.Equal(14, r.Height); 
    }

    [Fact]
    public void FloatEqual_ShouldReturnTrueForSmallDifferences() {
        float f1 = 1f; 
        float f2 = 1.0001f; 
        Assert.True(Util.FloatEqual(f1, f2)); 
    }

    [Fact]
    public void FloatEqual_ShouldReturnFalseForLargeDifferences() {
        float f1 = 1f; 
        float f2 = 1.1f; 
        Assert.False(Util.FloatEqual(f1, f2)); 
    }
}