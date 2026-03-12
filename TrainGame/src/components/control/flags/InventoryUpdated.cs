namespace TrainGame.Components; 

public class InventoryUpdatedFlag {

    private static InventoryUpdatedFlag inst; 
    public static InventoryUpdatedFlag Get() {
        if (inst == null) {
            inst = new InventoryUpdatedFlag();
        } 
        return inst; 
    }
}