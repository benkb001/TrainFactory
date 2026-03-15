namespace TrainGame.Components; 

public class DrawCityMessage {
    private City city; 

    public DrawCityMessage(City city) {
        this.city = city; 
    }

    public City GetCity() {
        return city; 
    }
}