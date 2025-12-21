using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class DrawAddCartInterfaceSystemTest {
    [Fact]
    public void DrawAddCartInterfaceSystem_ShouldDrawAddCartButtons() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2); 
        City city = new City("Test", inv); 
        Train train = new Train(inv, city); 

        city.AddCart(new Cart("Test", CartType.Freight)); 
        int btn = w.AddEntity(); 
        w.SetComponent<Button>(btn, new Button(true)); 
        w.SetComponent<AddCartInterfaceButton>(btn, new AddCartInterfaceButton(
            train, city
        )); 
        w.SetComponent<Active>(btn, Active.Get()); 

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(AddCartButton)]));
    }
}