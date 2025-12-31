namespace TrainGame.Components;

public class PauseButton {
    private static PauseButton inst; 
    public static PauseButton Get() {
        if (inst is null) {
            inst = new PauseButton(); 
        }
        return inst; 
    }
}

public class UnpauseButton {
    private static UnpauseButton inst; 
    public static UnpauseButton Get() {
        if (inst is null) {
            inst = new UnpauseButton(); 
        }
        return inst; 
    }
}

public class SpeedTimeButton {
    private static SpeedTimeButton inst; 
    public static SpeedTimeButton Get() {
        if (inst is null) {
            inst = new SpeedTimeButton(); 
        }
        return inst; 
    }
}
public class SlowTimeButton {
    private static SlowTimeButton inst; 
    public static SlowTimeButton Get() {
        if (inst is null) {
            inst = new SlowTimeButton(); 
        }
        return inst; 
    }
}