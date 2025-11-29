namespace TrainGame.Components;

//todo: test
public class DrawBackgroundMessage {
    private static DrawBackgroundMessage inst; 
    public static DrawBackgroundMessage Get() {
        if (inst is null) {
            inst = new DrawBackgroundMessage(); 
        }
        return inst; 
    }
}