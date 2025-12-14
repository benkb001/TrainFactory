using Color = Microsoft.Xna.Framework.Color; 

//TODO: Test
namespace TrainGame.Components;
public class Background {
    public Color BackgroundColor; 
    public float Depth;

    public Background(Color BackgroundColor, float Depth = 1f) {
        this.BackgroundColor = BackgroundColor; 
        this.Depth = Depth; 
    }
}