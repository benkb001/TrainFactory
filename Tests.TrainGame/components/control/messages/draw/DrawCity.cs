
using TrainGame.Components; 

public class DrawCityMessageTest {
    [Fact]
    public void DrawCityMessage_ShouldRespectConstructors() {
        City c = new City("Test", new Inventory("Test", 1, 1), 100f, 150f);
        DrawCityMessage dm = new DrawCityMessage(c); 
        Assert.Equal(c, dm.GetCity());
    }
}