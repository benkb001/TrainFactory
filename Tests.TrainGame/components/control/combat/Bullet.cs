
using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class BulletTest {
    [Fact]
    public void Bullet_ShouldRemoveShouldReturnTrueIfDecayedMoreThanMaxFramesActive() {
        Bullet b = new Bullet(1, maxFramesActive: 1); 
        //0 frames, fine
        Assert.False(b.ShouldRemove); 
        b.Decay();
        //1 frame, also fine
        Assert.False(b.ShouldRemove);
        //2 frames, more than max, remove
        b.Decay();
        Assert.True(b.ShouldRemove);
    }
}