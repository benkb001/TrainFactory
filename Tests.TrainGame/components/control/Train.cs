
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.Utils; 

public class TrainTest {
    private static Inventory inv = new Inventory("Test", 1, 1); 
    private static City c1 = new City("C1", inv); 
    private static City c2 = new City("C2", inv); 

    [Fact]
    public void Train_ShouldNotBeTravelingBeforeEmbarking() {
        Train t = new Train(inv, c1); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeTravelingAfterEmbarking() {
        Train t = new Train(inv, c1); 
        t.Embark(c2, new WorldTime()); 
        Assert.True(t.IsTraveling()); 
    }

    [Fact]
    public void Train_ShouldBeGoingToTheCityItEmbarkedTowards() {
        Train t = new Train(inv, c1); 
        t.Embark(c2, new WorldTime()); 
        Assert.Equal(c2, t.GoingTo); 
    }

    [Fact]
    public void Train_ShouldNotBeAbleToEmbarkTowardsTheSameCityItIsComingFrom() {
        Train t = new Train(inv, c1); 
        Assert.Throws<InvalidOperationException>(() => {
            t.Embark(c1, new WorldTime()); 
        }); 
    }

    [Fact]
    public void Train_ShouldBeArrivingAsSoonAsItHasTraveledEnoughDistance() {
        City c_start = new City("Start", inv, realX: 10f, realY: 0f); 
        City c_end = new City("End", inv, realX: 20f, realY: 0f); 
        Train t = new Train(inv, c_start, milesPerHour: 10f); 
        t.Embark(c2, new WorldTime()); 
        Assert.False(t.IsArriving(new WorldTime(hours: 0, minutes: 59))); 
        Assert.True(t.IsArriving(new WorldTime(hours: 1)));
    }

    [Fact]
    public void Train_ShouldBeComingFromTheCityItEmbarkedTowardsAfterArriving() {
        Train t = new Train(inv, c1, milesPerHour: 100f);
        t.Embark(c2, new WorldTime()); 
        t.Update(new WorldTime(hours: 1)); 
        Assert.Equal(c2, t.ComingFrom); 
    }

    [Fact]
    public void Train_ShouldNotBeTravelingAfterArriving() {
        Train t = new Train(inv, c1, milesPerHour: 100f); 
        t.Embark(c2, new WorldTime()); 
        //distance between c1 and c2 should be zero
        t.Update(new WorldTime(hours: 1)); 
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
        Train t = new Train(inv, c_start, milesPerHour: 10f); 
        t.Embark(c_end, new WorldTime()); 
        Assert.Equal(new Vector2(500f, 0f), t.GetMapPosition(new WorldTime(hours: 5))); 
    }

}