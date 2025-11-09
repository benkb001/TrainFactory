
using TrainGame.Components; 

public class CityUITest() {
    [Fact]
    public void CityUI_ShouldRespectConstructorArguments() {
        City c = new City("Test", new Inventory("Test", 1, 1)); 
        CityUI cUI = new CityUI(c); 
        Assert.Equal(c, cUI.GetCity());
    }
}