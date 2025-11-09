using Color = Microsoft.Xna.Framework.Color; 

namespace TrainGame.Components;
public class Outline {
    private int thickness; 
    private Color color; 
    public float Depth = 0f; 
    
    public Outline(int thickness = 1, float Depth = 0f) : this(Color.White, thickness, Depth) {}

    public Outline(Color color, int thickness = 1, float Depth = 0f) {
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