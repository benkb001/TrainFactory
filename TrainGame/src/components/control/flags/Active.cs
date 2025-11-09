namespace TrainGame.Components;

public class Active {
    private static Active inst; 
    public static Active Get() {
        if (inst is null) {
            inst = new Active(); 
        }
        return inst; 
    }
}