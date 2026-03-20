using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

public class DrawAddCartInterfaceSystemTest {
    [Fact]
    public void DrawAddCartInterfaceSystem_ShouldDrawAddCartButtons() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2); 
        City city = new City("Test", inv); 
        Train train = TrainWrap.Assemble(city);
        int trainEnt = TrainWrap.RegisterExisting(w, train, city);

        city.AddCart(CartType.Freight); 
        int btn = w.AddEntity(); 
        w.SetComponent<Button>(btn, new Button(true)); 
        w.SetComponent<AddCartInterfaceButton>(btn, new AddCartInterfaceButton(
            train, trainEnt, city
        )); 
        w.SetComponent<Active>(btn, Active.Get()); 

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(AddCartButton)]));
    }
}