namespace TrainGame.Components;

public class Menu {
    private static Menu inst; 
    public static Menu Get() {
        if (inst is null) {
            inst = new Menu(); 
        }
        return inst; 
    }
}