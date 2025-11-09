namespace TrainGame.Components;

public class Button {
    public bool Clicked = false; 
    public int Depth = 0; 

    public Button(bool Clicked = false, int Depth = 0) {
        this.Clicked = Clicked; 
        this.Depth = Depth; 
    }
}