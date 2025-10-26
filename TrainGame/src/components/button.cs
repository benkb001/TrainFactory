namespace TrainGame.Components;

public class Button {
    public bool Clicked = false; 
    public int Depth = 0; 

    public Button(bool b = false, int d = 0) {
        Clicked = b; 
        Depth = d; 
    }
}