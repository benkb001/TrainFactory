namespace TrainGame.Components;

public class Interactor {
    private static Interactor inst; 
    public static Interactor Get() {
        if (inst is null) {
            inst = new Interactor(); 
        }
        return inst; 
    }
}