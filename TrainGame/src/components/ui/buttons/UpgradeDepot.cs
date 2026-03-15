namespace TrainGame.Components;

public class UpgradeDepotButton {
    private City city; 
    public City GetCity() => city; 

    public UpgradeDepotButton(City city) {
        this.city = city; 
    }
}