namespace TrainGame.Components; 

public class CityUI {
    private City city; 

    public CityUI(City city) {
        this.city = city; 
    }

    public City GetCity() {
        return city; 
    }
}