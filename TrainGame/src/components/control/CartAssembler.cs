
namespace TrainGame.Components; 

using TrainGame.Constants; 
using TrainGame.Utils; 

public class CartAssembler : IAssembler {
    private City city; 
    private Machine m; 
    private CartType type; 

    public CartAssembler(City city, Machine m, CartType type = CartType.Freight) {
        this.city = city; 
        this.m = m; 
        this.type = type; 
    }

    public void Assemble() {
        string id = ID.GetNext($"{type}Cart");
        Cart cart = new Cart(id, type);
        city.AddCart(cart); 
    }       

    public Machine GetMachine() {
        return m; 
    }

    public City GetCity() {
        return city; 
    }

}

public enum CartType {
    Liquid,
    Freight
}