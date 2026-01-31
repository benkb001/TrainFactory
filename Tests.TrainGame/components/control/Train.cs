
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
    private static Inventory inv = new Inventory("Test", 1, 1); 
    private static (Inventory, City, City) init() {
        City c1 = new City("C1", inv); 
        City c2 = new City("C2", inv); 
        c1.AddConnection(c2);
        return (inv, c1, c2);
    }


    [Fact]
    public void Train_ShouldNotBeTravelingBeforeEmbarking() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeTravelingAfterEmbarking() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1); 
        t.Embark(c2, new WorldTime()); 
        Assert.True(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeGoingToTheCityItEmbarkedTowards() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1); 
        t.Embark(c2, new WorldTime()); 
        Assert.Equal(c2, t.GoingTo); 
    }

    [Fact]
    public void Train_ShouldNotBeAbleToEmbarkTowardsTheSameCityItIsComingFrom() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1); 
        Assert.Throws<InvalidOperationException>(() => {
            t.Embark(c1, new WorldTime()); 
        }); 
    }

    [Fact]
    public void Train_ShouldBeArrivingAsSoonAsItHasTraveledEnoughDistance() {
        City c_start = new City("Start", inv, realX: 10f, realY: 0f); 
        City c_end = new City("End", inv, realX: 20f, realY: 0f); 
        c_start.AddConnection(c_end);
        Train t = new Train(inv, c_start, milesPerHour: 10f); 
        t.Embark(c_end, new WorldTime()); 
        Assert.False(t.IsArriving()); 

        t.Move(new WorldTime(hours: 1));
        Assert.True(t.IsArriving());
    }

    [Fact]
    public void Train_ShouldBeComingFromTheCityItEmbarkedTowardsAfterArriving() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1, milesPerHour: 100f);
        t.Embark(c2, new WorldTime()); 

        t.Move(new WorldTime(hours: 1));
        t.Update(); 
        Assert.Equal(c2, t.ComingFrom); 
    }

    [Fact]
    public void Train_ShouldNotBeTravelingAfterArriving() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1, milesPerHour: 100f); 
        t.Embark(c2, new WorldTime()); 
        
        t.Move(new WorldTime(hours: 1));
        t.Update(); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldPositionItselfOnTheMapBasedOnTheProportionOfJourneyCompleted() {
        //realX = 0, uiX = 0
        //realX = 100, uiX = 1000 
        //mph = 10 
        //so after 5 hours, it should be at (500, 0)
        City c_start = new City("Start", inv, uiX: 0f, uiY: 0f, realX: 0f, realY: 0f); 
        City c_end = new City("End", inv, uiX: 1000f, uiY: 0f, realX: 100f, realY: 0f); 
        c_start.AddConnection(c_end);
        Train t = new Train(inv, c_start, milesPerHour: 10f); 
        t.Embark(c_end, new WorldTime()); 
        t.Move(new WorldTime(hours: 5));
        Assert.Equal(new Vector2(500f, 0f), t.GetMapPosition()); 
    }

    [Fact]
    public void Train_ShouldSetMPHBasedOnPowerAndMass() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1, power: 100f, mass: 10f); 
        Assert.Equal(10f, t.MilesPerHour);
        t.UpgradePower(900f); 
        Assert.Equal(100f, t.MilesPerHour); 
        t.UpgradeInventory(); 
        Assert.Equal(1000f / (10f + Constants.InvUpgradeMass), t.MilesPerHour);
        Cart c = new Cart(CartType.Freight); 
        t.AddCart(c); 

        Assert.Equal(1000f / (10f + Constants.CartMass[CartType.Freight] + Constants.InvUpgradeMass), t.MilesPerHour); 
    }

    [Fact]
    public void Train_AddCartShouldAddCart() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1, power: 100f, mass: 10f); 
        Cart c = new Cart(CartType.Freight); 
        t.AddCart(c); 
        Assert.Equal(1, t.Carts[CartType.Freight].Level); 
    }

    [Fact]
    public void Train_UpgradeInventoryShouldIncrementInvLevelAndMass() {
        (Inventory inv, City c1, City c2) = init();
        Train t = new Train(inv, c1, power: 100f, mass: 10f); 
        int defaultInvLevel = t.Inv.Level; 
        t.UpgradeInventory(); 
        Assert.Equal(10f + Constants.InvUpgradeMass, t.Mass); 
        Assert.Equal(defaultInvLevel + 1, t.Inv.Level); 
    }

}