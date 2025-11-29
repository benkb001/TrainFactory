namespace TrainGame.Components;

public class DrawMapMessage {
    private static DrawMapMessage inst; 
    public static DrawMapMessage Get() {
        if (inst is null) {
            inst = new DrawMapMessage(); 
        }
        return inst; 
    }
}