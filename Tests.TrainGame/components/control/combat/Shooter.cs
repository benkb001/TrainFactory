namespace TrainGame.Components; 

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

public class ShooterTest {
    [Fact]
    public void Shooter_UpdateShouldDecrementAmmo() {
        Shooter s = new Shooter(ammo: 2); 
        s.Update(new WorldTime(), 1); 
        Assert.Equal(1, s.Ammo);
    }
}