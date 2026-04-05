using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants;
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Callbacks; 

//sequential because global state (keyboard)
[Collection("Sequential")]
public class ShootSystemTest {
    [Fact]
    public void ShootSystem_ShouldShootWhenPlayerLeftPressedAndHoldingGun() {
        VirtualMouse.Reset(); 

        World w = WorldFactory.Build(); 
        int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10); 
        (int _, Inventory inv) = InventoryWrap.Add(w, "test", 1, 1); 
        w.SetComponent<Shooter>(e, new Shooter()); 
        w.SetComponent<DefaultShootPattern>(e, new DefaultShootPattern(new BulletContainer(new Bullet(1))));
        w.SetComponent<Player>(e, new Player());
        //ICKY: Should we have a dedicated component to separate player cuz bullets can be shooters now 
        w.SetComponent<Health>(e, new Health(1));

        //dont love it but must for it to work, could try to decouple 
        //heldItem and inventory a little, or heldItem and shooting a little, 
        //but they are kinda naturally coupled 
        w.Update();
        VirtualMouse.SetCoordinates(new Vector2(1, 1)); 
        VirtualMouse.LeftPress();
        w.Update();

        List<int> bulletEnts = w.GetMatchingEntities([typeof(Bullet), typeof(Velocity), typeof(Active)]);
        Assert.Single(bulletEnts); 

        VirtualMouse.Reset(); 
    }
}