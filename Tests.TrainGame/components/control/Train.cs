
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class TrainTest {

    private static (Inventory, Train) init() {
        Inventory inv = new Inventory("Test", 1, 1); 
        Dictionary<CartType, Inventory> carts = CartWrap.GetTestInventories();
        Train t = new Train(inv, Vector2.Zero, carts, "TestTrain", 0f, 1f, 1f, 1000f);
        return (inv, t);
    }


    [Fact]
    public void Train_ShouldNotBeTravelingBeforeEmbarking() {
        (Inventory inv, Train t) = init();
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeTravelingAfterEmbarking() {
        (Inventory inv, Train t) = init();
        t.Embark(new Vector2(1, 1), new WorldTime()); 
        Assert.True(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeGoingTowardsWhereItEmbarked() {
        (Inventory inv, Train t) = init();
        Vector2 dest = new Vector2(10, 10);
        t.Embark(dest, new WorldTime()); 
        Assert.Equal(dest, t.Destination); 
    }

    [Fact]
    public void Train_ShouldBeArrivingAsSoonAsItHasTraveledEnoughDistance() {
        (Inventory _, Train t) = init();
        Vector2 dest = new Vector2(1, 0);
        t.Embark(dest, new WorldTime()); 
        Assert.False(t.IsArriving()); 

        t.Move(new WorldTime(hours: 1));
        Assert.True(t.IsArriving());
    }

    [Fact]
    public void Train_ShouldBeAtThePositionItEmbarkedTowardsAfterArriving() {
        (Inventory inv, Train t) = init();
        t.Embark(new Vector2(1, 0), new WorldTime()); 
        t.Move(new WorldTime(hours: 1));
        t.Update(); 
        Assert.Equal(new Vector2(1, 0), t.Position); 
    }

    [Fact]
    public void Train_ShouldNotBeTravelingAfterArriving() {
        (Inventory inv, Train t) = init();
        t.Embark(new Vector2(1, 0), new WorldTime()); 
        t.Move(new WorldTime(hours: 1));
        t.Update(); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldSetMPHBasedOnPowerAndMass() {
        (Inventory inv, Train _) = init();
        Dictionary<CartType, Inventory> carts = new(){
            [CartType.Freight] = inv
        };
        Train t = new Train(inv, Vector2.Zero, carts, "TestTrain", power: 100f, mass: 10f); 
        Assert.Equal(10f, t.MilesPerHour);
        t.UpgradePower(900f); 
        Assert.Equal(100f, t.MilesPerHour); 
        t.UpgradeInventory(); 
        Assert.Equal(1000f / (10f + Constants.InvUpgradeMass), t.MilesPerHour);
        t.AddCart(CartType.Freight); 

        Assert.Equal(1000f / (10f + Constants.CartMass[CartType.Freight] + Constants.InvUpgradeMass), t.MilesPerHour); 
    }

    [Fact]
    public void Train_AddCartShouldAddCart() {
        (Inventory inv, Train t) = init();
        int prevLevel = t.Carts[CartType.Freight].Level;
        t.AddCart(CartType.Freight); 
        Assert.Equal(prevLevel + 1, t.Carts[CartType.Freight].Level); 
    }

    [Fact]
    public void Train_UpgradeInventoryShouldIncrementInvLevelAndMass() {
        (Inventory inv, Train _) = init();
        Train t = new Train(inv, Vector2.Zero, new Dictionary<CartType, Inventory>(), "TestTrain", power: 100f, mass: 10f); 
        int defaultInvLevel = t.Inv.Level; 
        t.UpgradeInventory(); 
        Assert.Equal(10f + Constants.InvUpgradeMass, t.Mass); 
        Assert.Equal(defaultInvLevel + 1, t.Inv.Level); 
    }

}