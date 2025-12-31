namespace TrainGame.Components; 

public class TrainYard {
    private static TrainYard inst; 
    public static TrainYard Get() {
        if (inst is null) {
            inst = new TrainYard(); 
        }
        return inst; 
    }
}