namespace TrainGame.Components;

public class MapUIFlag {
    private static MapUIFlag inst; 
    public static MapUIFlag Get() {
        if (inst is null) {
            inst = new MapUIFlag(); 
        }
        return inst; 
    }
}