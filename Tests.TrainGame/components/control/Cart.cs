
using TrainGame.Components; 

using TrainGame.Constants; 

public class CartTest {
    [Fact]
    public void Cart_ShouldRespectConstructors() {
        Cart c = new Cart(CartType.Freight);
        Assert.Equal(CartType.Freight, c.Type); 
    }
}