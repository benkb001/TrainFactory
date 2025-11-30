
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components;

public class CameraReturnTest {
    [Fact]
    public void CameraReturn_ShouldRespectConstructorArguments() {
        CameraReturn cr = new CameraReturn(new Vector2(3, 4), 0.5f); 
        Assert.Equal(new Vector2(3, 4), cr.Position); 
        Assert.Equal(0.5f, cr.Zoom); 
    }
}