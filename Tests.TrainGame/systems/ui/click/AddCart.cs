using TrainGame.Systems; 
using TrainGame.ECS; 
using TrainGame.Components; 

public class AddCartClickSystemTest {
    [Fact]
    public void AddCartClickSystem_ShouldAddCartToTrainAndRemoveFromCityWhenClicked() {
        World w = WorldFactory.Build(); 
    
        Inventory inv = new Inventory("T", 1, 1); 
        City city = new City("T", inv); 
        Train t = new Train(inv, city); 
        Cart cart = new Cart(CartType.Freight); 
        city.AddCart(cart); 

        int btnEntity = EntityFactory.Add(w); 
        w.SetComponent<Button>(btnEntity, new Button(true)); 
        w.SetComponent<AddCartButton>(btnEntity, new AddCartButton(t, city, cart)); 

        w.Update(); 

        Assert.Empty(city.Carts); 
    }
}