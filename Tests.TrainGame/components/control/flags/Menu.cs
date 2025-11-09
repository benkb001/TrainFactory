using System.Collections.Generic;
using System; 
using TrainGame.Components; 

public class MenuTest {
    [Fact]
    public void Menu_ShouldReturnAnInstance() {
        Assert.True(Menu.Get() is Menu);
    }

    [Fact]
    public void Menu_ShouldAlwaysReturnTheSameInstance() {
        Assert.Equal(Menu.Get(), Menu.Get()); 
    }
}