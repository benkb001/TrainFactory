namespace TrainGame.Components;

public class ProgressBar {
    public float Completion; 
    public float MaxWidth; 

    public ProgressBar(float MaxWidth, float Completion = 0f) {
        this.MaxWidth = MaxWidth; 
        this.Completion = Completion;
    }
}