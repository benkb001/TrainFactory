namespace TrainGame.Components;

public class Toast {
    private int curTicks; 
    private int maxTicks; 

    public float RemainingDuration => (float)curTicks / maxTicks; 
    
    public Toast(int maxTicks = 100) {
        this.curTicks = maxTicks; 
        this.maxTicks = maxTicks;
    }

    public void DecrementDuration() {
        curTicks--; 
    }
}