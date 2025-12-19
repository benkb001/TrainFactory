
using TrainGame.Components; 

using TrainGame.Constants; 

public class CartTest {
    [Fact]
    public void Cart_ShouldRespectConstructors() {
        Cart c = new Cart("C1", CartType.Freight);
        Assert.Equal("C1", c.Id); 
        Assert.Equal(Constants.FreightCartBaseMass, c.Mass); 
    }
}