namespace TrainGame.Components;

public class Data {
    private static Data inst; 
    public static Data Get() {
        if (inst == null) {
            inst = new Data();
        } 
        return inst; 
    }
}