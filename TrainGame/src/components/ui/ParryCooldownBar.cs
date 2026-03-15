namespace TrainGame.Components;

using TrainGame.Utils;

public class ParryCooldownBar {
    private Parrier parrier; 

    public ParryCooldownBar(Parrier p) {
        parrier = p; 
    }

    public float Completion(WorldTime now) {
        return parrier.PercentCooldownComplete(now); 
    }
}