namespace TrainGame.Components;

public class Collidable {
    private static Collidable inst; 
    public static Collidable Get() {
        if (inst == null) {
            inst = new Collidable();
        } 
        return inst; 
    }
}