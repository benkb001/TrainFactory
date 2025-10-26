using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class VelocityTest {

    [Fact]
    public void Velocity_ShouldRespectConstructorArguments() {
        float dx = 1.5f; 
        float dy = 2.5f; 

        Velocity v = new Velocity(dx, dy); 

        Assert.Equal(dx, v.Vector.X); 
        Assert.Equal(dy, v.Vector.Y); 
    }

    [Fact]
    public void Velocity_ShouldDefaultToZero() {
        Velocity v = new Velocity(); 

        Assert.Equal(Vector2.Zero, v.Vector); 
    }

    [Fact] 
    public void Velocity_ShouldRespectChanges() {
        float dx = 1.5f; 
        float dy = 1.5f; 

        Velocity v = new Velocity(); 
        v.Vector = new Vector2(dx, dy); 
        Assert.Equal(dx, v.Vector.X); 
        Assert.Equal(dy, v.Vector.Y); 
    }
}