using Color = Microsoft.Xna.Framework.Color; 

namespace TrainGame.Components;
public class Outline {
    private int thickness; 
    private Color color; 

    public Outline() {
        this.thickness = 1; 
        this.color = Color.White; 
    }
    
    public Outline(int thickness, Color color) {
        this.thickness = thickness; 
        this.color = color; 
    }

    public int GetThickness() {
        return thickness; 
    }

    public Color GetColor() {
        return color; 
    }

    public void SetColor(Color c) {
        color = c; 
    }
}