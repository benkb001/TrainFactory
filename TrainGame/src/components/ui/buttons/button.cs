namespace TrainGame.Components;

public class Button {
    public bool Clicked = false; 
    public float Depth = 0f; 

    public Button(bool Clicked = false, float Depth = 0f) {
        this.Clicked = Clicked; 
        this.Depth = Depth; 
    }
}