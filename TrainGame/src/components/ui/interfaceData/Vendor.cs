namespace TrainGame.Components;

using TrainGame.Systems;

public class VendorInterfaceData : IInterfaceData {
    private City city;
    private string vendorID; 

    public string VendorID => vendorID; 
    public Inventory Source; 
    public Inventory Destination;
    
    public VendorInterfaceData(string vendorID, Inventory Source, Inventory Destination, City city) {
        this.vendorID = vendorID; 
        this.Source = Source;
        this.Destination = Destination; 
        this.city = city;
    }

    public SceneType GetSceneType() {
        return SceneType.VendorInterface; 
    }

    public Menu GetMenu() {
        return new Menu(city: city); 
    }
}