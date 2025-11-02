using Color = Microsoft.Xna.Framework.Color; 

namespace TrainGame.Components;
public class Outline {
    private int thickness; 
    private Color color; 
    public float Depth = 0f; 

    public Outline(float Depth = 0f) {
        this.thickness = 1; 
        this.color = Color.White; 
        this.Depth = Depth; 
    }
    
    public Outline(int thickness, Color color, float Depth = 0f) {
        this.thickness = thickness; 
        this.color = color; 
        this.Depth = Depth;
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