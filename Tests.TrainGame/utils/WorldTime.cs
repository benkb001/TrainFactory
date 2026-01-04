using TrainGame.Utils;

public class WorldTimeTest {
    [Fact]
    public void WorldTime_ConstructorShouldAddMinutesToHoursIfMinutesOverSixty() {
        WorldTime wt = new WorldTime(minutes: 60); 
        Assert.Equal(1, wt.Hours); 
        Assert.Equal(0, wt.Minutes); 
    }

    [Fact]
    public void WorldTime_ConstructorShouldPassFractionsOfAnHourToMinutes() {
        WorldTime wt = new WorldTime(hours: 0.5f);
        Assert.Equal(30, wt.Minutes); 
        Assert.Equal(0, wt.Hours); 
    }

    [Fact]
    public void WorldTime_UpdateShouldAddMinutesToHoursIfMinutesOverSixty() {
        WorldTime wt = new WorldTime(minutes: 59, ticks: 59); 
        Assert.Equal(0, wt.Hours); 
        wt.Update(); 
        Assert.Equal(1, wt.Hours); 
    }

    [Fact]
    public void WorldTime_IsAfterOrAtShouldWorkIfOneTickGreaterOrLess() {
        WorldTime before = new WorldTime(days: 1, hours: 1, minutes: 1, ticks: 1); 
        WorldTime after = new WorldTime(days: 1, hours: 1, minutes: 1, ticks: 2); 
        Assert.True(after.IsAfterOrAt(before)); 
        Assert.False(before.IsAfterOrAt(after)); 
    }
}