
using TrainGame.Components; 

public class EmbarkButtonTest {
    [Fact]
    public void EmbarkButton_ShouldRespectConstructors() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv); 
        Train t = new Train(inv, c); 
        EmbarkButton eb = new EmbarkButton(c, t); 
        Assert.Equal(t, eb.GetTrain()); 
        Assert.Equal(c, eb.GetDestination()); 
    }
}