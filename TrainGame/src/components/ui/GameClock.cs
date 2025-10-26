namespace TrainGame.Components;

public class GameClockView {
    private static GameClockView inst; 
    public static GameClockView Get() {
        if (inst == null) {
            inst = new GameClockView();
        } 
        return inst; 
    }
}