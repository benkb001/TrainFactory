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

        int btnEntity = EntityFactory.Add(w); 
        w.SetComponent<Button>(btnEntity, new Button(true)); 
        w.SetComponent<AddCartButton>(btnEntity, new AddCartButton(t, city, CartType.Freight)); 

        w.Update(); 

        Assert.Equal(1, t.Carts[CartType.Freight].Level); 
    }
}