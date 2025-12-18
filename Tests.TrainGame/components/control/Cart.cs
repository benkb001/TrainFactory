
using TrainGame.Components; 

public class CartTest {
    [Fact]
    public void Cart_ShouldRespectConstructors() {
        Cart c = new Cart("C1", 2, 3, mass: 10f);
        Assert.Equal("C1", c.Id); 
        Assert.Equal(10f, c.Mass); 
    }
}