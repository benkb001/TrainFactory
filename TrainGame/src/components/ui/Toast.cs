namespace TrainGame.Components;

public class Toast {
    public float RemainingDuration = 1f; 
    
    public void DecrementDuration() {
        RemainingDuration -= 0.005f; 
    }
}