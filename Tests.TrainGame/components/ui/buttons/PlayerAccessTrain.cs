
using TrainGame.Components; 

public class PlayerAccessTrainButtonTest {
    private static Inventory inv = new Inventory("Test", 1, 1); 
    private static City c1 = new City("C1", inv); 
    [Fact]
    public void PlayerAccessTrain_ShouldDisplayCorrectMessage() {
        Train t = new Train(inv, c1); 
        PlayerAccessTrainButton acBtn = new PlayerAccessTrainButton(t); 
        Assert.Contains("enter", acBtn.GetMessage());
        t.HasPlayer = true; 
        Assert.Contains("exit", acBtn.GetMessage()); 
    }
}