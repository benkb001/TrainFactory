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