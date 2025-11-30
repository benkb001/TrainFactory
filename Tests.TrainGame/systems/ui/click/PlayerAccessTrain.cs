
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 

public class PlayerAccessTrainClickSystemTest {
    private static Inventory inv = new Inventory("Test", 1, 1); 
    private static City c1 = new City("C1", inv); 
    [Fact]
    public void PlayerAccessTrainClickSystem_ShouldSetCorrectTrainHasPlayerValue() {
        World w = new World(); 
        RegisterComponents.All(w); 
        PlayerAccessTrainClickSystem.Register(w); 

        int e = EntityFactory.Add(w); 

        Train t = new Train(inv, c1);

        Assert.False(t.HasPlayer); 

        PlayerAccessTrainButton acBtn = new PlayerAccessTrainButton(t); 
        w.SetComponent<PlayerAccessTrainButton>(e, acBtn); 
        w.SetComponent<Button>(e, new Button(true)); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 100, 100)); 
        w.Update(); 

        Assert.True(t.HasPlayer); 
    }
}