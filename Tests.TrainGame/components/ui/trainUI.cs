
using TrainGame.Components; 

public class TrainUITest() {
    [Fact]
    public void CityUI_ShouldRespectConstructorArguments() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv); 
        Train t = new Train(origin: c, Inv: inv); 
        TrainUI tUI = new TrainUI(t); 
        
        Assert.Equal(t, tUI.GetTrain());
    }
}