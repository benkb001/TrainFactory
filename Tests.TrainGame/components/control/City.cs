using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
public class CityTest {
    [Fact]
    public void City_ShouldRespectConstructorArguments() {
        City c = new City("Test", new Inventory("Test", 1, 1), 100f, 150f);
        Assert.Equal("Test", c.Inv.GetId()); 
        Assert.Equal(100f, c.UiX);
        Assert.Equal(150, c.UiY); 
    }

    [Fact]
    public void City_ShouldRememberWhichCitiesItIsAdjacentTo() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv, 100f, 150f);

        City c1 = new City("C1", inv, 0f, 0f); 
        City c2 = new City("C2", inv , 0f, 0f);
        City c3 = new City("C3", inv, 0f, 0f); 
        City c4 = new City("Disjoint", inv, 0f, 0f); 

        c.AddConnection(c1); 
        c.AddConnections([c2, c3]); 

        List<City> cs = c.AdjacentCities; 
        Assert.Contains(cs, city => city.CityId == "C1"); 
        Assert.Contains(cs, city => city.CityId == "C2"); 
        Assert.Contains(cs, city => city.CityId == "C3"); 
        Assert.DoesNotContain(cs, city => city.CityId == "C4");  
    }
}