namespace TrainGame.Components;

using TrainGame.Systems;

public class VendorInterfaceData : IInterfaceData {
    private City city; 
    private string vendorID; 

    public string VendorID => vendorID; 
    public City GetCity() => city; 
    
    public VendorInterfaceData(City city, string vendorID) {
        this.city = city; 
        this.vendorID = vendorID; 
    }

    public SceneType GetSceneType() {
        return SceneType.VendorInterface; 
    }

    public Menu GetMenu() {
        return new Menu(city: city); 
    }
}