namespace TrainGame.Components; 
using TrainGame.ECS;

public static class CityWrap {
    public static City GetTest() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("test", inv);
        return c; 
    }
}