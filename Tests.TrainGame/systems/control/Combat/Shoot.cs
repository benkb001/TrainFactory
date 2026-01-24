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
        Inventory.Item i = new Inventory.Item(ItemId: Weapons.GunMap.ToList()[0].Key, Count: 1); 
        int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10); 
        (int _, Inventory inv) = InventoryWrap.Add(w, "test", 1, 1); 
        inv.Add(i); 
        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 100);

        //dont love it but must for it to work, could try to decouple 
        //heldItem and inventory a little, or heldItem and shooting a little, 
        //but they are kinda naturally coupled 
       
        w.SetComponent<HeldItem>(e, new HeldItem(inv, invView.GetInventoryEntity())); 

        w.Update();
        VirtualMouse.SetCoordinates(new Vector2(1, 1)); 
        VirtualMouse.LeftPress();
        w.Update();

        Assert.Equal("Gun", w.GetComponent<HeldItem>(e).ItemId); 

        List<int> bulletEnts = w.GetMatchingEntities([typeof(Bullet), typeof(Velocity), typeof(Active)]);
        Assert.Single(bulletEnts); 

        VirtualMouse.Reset(); 
    }
}