using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class AddCartInterfaceClickSystemTest {
    [Fact]
    public void AddCartInterfaceSystem_ShouldCreateADrawAddCartInterfaceDrawMessage() {
        World w = new World(); 
        RegisterComponents.All(w); 
        AddCartInterfaceClickSystem.Register(w);

        Inventory inv = new Inventory("Test", 2, 2); 
        City city = new City("Test", inv); 
        Train train = new Train(inv, city); 

        int btn = w.AddEntity(); 
        w.SetComponent<Button>(btn, new Button(true)); 
        w.SetComponent<AddCartInterfaceButton>(btn, new AddCartInterfaceButton(
            train, city
        )); 
        w.SetComponent<Active>(btn, Active.Get()); 

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(DrawAddCartInterfaceMessage)]));
    }
}